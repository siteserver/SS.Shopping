using SiteServer.Plugin;
using SS.Shopping.Core;
using SS.Shopping.Provider;

namespace SS.Shopping.Parse
{
    public static class StlShoppingAddSuccess
    {
        public const string ElementName = "stl:shoppingAddSuccess";

        public const string AttributeCartUrl = "cartUrl";
        public const string AttributeContinueUrl = "continueUrl";

        public static object ApiAddSuccessGet(IRequest context)
        {
            var siteId = context.GetPostInt("siteId");
            var sessionId = context.GetPostString("sessionId");

            var cartInfoList = CartDao.GetCartInfoList(siteId, context.UserName, sessionId);

            var productName = string.Empty;
            decimal productFee = 0;
            var totalCount = 0;
            decimal totalFee = 0;

            foreach (var cartInfo in cartInfoList)
            {
                if (string.IsNullOrEmpty(productName))
                {
                    productName = cartInfo.ProductName;
                    productFee = cartInfo.Fee;
                }
                totalCount += cartInfo.Count;
                totalFee += cartInfo.Fee * cartInfo.Count;
            }

            return new
            {
                productName,
                productFee = "¥" + productFee.ToString("N"),
                totalCount,
                totalFee = "¥" + totalFee.ToString("N")
            };
        }

        public static string Parse(IParseContext context)
        {
            var cartUrl = string.Empty;
            var continueUrl = string.Empty;

            foreach (var attriName in context.StlAttributes.AllKeys)
            {
                var value = context.StlAttributes[attriName];

                if (Utils.EqualsIgnoreCase(attriName, AttributeCartUrl))
                {
                    cartUrl = Context.ParseApi.ParseAttributeValue(value, context);
                }
                else if (Utils.EqualsIgnoreCase(attriName, AttributeContinueUrl))
                {
                    continueUrl = Context.ParseApi.ParseAttributeValue(value, context);
                }
            }

            if (string.IsNullOrEmpty(continueUrl))
            {
                continueUrl = Context.SiteApi.GetSiteUrl(context.SiteId);
            }

            string template;
            if (!string.IsNullOrEmpty(context.StlInnerHtml))
            {
                template = Context.ParseApi.Parse(context.StlInnerHtml, context);
            }
            else
            {
                template = $@"
<div class=""add_cart"">
  <div class=""add_cart_left"">
    <div class=""cart_title"">
      商品已成功添加至购物车！
    </div>
    <div class=""cart_list"">
      <h2 id=""productName""></h2>
      <span id=""productFee""></span>
      <i>X 1</i>
    </div>
  </div>
  <div class=""add_cart_right"">
    <div class=""cart_num"">
      购物车中已有<span id=""totalCount""></span>件商品
    </div>
    <div class=""cart_pay"">
      应付总额（不含运费）：<span id=""totalFee""></span>
    </div>
    <div class=""cart_btn_bg"">
      <a href=""{cartUrl}"" class=""cart_btn"">去购物车结算</a>
      <a href=""{continueUrl}"" class=""shoping_btn"">继续购物</a>
    </div>
  </div>
</div>
";
            }

            var pluginUrl = Context.PluginApi.GetPluginUrl(Main.PluginId);
            var apiUrl = Context.PluginApi.GetPluginApiUrl(Main.PluginId);

            var jqueryUrl = $"{pluginUrl}/assets/js/jquery.min.js";
            var utilsUrl = $"{pluginUrl}/assets/js/utils.js";
            var apiAddSuccessGetUrl = $"{apiUrl}/{nameof(ApiAddSuccessGet)}";

            return $@"
{template}
<script type=""text/javascript"" src=""{jqueryUrl}""></script>
<script type=""text/javascript"" src=""{utilsUrl}""></script>
<script type=""text/javascript""> 
    $(document).ready(function(){{
        var sessionId = shoppingGetSessionId();
        $.ajax({{
            url : ""{apiAddSuccessGetUrl}"",
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
                $('#productName').html(data.productName);
                $('#productFee').html(data.productFee);
                $('#totalCount').html(data.totalCount);
                $('#totalFee').html(data.totalFee);
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
