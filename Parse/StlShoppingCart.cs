using System;
using System.Collections.Generic;
using System.Web;
using SiteServer.Plugin;
using SS.Shopping.Core;
using SS.Shopping.Model;

namespace SS.Shopping.Parse
{
    public static class StlShoppingCart
    {
        public const string ElementName = "stl:shoppingCart";

        private const string AttributeLoginUrl = "loginUrl";
        private const string AttributePayUrl = "payUrl";

        public static object ApiCartGet(IRequest context)
        {
            var siteId = context.GetPostInt("siteId");
            var sessionId = context.GetPostString("sessionId");

            var cartInfoList = Main.CartDao.GetCartInfoList(siteId, context.UserName, sessionId);

            var totalCount = 0;
            decimal totalFee = 0;

            foreach (var cartInfo in cartInfoList)
            {
                totalCount += cartInfo.Count;
                totalFee += cartInfo.Fee * cartInfo.Count;
            }

            var configInfo = Context.ConfigApi.GetConfig<ConfigInfo>(Main.PluginId, siteId) ?? new ConfigInfo();

            return new
            {
                context.IsUserLoggin,
                configInfo.IsForceLogin,
                cartInfoList,
                totalCount,
                totalFee
            };
        }

        public static object ApiCartSave(IRequest context)
        {
            var siteId = context.GetPostInt("siteId");
            var sessionId = context.GetPostString("sessionId");
            var cartInfoList = context.GetPostObject<List<CartInfo>>("cartInfoList");
            Main.CartDao.Delete(siteId, context.UserName, sessionId);
            foreach (var cartInfo in cartInfoList)
            {
                cartInfo.UserName = context.UserName;
                cartInfo.Id = Main.CartDao.Insert(cartInfo);
            }

            return cartInfoList;
        }

