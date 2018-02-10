using SiteServer.Plugin;
using SS.Shopping.Core;

namespace SS.Shopping.Parse
{
    public class StlShoppingAddSuccess
    {
        private StlShoppingAddSuccess()
        {
        }

        public const string ElementName = "stl:shoppingAddSuccess";

        public const string AttributeCartUrl = "cartUrl";
        public const string AttributeContinueUrl = "continueUrl";

        public static object ApiAddSuccessGet(IRequest context)
        {
            var siteId = context.GetPostInt("siteId");
            var sessionId = context.GetPostString("sessionId");

            var cartInfoList = Main.CartDao.GetCartInfoList(siteId, context.UserName, sessionId);

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

            foreach (var attriName in context.StlElementAttributes.Keys)
            {
                var value = context.StlElementAttributes[attriName];

                if (Utils.EqualsIgnoreCase(attriName, AttributeCartUrl))
                {
                    cartUrl = Main.Instance.ParseApi.ParseAttributeValue(value, context);
                }
                else if (Utils.EqualsIgnoreCase(attriName, AttributeContinueUrl))
                {
                    continueUrl = Main.Instance.ParseApi.ParseAttributeValue(value, context);
                }
            }

            if (string.IsNullOrEmpty(continueUrl))
            {
                continueUrl = Main.Instance.FilesApi.GetSiteUrl(context.SiteId);
            }

            string template;
            if (!string.IsNullOrEmpty(context.StlElementInnerXml))
            {
                template = Main.Instance.ParseApi.ParseInnerXml(context.StlElementInnerXml, context);
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

            var jqueryUrl = Main.Instance.PluginApi.GetPluginUrl("assets/js/jquery.min.js");
            var apiUrl = Main.Instance.PluginApi.GetPluginApiUrl(nameof(ApiAddSuccessGet));

            return $@"
{template}
<script type=""text/javascript"" src=""{jqueryUrl}""></script>
<script type=""text/javascript""> 
    function shoppingGuid() {{
      return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {{
        var r = Math.random()*16|0, v = c == 'x' ? r : (r&0x3|0x8);
        return v.toString(16);
      }});
    }}
    function shoppingSetCookie(cname, cvalue) {{
        var d = new Date();  
        d.setTime(d.getTime() + (24*60*60*1000));
        var expires = ""expires=""+d.toUTCString();  
        document.cookie = cname + ""="" + cvalue + ""; "" + expires;  
    }}
    function shoppingGetCookie(cname) {{
        var name = cname + ""="";  
        var ca = document.cookie.split(';');  
        for(var i=0; i<ca.length; i++) {{
            var c = ca[i];  
            while (c.charAt(0)==' ') c = c.substring(1);  
            if (c.indexOf(name) != -1) return c.substring(name.length, c.length);  
        }}  
        return """";  
    }}
    $(document).ready(function(){{
        var sessionId = shoppingGetCookie('ss-shopping-session-id');
        if (!sessionId) {{
            sessionId = shoppingGuid();
            shoppingSetCookie('ss-shopping-session-id', sessionId);
        }}
        $.ajax({{
            url : ""{apiUrl}"",
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
