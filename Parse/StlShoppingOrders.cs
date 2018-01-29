using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using SiteServer.Plugin;
using SS.Payment.Core;
using SS.Shopping.Model;
using Utils = SS.Shopping.Core.Utils;

namespace SS.Shopping.Parse
{
    public class StlShoppingOrders
    {
        private StlShoppingOrders()
        {
        }

        public const string ElementName = "stl:shoppingOrders";

        public const string AttributeSuccessUrl = "successUrl";
        public const string AttributeOrderUrl = "orderUrl";
        public const string AttributeWeixinName = "weixinName";

        public static object ApiOrdersGet(IRequest context)
        {
            var type = context.GetPostString("type");

            var orderInfoList = new List<OrderInfo>();
            if (context.IsUserLoggin)
            {
                orderInfoList = string.IsNullOrEmpty(type)
                    ? Main.OrderDao.GetOrderInfoList(context.UserName, string.Empty)
                    : Main.OrderDao.GetOrderInfoList(context.UserName, Convert.ToBoolean(type));

                foreach (var orderInfo in orderInfoList)
                {
                    orderInfo.CartInfoList = Main.CartDao.GetCartInfoList(orderInfo.Id);
                }
            }

            return new
            {
                context.IsUserLoggin,
                orderInfoList
            };
        }

        public static object ApiOrdersRemove(IRequest context)
        {
            if (context.IsUserLoggin)
            {
                var orderId = context.GetPostInt("orderId");
                Main.OrderDao.Delete(orderId);
            }

            return new { };
        }

        public static object ApiOrdersPay(IRequest context)
        {
            if (!context.IsUserLoggin) return null;

            var siteId = context.GetPostInt("siteId");
            var orderId = context.GetPostInt("orderId");
            var channel = context.GetPostString("channel");
            var isMobile = context.GetPostBool("isMobile");
            var successUrl = context.GetPostString("successUrl");
            if (string.IsNullOrEmpty(successUrl))
            {
                successUrl = Main.FilesApi.GetSiteUrl(siteId);
            }

            var siteInfo = Main.SiteApi.GetSiteInfo(siteId);
            var orderInfo = Main.OrderDao.GetOrderInfo(orderId);
            orderInfo.Channel = channel;

            var paymentApi = new PaymentApi(siteId);

            var amount = orderInfo.TotalFee;
            var orderNo = orderInfo.Guid;
            successUrl = $"{successUrl}?guid={orderNo}";
            if (channel == "alipay")
            {
                return isMobile
                    ? paymentApi.ChargeByAlipayMobi(siteInfo.SiteName, amount, orderNo, successUrl)
                    : paymentApi.ChargeByAlipayPc(siteInfo.SiteName, amount, orderNo, successUrl);
            }
            if (channel == "weixin")
            {
                var notifyUrl = Main.FilesApi.GetApiHttpUrl(nameof(StlShoppingPay.ApiPayWeixinNotify), orderNo);
                var url = HttpUtility.UrlEncode(paymentApi.ChargeByWeixin(siteInfo.SiteName, amount, orderNo, notifyUrl));
                var qrCodeUrl =
                    $"{Main.FilesApi.GetApiHttpUrl(nameof(StlShoppingPay.ApiPayQrCode))}?qrcode={url}";
                return new
                {
                    qrCodeUrl,
                    orderNo
                };
            }
            if (channel == "jdpay")
            {
                return paymentApi.ChargeByJdpay(siteInfo.SiteName, amount, orderNo, successUrl);
            }

            return null;
        }