        public static string Parse(IParseContext context)
        {
            var payUrl = string.Empty;
            var loginUrl = string.Empty;

            foreach (var attriName in context.StlAttributes.AllKeys)
            {
                var value = context.StlAttributes[attriName];
                if (Utils.EqualsIgnoreCase(attriName, AttributePayUrl))
                {
                    payUrl = Context.ParseApi.ParseAttributeValue(value, context);
                }
                else if (Utils.EqualsIgnoreCase(attriName, AttributeLoginUrl))
                {
                    loginUrl = Context.ParseApi.ParseAttributeValue(value, context);
                }
            }
            if (string.IsNullOrEmpty(loginUrl))
            {
                loginUrl = "/home/#/login";
            }

            var currentUrl = Context.ParseApi.GetCurrentUrl(context);
            var loginToCartUrl = $"{loginUrl}?redirectUrl={HttpUtility.UrlEncode(currentUrl)}";
            var loginToPayUrl = $"{loginUrl}?redirectUrl={HttpUtility.UrlEncode(payUrl)}";

            var html = Context.ParseApi.Parse(context.StlInnerHtml, context);
            if (string.IsNullOrEmpty(context.StlInnerHtml))
            {
                html = $@"
<div class=""cart"">
    <div v-show=""!isUserLoggin && !isForceLogin"" class=""logout"">
        您还没有登录！登录后购物车的商品将保存到您的账号中
        <a href=""{loginToCartUrl}"">立即登录</a>
    </div>
    <div class=""cart_lists"">
        <div class=""cart_all"">
            <label>购物车</label>
        </div>
        <ul class=""cart_ul"">
            <li v-for=""cartInfo in cartInfoList"">
            <a :href=""cartInfo.linkUrl"" class=""img_h2"" target=""_blank"">
                <img :src=""cartInfo.imageUrl"" />
                <h2>{{{{ cartInfo.productName }}}}</h2>
            </a>
            <span class=""cart_price"">¥{{{{ cartInfo.fee.toFixed(2) }}}}</span>
            <div class=""cart_number"">
                <div class=""cart_num_click"">
                <div class=""num_jian"" @click=""minus(cartInfo)"">-</div>
                <div class=""num_text"">{{{{ cartInfo.count }}}}</div>
                <div class=""num_jia"" @click=""plus(cartInfo)"">+</div>
                </div>
            </div>
            <a class=""cart_del"" href=""javascript:;"" @click=""remove(cartInfo)"">删除</a>
            </li>
        </ul>
    </div>
    <div class=""cart_lists cart_bg"">
        <div class=""cart_pay1"">
            应付金额：<b>￥{{{{ totalFee.toFixed(2) }}}}</b>
        </div>
        <a :href=""totalFee > 0 ? ((isForceLogin && !isUserLoggin) ? loginUrl : payUrl) : 'javascript:;'"" class=""cart_btn_all"">结 算</a>
    </div>
</div>
";
            }

            var elementId = "el-" + Guid.NewGuid();
            var vueId = "v" + Guid.NewGuid().ToString().Replace("-", string.Empty);
            html = $@"<div id=""{elementId}"">{html}</div>";

            var pluginUrl = Context.PluginApi.GetPluginUrl(Main.PluginId);
            var apiUrl = Context.PluginApi.GetPluginApiUrl(Main.PluginId);

            var jqueryUrl = $"{pluginUrl}/assets/js/jquery.min.js";
            var utilsUrl = $"{pluginUrl}/assets/js/utils.js";
            var vueUrl = $"{pluginUrl}/assets/js/vue.min.js";
            var apiGetUrl = $"{apiUrl}/{nameof(ApiCartGet)}";
            var apiSaveUrl = $"{apiUrl}/{nameof(ApiCartSave)}";

            html += $@"
<script type=""text/javascript"" src=""{jqueryUrl}""></script>
<script type=""text/javascript"" src=""{utilsUrl}""></script>
<script type=""text/javascript"" src=""{vueUrl}""></script>
<script type=""text/javascript"">
    var sessionId = shoppingGetSessionId();
    var {vueId} = new Vue({{
        el: '#{elementId}',
        data: {{
            isUserLoggin: false,
            isForceLogin: false,
            cartInfoList: [],
            totalCount: 0,
            totalFee: 0,
            payUrl: '{payUrl}',
            loginUrl: '{loginToPayUrl}',
        }},
        methods: {{
            plus: function (cartInfo) {{
                cartInfo.count += 1;
                this.totalFee += cartInfo.fee;
                this.save();
            }},
            minus: function (cartInfo) {{
                if (cartInfo.count <= 1) return;
                cartInfo.count -= 1;
                this.totalFee -= cartInfo.fee;
                this.save();
            }},
            remove: function (cartInfo) {{
                var index = this.cartInfoList.indexOf(cartInfo);
                if (index > -1) {{
                    this.cartInfoList.splice(index, 1);
                    this.totalFee -= cartInfo.fee * cartInfo.count;
                    this.save();
                }}
            }},
            save: function () {{
                $.ajax({{
                    url : ""{apiSaveUrl}"",
                    xhrFields: {{
                        withCredentials: true
                    }},
                    type: ""POST"",
                    data: JSON.stringify({{
                        siteId: '{context.SiteId}',
                        sessionId: sessionId,
                        cartInfoList: this.cartInfoList
                    }}),
                    contentType: ""application/json; charset=utf-8"",
                    dataType: ""json"",
                    success: function(data)
                    {{
                        console.log('saved');
                    }},
                    error: function (err)
                    {{
                        var err = JSON.parse(err.responseText);
                        console.log(err.message);
                    }}
                }});
            }},
        }}
    }});
    $(document).ready(function(){{
        $.ajax({{
            url : ""{apiGetUrl}"",
            xhrFields: {{
                withCredentials: true
            }},
            type: ""POST"",
            data: JSON.stringify({{
                siteId: '{context.SiteId}',
                sessionId: sessionId
            }}),
            contentType: ""application/json; charset=utf-8"",
            dataType: ""json"",
            success: function(data)
            {{
                {vueId}.isUserLoggin = data.isUserLoggin;
                {vueId}.isForceLogin = data.isForceLogin;
                {vueId}.cartInfoList = data.cartInfoList;
                {vueId}.totalCount = data.totalCount;
                {vueId}.totalFee = data.totalFee;
            }},
            error: function (err)
            {{
                var err = JSON.parse(err.responseText);
                console.log(err.message);
            }}
        }});
    }});
</script>
";

            return html;
        }
    }
}
