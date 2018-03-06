using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using SiteServer.Plugin;
using SS.Shopping.Core;
using SS.Shopping.Model;

namespace SS.Shopping.Parse
{
    public class StlShoppingAdd
    {
        private StlShoppingAdd()
        {
        }

        public const string ElementName = "stl:shoppingAdd";

        public const string AttributeSuccessUrl = "successUrl";

        public static object ApiAdd(IRequest context)
        {
            var siteId = context.GetPostInt("siteId");
            var productId = context.GetPostString("productId");
            var productName = context.GetPostString("productName");
            var imageUrl = context.GetPostString("imageUrl");
            var linkUrl = context.GetPostString("linkUrl");
            var fee = context.GetPostDecimal("fee");
            var isDelivery = context.GetPostBool("isDelivery");
            var count = context.GetPostInt("count", 1);
            var sessionId = context.GetPostString("sessionId");

            var cartInfo = new CartInfo
            {
                SiteId = siteId,
                UserName = context.UserName,
                SessionId = sessionId,
                ProductId = productId,
                ProductName = productName,
                ImageUrl = imageUrl,
                LinkUrl = linkUrl,
                Fee = fee,
                IsDelivery = isDelivery,
                Count = count,
                AddDate = DateTime.Now
            };

            var cartId = Main.CartDao.GetCartId(siteId, sessionId, productId);
            if (cartId == 0)
            {
                cartId = Main.CartDao.Insert(cartInfo);
            }
            else
            {
                cartInfo = Main.CartDao.GetCartInfo(cartId);
                cartInfo.UserName = context.UserName;
                cartInfo.ProductName = productName;
                cartInfo.ImageUrl = imageUrl;
                cartInfo.LinkUrl = linkUrl;
                cartInfo.Fee = fee;
                cartInfo.IsDelivery = isDelivery;
                cartInfo.Count += count;
                cartInfo.AddDate = DateTime.Now;
                Main.CartDao.Update(cartInfo);
            }

            return cartId;
        }

        public static string Parse(IParseContext context)
        {
            var productId = string.Empty;
            var productName = string.Empty;
            var imageUrl = string.Empty;
            var linkUrl = string.Empty;
            decimal fee = 0;
            var isDelivery = true;
            int count = 1;
            var successUrl = string.Empty;

            foreach (var attriName in context.StlAttributes.Keys)
            {
                var value = context.StlAttributes[attriName];
                if (Utils.EqualsIgnoreCase(attriName, nameof(CartInfo.ProductId)))
                {
                    productId = Main.Instance.ParseApi.ParseAttributeValue(value, context);
                }
                else if (Utils.EqualsIgnoreCase(attriName, nameof(CartInfo.ProductName)))
                {
                    productName = Main.Instance.ParseApi.ParseAttributeValue(value, context);
                }
                else if (Utils.EqualsIgnoreCase(attriName, nameof(CartInfo.ImageUrl)))
                {
                    imageUrl = Main.Instance.ParseApi.ParseAttributeValue(value, context);
                }
                else if (Utils.EqualsIgnoreCase(attriName, nameof(CartInfo.LinkUrl)))
                {
                    linkUrl = Main.Instance.ParseApi.ParseAttributeValue(value, context);
                }
                else if (Utils.EqualsIgnoreCase(attriName, nameof(CartInfo.Fee)))
                {
                    value = Main.Instance.ParseApi.ParseAttributeValue(value, context);
                    decimal.TryParse(value, out fee);
                }
                else if (Utils.EqualsIgnoreCase(attriName, nameof(CartInfo.IsDelivery)))
                {
                    value = Main.Instance.ParseApi.ParseAttributeValue(value, context);
                    bool.TryParse(value, out isDelivery);
                }
                else if (Utils.EqualsIgnoreCase(attriName, nameof(CartInfo.Count)))
                {
                    value = Main.Instance.ParseApi.ParseAttributeValue(value, context);
                    int.TryParse(value, out count);
                }
                else if (Utils.EqualsIgnoreCase(attriName, AttributeSuccessUrl))
                {
                    successUrl = Main.Instance.ParseApi.ParseAttributeValue(value, context);
                }
            }

            var stlAnchor = new HtmlAnchor();

            foreach (var attributeName in context.StlAttributes.Keys)
            {
                stlAnchor.Attributes.Add(attributeName, context.StlAttributes[attributeName]);
            }

            stlAnchor.InnerHtml = Main.Instance.ParseApi.ParseInnerXml(context.StlInnerXml, context);
            stlAnchor.HRef = "javascript:;";

            var jqueryUrl = Main.Instance.PluginApi.GetPluginUrl("assets/js/jquery.min.js");
            var utilsUrl = Main.Instance.PluginApi.GetPluginUrl("assets/js/utils.js");
            var apiUrl = Main.Instance.PluginApi.GetPluginApiUrl(nameof(ApiAdd));

            var script = $@"
<script type=""text/javascript"" src=""{jqueryUrl}""></script>
<script type=""text/javascript"" src=""{utilsUrl}""></script>
<script type=""text/javascript"">
    function addToCart_{productId}(){{
        var sessionId = shoppingGetSessionId();
        $.ajax({{
            url : ""{apiUrl}"",
            type: ""POST"",
            data: JSON.stringify({{
                siteId: '{context.SiteId}',
                productId: '{productId}',
                productName: '{productName}',
                imageUrl: '{imageUrl}',
                linkUrl: '{linkUrl}',
                fee: {fee},
                isDelivery: {isDelivery.ToString().ToLower()},
                count: {count},
                sessionId: sessionId
            }}),
            contentType: ""application/json; charset=utf-8"",
            dataType: ""json"",
            success: function(data)
            {{
                location.href = '{successUrl}';
            }},
            error: function (err)
            {{
                var err = JSON.parse(err.responseText);
                console.log(err.message);
            }}
        }});
    }}
</script>
";
            stlAnchor.Attributes["onclick"] = $"addToCart_{productId}();return false;";

            return $"{script}{GetControlRenderHtml(stlAnchor)}";
        }

        private static string GetControlRenderHtml(Control control)
        {
            var builder = new StringBuilder();
            var sw = new System.IO.StringWriter(builder);
            var htw = new HtmlTextWriter(sw);
            control.RenderControl(htw);
            return builder.ToString();
        }
    }
}