        public static string Parse(IParseContext context)
        {
            var successUrl = string.Empty;
            var orderUrl = string.Empty;
            var weixinName = string.Empty;

            foreach (var attriName in context.Attributes.Keys)
            {
                var value = context.Attributes[attriName];
                if (Utils.EqualsIgnoreCase(attriName, AttributeSuccessUrl))
                {
                    successUrl = Main.ParseApi.ParseAttributeValue(value, context);
                }
                else if (Utils.EqualsIgnoreCase(attriName, AttributeOrderUrl))
                {
                    orderUrl = Main.ParseApi.ParseAttributeValue(value, context);
                }
                else if (Utils.EqualsIgnoreCase(attriName, AttributeWeixinName))
                {
                    weixinName = Main.ParseApi.ParseAttributeValue(value, context);
                }
            }

            var paymentApi = new PaymentApi(context.SiteId);

            var html = Main.ParseApi.ParseInnerXml(context.InnerXml, context);
            if (string.IsNullOrEmpty(context.InnerXml))
            {
                if (!string.IsNullOrEmpty(weixinName))
                {
                    weixinName = $@"<p style=""text-align: center"">{weixinName}</p>";
                }

                var htmlBuilder = new StringBuilder();
                htmlBuilder.Append(@"
<div class=""order-content"" v-show=""isUserLoggin"">
  <div class=""order-lists-a"">
    <a href=""javascript:;"" :class=""{ a_cut: type == '' }"" @click=""setOrderType('')"">全部订单</a>
    <a href=""javascript:;"" :class=""{ a_cut: type == 'True' }"" @click=""setOrderType('True')"">已支付</a>
    <a href=""javascript:;"" :class=""{ a_cut: type == 'False' }"" @click=""setOrderType('False')"">未支付</a>
  </div>

  <div v-for=""orderInfo in orderInfoList"">

    <div class="" order-sub "">
      <div class="" o-s-date "">
        {{ orderInfo.addDate }}
      </div>
      <div class="" o-s-id "">
        订单号：{{ orderInfo.guid }}
      </div>
      <div class="" o-s-total "">
        订单金额:
        <b>¥{{ getOrderFee(orderInfo) }}元</b>
      </div>
      <div class="" clear ""></div>
      <div class="" o-s-line ""></div>
      <div class="" o-s-person "">
        收货人：{{ orderInfo.realName }}
      </div>
      <div class="" o-s-address "">
        收货地址：{{ orderInfo.location + ' ' + orderInfo.address }}
      </div>
    </div>

    <div class="" order-table order-lists "">
      <div class="" o-t-title "">
        <div class="" o-t-name "">商品名称</div>
        <div class="" o-t-num "">数量</div>
        <div class="" o-t-price "">金额</div>
        <div class="" o-t-state "">状态</div>
        <div class="" o-t-operate "">操作</div>
      </div>
      <div class="" o-t-list "">
        <div class="" o-t-texts "">
          <ul>
            <li v-for=""cartInfo in orderInfo.cartInfoList"">
              <div class="" o-t-name "">
                <a :href=""cartInfo.linkUrl"">
                  <div class="" name-img "">
                    <img :src=""cartInfo.imageUrl"">
                  </div>
                  <p>{{ cartInfo.productName }}</p>
                </a>
              </div>
              <div class="" o-t-num "">
                <span>数量：</span>
                {{ cartInfo.count }}
              </div>
              <div class="" o-t-price "">
                <span>金额：</span>
                ¥{{ cartInfo.fee.toFixed(2) }}
              </div>
            </li>
          </ul>
        </div>
        <div class="" o-t-state "">
          <span>状态：</span>
            {{ getStateText(orderInfo.isPaied, orderInfo.state) }}
        </div>
        <div class="" o-t-operate "">
          <a href=""javascript:;"" @click=""openPay(orderInfo)"" class="" go-pay "" v-show=""!orderInfo.isPaied"">继续支付</a>
          <a href=""javascript:;"" class="" t-o-link "" @click=""viewOrder(orderInfo)"">查看</a>
          <a href=""javascript:;"" class="" t-o-link "" @click=""removeOrder(orderInfo)"">删除</a>
        </div>
      </div>
    </div>

  </div>

  <div class=""pages"" style=""display: none"">
    <a href=""#"" class=""page_f"">首页</a>
    <a href=""#"" class=""page_f"">上一页</a>
    <a href=""#"" class=""page_cut"">1</a>
    <a href=""#"">2</a>
    <a href=""#"">3</a>
    <a href=""#"">4</a>
    <a href=""#"">5</a>
    <a href=""#"" class=""page_f"">下一页</a>
    <a href=""#"" class=""page_f"">尾页</a>
  </div>
");
                htmlBuilder.Append($@"
<div class=""mask1_bg mask1_bg_cut"" v-show=""orderInfoToPay || isPaymentSuccess"" @click=""orderInfoToPay = isPaymentSuccess = false""></div>
<div class=""detail_alert detail_alert_cut"" v-show=""orderInfoToPay"">
  <div class=""close"" @click=""orderInfoToPay = isPaymentSuccess = false""></div>
  <div class=""alert_input"">
    金额: ¥{{{{ getOrderFee(orderInfoToPay) }}}}元
  </div>
  <div class=""pay_list"">
    <p>支付方式</p>
    <ul>
        <li v-show=""(isAlipayPc && !isMobile) || (isAlipayMobi && isMobile)"" :class=""{{ pay_cut: channel === 'alipay' }}"" @click=""channel = 'alipay'"" class=""channel_alipay""><b></b></li>
        <li v-show=""isWeixin"" :class=""{{ pay_cut: channel === 'weixin' }}"" @click=""channel = 'weixin'"" class=""channel_weixin""><b></b></li>
        <li v-show=""isJdpay"" :class=""{{ pay_cut: channel === 'jdpay' }}"" @click=""channel = 'jdpay'"" class=""channel_jdpay""><b></b></li>
    </ul>
    <div class=""mess_text""></div>
    <a href=""javascript:;"" @click=""pay"" class=""pay_go"">立即支付</a>
  </div>
</div>
<div class=""detail_alert detail_alert_cut"" v-show=""orderInfoToPay && isWxQrCode"">
  <div class=""close"" @click=""orderInfoToPay = isWxQrCode = isPaymentSuccess = false""></div>
  <div class=""pay_list"">
    <p style=""text-align: center""> 打开手机微信，扫一扫下面的二维码，即可完成支付</p>
    {weixinName}
    <p style=""margin-left: 195px;margin-bottom: 80px;""><img :src=""qrCodeUrl"" style=""width: 200px;height: 200px;""></p>
  </div>
</div>
<div class=""detail_alert detail_alert_cut"" v-show=""isPaymentSuccess"">
  <div class=""close"" @click=""orderInfoToPay = isWxQrCode = isPaymentSuccess = false""></div>
  <div class=""pay_list"">
    <p style=""text-align: center"">支付成功，谢谢支持</p>
    <div class=""mess_text""></div>
    <a href=""javascript:;"" @click=""weixinPaiedClose"" class=""pay_go"">关闭</a>
  </div>
</div>
");
                htmlBuilder.Append("</div>");

                html = htmlBuilder.ToString();
            }

            var elementId = "el-" + Guid.NewGuid();
            var vueId = "v" + Guid.NewGuid().ToString().Replace("-", string.Empty);
            html = $@"<div id=""{elementId}"" class=""shopping_order"">{html}</div>";

            var jqueryUrl = Main.FilesApi.GetPluginUrl("assets/js/jquery.min.js");
            var vueUrl = Main.FilesApi.GetPluginUrl("assets/js/vue.min.js");
            var deviceUrl = Main.FilesApi.GetPluginUrl("assets/js/device.min.js");
            var baseCssUrl = Main.FilesApi.GetPluginUrl("assets/css/base.css");
            var orderCssUrl = Main.FilesApi.GetPluginUrl("assets/css/order.css");
            var apiGetUrl = Main.FilesApi.GetApiJsonUrl(nameof(ApiOrdersGet));
            var apiRemoveOrderUrl = Main.FilesApi.GetApiJsonUrl(nameof(ApiOrdersRemove));
            var apiPayUrl = Main.FilesApi.GetApiJsonUrl(nameof(ApiOrdersPay));
            var apiWeixinIntervalUrl = Main.FilesApi.GetApiJsonUrl(nameof(StlShoppingPay.ApiPayWeixinInterval));

            html += $@"
<script type=""text/javascript"" src=""{jqueryUrl}""></script>
<script type=""text/javascript"" src=""{vueUrl}""></script>
<script type=""text/javascript"" src=""{deviceUrl}""></script>
<link rel=""stylesheet"" type=""text/css"" href=""{baseCssUrl}"" />
<link rel=""stylesheet"" type=""text/css"" href=""{orderCssUrl}"" />
<script type=""text/javascript"">
    var match = location.search.match(new RegExp(""[\?\&]isPaymentSuccess=([^\&]+)"", ""i""));
    var isPaymentSuccess = (!match || match.length < 1) ? false : true;
    var {vueId} = new Vue({{
        el: '#{elementId}',
        data: {{
            type: '',
            isUserLoggin: false,
            orderInfoList: [],
            orderInfoToPay: null,
            isAlipayPc: {paymentApi.IsAlipayPc.ToString().ToLower()},
            isAlipayMobi: {paymentApi.IsAlipayMobi.ToString().ToLower()},
            isWeixin: {paymentApi.IsWeixin.ToString().ToLower()},
            isJdpay: {paymentApi.IsJdpay.ToString().ToLower()},
            isMobile: device.mobile(),
            channel: 'alipay',
            isWxQrCode: false,
            isPaymentSuccess: isPaymentSuccess,
            qrCodeUrl: ''
        }},
        methods: {{
            getStateText: function (isPaied, state) {{
                if (!isPaied) return '未支付';
                if (state == 'Done') return '已完成';
                return '已支付';
            }},
            getOrderFee: function(orderInfo) {{
                if (!orderInfo) return '';
                return (orderInfo.totalFee + orderInfo.expressCost).toFixed(2);
            }},
            setOrderType: function(type) {{
                {vueId}.type = type;
                {vueId}.orderInfoList = [];
                $.ajax({{
                    url : ""{apiGetUrl}"",
                    type: ""POST"",
                    data: JSON.stringify({{
                        type: type
                    }}),
                    contentType: ""application/json; charset=utf-8"",
                    dataType: ""json"",
                    success: function(data)
                    {{
                        {vueId}.isUserLoggin = data.isUserLoggin;
                        {vueId}.orderInfoList = data.orderInfoList;
                    }},
                    error: function (err)
                    {{
                        var err = JSON.parse(err.responseText);
                        console.log(err.message);
                    }}
                }});
            }},
            viewOrder: function(orderInfo) {{
                location.href = '{orderUrl}?guid=' + orderInfo.guid;
            }},
            removeOrder: function (orderInfo) {{
                if (!confirm(""此操作将删除订单，确认吗？"")) return false;
                var index = this.orderInfoList.indexOf(orderInfo);
                if (index > -1) {{
                    this.orderInfoList.splice(index, 1);
                    $.ajax({{
                        url : ""{apiRemoveOrderUrl}"",
                        type: ""POST"",
                        data: JSON.stringify({{
                            orderId: orderInfo.id
                        }}),
                        contentType: ""application/json; charset=utf-8"",
                        dataType: ""json"",
                        success: function(data)
                        {{
                            console.log('removed');
                        }},
                        error: function (err)
                        {{
                            var err = JSON.parse(err.responseText);
                            console.log(err.message);
                        }}
                    }});
                }}
            }},
            openPay: function (orderInfo) {{
                this.orderInfoToPay = orderInfo;
            }},
            weixinInterval: function(orderNo) {{
                var $this = this;
                var interval = setInterval(function(){{
                    $.ajax({{
                        url : ""{apiWeixinIntervalUrl}"",
                        type: ""POST"",
                        data: JSON.stringify({{orderNo: orderNo}}),
                        contentType: ""application/json; charset=utf-8"",
                        dataType: ""json"",
                        success: function(data)
                        {{
                            if (data.isPaied) {{
                                clearInterval(interval);
                                $this.orderInfoToPay = $this.isWxQrCode = false;
                                $this.isPaymentSuccess = true;
                            }}
                        }},
                        error: function (err)
                        {{
                            var err = JSON.parse(err.responseText);
                            console.log(err.message);
                        }}
                    }});
                }}, 3000);
            }},
            weixinPaiedClose: function() {{
                this.orderInfoToPay = this.isWxQrCode = this.isPaymentSuccess = false;
            }},
            pay: function () {{
                var $this = this;

                $.ajax({{
                    url : ""{apiPayUrl}"",
                    type: ""POST"",
                    data: JSON.stringify({{
                        siteId: {context.SiteId},
                        channel: this.channel,
                        orderId: this.orderInfoToPay.id,
                        isMobile: this.isMobile,
                        successUrl: '{successUrl}'
                    }}),
                    contentType: ""application/json; charset=utf-8"",
                    dataType: ""json"",
                    success: function(charge)
                    {{
                        if ($this.channel === 'weixin') {{
                            $this.isWxQrCode = true;
                            $this.qrCodeUrl = charge.qrCodeUrl;
                            $this.weixinInterval(charge.orderNo);
                        }} else {{
                            document.write(charge);
                        }}
                    }},
                    error: function (err)
                    {{
                        var err = JSON.parse(err.responseText);
                        console.log(err.message);
                    }}
                }});
            }}
        }}
    }});
    $(document).ready(function(){{
        {vueId}.setOrderType('');
    }});
</script>
";

            return html;
        }
    }
}
