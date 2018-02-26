using System;
using SiteServer.Plugin;
using SS.Shopping.Core;

namespace SS.Shopping.Parse
{
    public class StlShoppingPaySuccess
    {
        private StlShoppingPaySuccess()
        {
        }

        public const string ElementName = "stl:shoppingPaySuccess";

        public const string AttributeOrderUrl = "orderUrl";

        public static object ApiPaySuccessGet(IRequest context)
        {
            var guid = context.GetPostString("guid");
            Main.OrderDao.UpdateIsPaied(guid);
            return Main.OrderDao.GetOrderInfo(guid);
        }

        public static string Parse(IParseContext context)
        {
            var orderUrl = string.Empty;

            foreach (var attriName in context.StlAttributes.Keys)
            {
                var value = context.StlAttributes[attriName];
                if (Utils.EqualsIgnoreCase(attriName, AttributeOrderUrl))
                {
                    orderUrl = Main.Instance.ParseApi.ParseAttributeValue(value, context);
                }
            }

            string template;
            if (!string.IsNullOrEmpty(context.StlInnerXml))
            {
                template = Main.Instance.ParseApi.ParseInnerXml(context.StlInnerXml, context);
            }
            else
            {
                template = @"
<div class=""cashier"">
    <div class=""cashier_title"">
        感谢您提交订单，我们将及时为您处理！
    </div>
    <div class=""cashier_next"">
        <div class=""cashier_number"">
            订单：<span>{{ orderInfo.guid }}</span>
        </div>
        <div class=""cashier_text"">
            <span>{{ orderInfo.totalCount }}件商品</span><span>收件人：{{ orderInfo.realName }}</span>
        </div>
        <div class=""cashier_text1"">
            
        </div>
    </div>
    <div class=""cashier_alert"">
        <span>安全提醒：</span>平台及销售商不会以订单异常、系统升级等理由，通过任何方式发送给您退款链接。请您谨防钓鱼链接或诈骗电话！
    </div>
    <div class=""cashier_pay"">
        支付金额：<span>¥{{ (orderInfo.totalFee + orderInfo.expressCost).toFixed(2) }}</span>
    </div>
    <div class=""view_order_bg"">
        <a href=""javascript:;"" @click=""viewOrder"" class=""view_order"">查看订单</a>
    </div>
</div>
";
            }           

            var elementId = "el-" + Guid.NewGuid();
            var vueId = "v" + Guid.NewGuid().ToString().Replace("-", string.Empty);
            var jqueryUrl = Main.Instance.PluginApi.GetPluginUrl("assets/js/jquery.min.js");
            var vueUrl = Main.Instance.PluginApi.GetPluginUrl("assets/js/vue.min.js");
            var baseCssUrl = Main.Instance.PluginApi.GetPluginUrl("assets/css/base.css");
            var apiGetUrl = Main.Instance.PluginApi.GetPluginApiUrl(nameof(ApiPaySuccessGet));

            return $@"
<script type=""text/javascript"" src=""{jqueryUrl}""></script>
<script type=""text/javascript"" src=""{vueUrl}""></script>
<link rel=""stylesheet"" type=""text/css"" href=""{baseCssUrl}"" />
<div id=""{elementId}"">
    {template}
</div>
<script type=""text/javascript"">
    var match = location.search.match(new RegExp(""[\?\&]guid=([^\&]+)"", ""i""));
    var guid = (!match || match.length < 1) ? """" : decodeURIComponent(match[1]);
    var {vueId} = new Vue({{
        el: '#{elementId}',
        data: {{
            orderInfo: {{}}
        }},
        methods: {{
            viewOrder: function () {{
                location.href = '{orderUrl}?guid=' + guid;
            }}
        }}
    }});
    $(document).ready(function(){{
        if (!guid) return;
        $.ajax({{
            url : ""{apiGetUrl}"",
            type: ""POST"",
            data: JSON.stringify({{
                guid: guid
            }}),
            contentType: ""application/json; charset=utf-8"",
            dataType: ""json"",
            success: function(data)
            {{
                {vueId}.orderInfo = data;
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
        }
    }
}
