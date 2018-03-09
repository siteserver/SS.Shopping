using System;
using System.Text;
using SiteServer.Plugin;
using SS.Payment.Core;
using Utils = SS.Shopping.Core.Utils;

namespace SS.Shopping.Parse
{
    public class StlShoppingOrder
    {
        private StlShoppingOrder()
        {
        }

        public const string ElementName = "stl:shoppingOrder";

        public const string AttributeSuccessUrl = "successUrl";
        public const string AttributeWeixinName = "weixinName";

        public static object ApiOrderGet(IRequest context)
        {
            var guid = context.GetPostString("guid");
            var orderInfo = Main.OrderDao.GetOrderInfo(guid);
            if (orderInfo != null)
            {
                orderInfo.CartInfoList = Main.CartDao.GetCartInfoList(orderInfo.Id);
            }

            return new
            {
                context.IsUserLoggin,
                orderInfo
            };
        }

        public static string Parse(IParseContext context)
        {
            var successUrl = string.Empty;
            var weixinName = string.Empty;

            foreach (var attriName in context.StlAttributes.Keys)
            {
                var value = context.StlAttributes[attriName];
                if (Utils.EqualsIgnoreCase(attriName, AttributeSuccessUrl))
                {
                    successUrl = Main.Instance.ParseApi.ParseAttributeValue(value, context);
                }
                else if (Utils.EqualsIgnoreCase(attriName, AttributeWeixinName))
                {
                    weixinName = Main.Instance.ParseApi.ParseAttributeValue(value, context);
                }
            }

            var html = Main.Instance.ParseApi.ParseInnerXml(context.StlInnerXml, context);
            if (string.IsNullOrEmpty(context.StlInnerXml))
            {
                var htmlBuilder = new StringBuilder();
                htmlBuilder.Append(@"
<div class=""order-content order-cont-other"">
  <div class=""order-h3"">
    订单内容
    <a href=""javascript:;"" @click=""openPay()"" class="" go-pay "" v-show=""!orderInfo.isPaied"">继续支付</a>
  </div>
  <div class=""order-table"">
    <div class=""o-t-title"">
      <div class=""o-t-name"">商品名称</div>
      <div class=""o-t-price"">商品单价</div>
      <div class=""o-t-num"">商品数量</div>
      <div class=""o-t-total"">商品总价</div>
    </div>
    <div class=""o-t-list"" v-for=""cartInfo in orderInfo.cartInfoList"">
      <div class=""o-t-name"">
        <a :href=""cartInfo.linkUrl"">
          <div class=""name-img"">
            <img :src=""cartInfo.imageUrl"" />
          </div>
          <p>{{ cartInfo.productName }}</p>
        </a>
      </div>
      <div class=""o-t-price"">
        <span>单价：</span>
        ¥{{ cartInfo.fee.toFixed(2) }}
      </div>
      <div class=""o-t-num"">
        <span>数量：</span>
        {{ cartInfo.count }}
      </div>
      <div class=""o-t-total"">
        <span>总价：</span>
        ¥{{ (cartInfo.fee * cartInfo.count).toFixed(2) }}
      </div>
    </div>
  </div>
  <div class=""order-h3"">
    订单内容
  </div>
  <div class=""order-ul"">
    <ul>
      <li>
        <span>订单编号</span>
        <p>{{ orderInfo.guid }}&nbsp;</p>
      </li>
      <li v-show=""orderInfo.channel"">
        <span>支付方式</span>
        <p>{{ getPayChannel() }}&nbsp;</p>
      </li>
      <li>
        <span>下单时间</span>
        <p>{{ orderInfo.addDate }}&nbsp;</p>
      </li>
    </ul>
  </div>
  <div class=""order-h3"">
    收货人信息
  </div>
  <div class=""order-ul"">
    <ul>
      <li>
        <span>收货人</span>
        <p>{{ orderInfo.realName }}&nbsp;</p>
      </li>
      <li>
        <span>地址</span>
        <p>{{ orderInfo.location + ' ' + orderInfo.address }}&nbsp;</p>
      </li>
      <li>
        <span>联系方式</span>
        <p>{{ orderInfo.mobile }}&nbsp;</p>
      </li>
      <li>
        <span>电子邮件</span>
        <p>{{ orderInfo.email }}&nbsp;</p>
      </li>
    </ul>
  </div>
  <div class=""order-h3"">
    订单备注
  </div>
  <div class=""order-textarea"">
    {{ orderInfo.message || '无' }}
  </div>
  <div class=""order-h3"">
    结算信息
  </div>
  <div class=""order-ul order-p"">
    <ul>
      <li>
        <span>商品金额</span>
        <p>
          ¥{{ (orderInfo.totalFee || 0).toFixed(2) }}元 + 运费：¥{{ (orderInfo.expressCost || 0).toFixed(2) }}元 = 订单总金额：<b>¥{{ (orderInfo.totalFee + orderInfo.expressCost).toFixed(2) }}</b>元
        </p>
      </li>
    </ul>
  </div>
");
                if (!string.IsNullOrEmpty(weixinName))
                {
                    weixinName = $@"<p style=""text-align: center"">{weixinName}</p>";
                }
                htmlBuilder.Append($@"
<div class=""mask1_bg mask1_bg_cut"" v-show=""isPayment || isPaymentSuccess"" @click=""isPayment = isPaymentSuccess = false""></div>
<div class=""detail_alert detail_alert_cut"" v-show=""isPayment"">
  <div class=""close"" @click=""isPayment = isPaymentSuccess = false""></div>
  <div class=""alert_input"">
    金额: ¥{{{{ getOrderFee(orderInfo) }}}}元
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
<div class=""detail_alert detail_alert_cut"" v-show=""isPayment && isWxQrCode"">
  <div class=""close"" @click=""isPayment = isWxQrCode = isPaymentSuccess = false""></div>
  <div class=""pay_list"">
    <p style=""text-align: center""> 打开手机微信，扫一扫下面的二维码，即可完成支付</p>
    {weixinName}
    <p style=""margin-left: 195px;margin-bottom: 80px;""><img :src=""qrCodeUrl"" style=""width: 200px;height: 200px;""></p>
  </div>
</div>
<div class=""detail_alert detail_alert_cut"" v-show=""isPaymentSuccess"">
  <div class=""close"" @click=""isPayment = isWxQrCode = isPaymentSuccess = false""></div>
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

            var paymentApi = new PaymentApi(context.SiteId);

            var jqueryUrl = Main.Instance.PluginApi.GetPluginUrl("assets/js/jquery.min.js");
            var vueUrl = Main.Instance.PluginApi.GetPluginUrl("assets/js/vue.min.js");
            var deviceUrl = Main.Instance.PluginApi.GetPluginUrl("assets/js/device.min.js");
            //var baseCssUrl = Main.Instance.PluginApi.GetPluginUrl("assets/css/base.css");
            var orderCssUrl = Main.Instance.PluginApi.GetPluginUrl("assets/css/order.css");
            var apiGetUrl = Main.Instance.PluginApi.GetPluginApiUrl(nameof(ApiOrderGet));
            var apiPayUrl = Main.Instance.PluginApi.GetPluginApiUrl(nameof(StlShoppingOrders.ApiOrdersPay));
            var apiWeixinIntervalUrl = Main.Instance.PluginApi.GetPluginApiUrl(nameof(StlShoppingPay.ApiPayWeixinInterval));

            html += $@"
<script type=""text/javascript"" src=""{jqueryUrl}""></script>
<script type=""text/javascript"" src=""{vueUrl}""></script>
<script type=""text/javascript"" src=""{deviceUrl}""></script>
<link rel=""stylesheet"" type=""text/css"" href=""{orderCssUrl}"" />
<script type=""text/javascript"">
    var match = location.search.match(new RegExp(""[\?\&]guid=([^\&]+)"", ""i""));
    var guid = (!match || match.length < 1) ? '' : match[1];
    match = location.search.match(new RegExp(""[\?\&]isPaymentSuccess=([^\&]+)"", ""i""));
    var isPaymentSuccess = (!match || match.length < 1) ? false : true;
    var {vueId} = new Vue({{
        el: '#{elementId}',
        data: {{
            isUserLoggin: false,
            orderInfo: {{}},
            isAlipayPc: {paymentApi.IsAlipayPc.ToString().ToLower()},
            isAlipayMobi: {paymentApi.IsAlipayMobi.ToString().ToLower()},
            isWeixin: {paymentApi.IsWeixin.ToString().ToLower()},
            isJdpay: {paymentApi.IsJdpay.ToString().ToLower()},
            isMobile: device.mobile(),
            isPayment: false,
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
            getOrderFee: function() {{
                return (this.orderInfo.totalFee + this.orderInfo.expressCost).toFixed(2);
            }},
            getPayChannel: function() {{
                if (this.orderInfo.channel === 'alipay') return '支付宝支付';
                else if (this.orderInfo.channel === 'weixin') return '微信支付';
                else if (this.orderInfo.channel === 'jdpay') return '京东支付';
                return '';
            }},
            getOrder: function() {{
                $.ajax({{
                    url : ""{apiGetUrl}"",
                    xhrFields: {{
                        withCredentials: true
                    }},
                    type: ""POST"",
                    data: JSON.stringify({{
                        guid: guid
                    }}),
                    contentType: ""application/json; charset=utf-8"",
                    dataType: ""json"",
                    success: function(data)
                    {{
                        {vueId}.isUserLoggin = data.isUserLoggin;
                        {vueId}.orderInfo = data.orderInfo;
                    }},
                    error: function (err)
                    {{
                        var err = JSON.parse(err.responseText);
                        console.log(err.message);
                    }}
                }});
            }},
            openPay: function () {{
                this.isPayment = true;
            }},
            weixinInterval: function(orderNo) {{
                var $this = this;
                var interval = setInterval(function(){{
                    $.ajax({{
                        url : ""{apiWeixinIntervalUrl}"",
                        xhrFields: {{
                            withCredentials: true
                        }},
                        type: ""POST"",
                        data: JSON.stringify({{orderNo: orderNo}}),
                        contentType: ""application/json; charset=utf-8"",
                        dataType: ""json"",
                        success: function(data)
                        {{
                            if (data.isPaied) {{
                                clearInterval(interval);
                                $this.isPayment = $this.isWxQrCode = false;
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
                this.isPayment = this.isWxQrCode = this.isPaymentSuccess = false;
            }},
            pay: function () {{
                var $this = this;

                $.ajax({{
                    url : ""{apiPayUrl}"",
                    xhrFields: {{
                        withCredentials: true
                    }},
                    type: ""POST"",
                    data: JSON.stringify({{
                        siteId: {context.SiteId},
                        channel: this.channel,
                        orderId: this.orderInfo.id,
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
        {vueId}.getOrder();
    }});
</script>
";

            return html;
        }
    }
}
