using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SiteServer.Plugin;
using SS.Shopping.Model;

namespace SS.Shopping.Core
{
    public static class Utils
    {
        public static string GetMessageHtml(string message)
        {
            return $@"<div class=""alert alert-info"" role=""alert"">{message}</div>";
        }

        public static string GetMessageHtml(string message, bool isSuccess)
        {
            return isSuccess
                ? $@"<div class=""alert alert-success"" role=""alert"">{message}</div>"
                : $@"<div class=""alert alert-danger"" role=""alert"">{message}</div>";
        }

        public static void SelectListItems(ListControl listControl, params string[] values)
        {
            if (listControl != null)
            {
                foreach (ListItem item in listControl.Items)
                {
                    item.Selected = false;
                }
                foreach (ListItem item in listControl.Items)
                {
                    foreach (var value in values)
                    {
                        if (string.Equals(item.Value, value))
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        public static bool ToBool(string boolStr)
        {
            bool result;
            return bool.TryParse(boolStr, out result) && result;
        }

        public static string GetUrlWithoutQueryString(string rawUrl)
        {
            string urlWithoutQueryString;
            if (rawUrl != null && rawUrl.IndexOf("?", StringComparison.Ordinal) != -1)
            {
                var queryString = rawUrl.Substring(rawUrl.IndexOf("?", StringComparison.Ordinal));
                urlWithoutQueryString = rawUrl.Replace(queryString, "");
            }
            else
            {
                urlWithoutQueryString = rawUrl;
            }
            return urlWithoutQueryString;
        }

        public static string AddQueryString(string url, NameValueCollection queryString)
        {
            if (queryString == null || url == null || queryString.Count == 0)
                return url;

            var builder = new StringBuilder();
            foreach (string key in queryString.Keys)
            {
                builder.Append($"&{key}={HttpUtility.UrlEncode(queryString[key])}");
            }
            if (url.IndexOf("?", StringComparison.Ordinal) == -1)
            {
                if (builder.Length > 0) builder.Remove(0, 1);
                return string.Concat(url, "?", builder.ToString());
            }
            if (url.EndsWith("?"))
            {
                if (builder.Length > 0) builder.Remove(0, 1);
            }
            return string.Concat(url, builder.ToString());
        }

        public static bool EqualsIgnoreCase(string a, string b)
        {
            if (a == b) return true;
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return false;
            return string.Equals(a.Trim().ToLower(), b.Trim().ToLower());
        }

        public static string GetTopSqlString(DatabaseType databaseType, string tableName, string columns, string whereAndOrder, int topN)
        {
            if (topN > 0)
            {
                return databaseType == DatabaseType.MySql ? $"SELECT {columns} FROM {tableName} {whereAndOrder} LIMIT {topN}" : $"SELECT TOP {topN} {columns} FROM {tableName} {whereAndOrder}";
            }
            return $"SELECT {columns} FROM {tableName} {whereAndOrder}";
        }

        public static object Eval(object dataItem, string name)
        {
            object o = null;
            try
            {
                o = DataBinder.Eval(dataItem, name);
            }
            catch
            {
                // ignored
            }
            if (o == DBNull.Value)
            {
                o = null;
            }
            return o;
        }

        public static int EvalInt(object dataItem, string name)
        {
            var o = Eval(dataItem, name);
            return o == null ? 0 : ParseInt(o.ToString());
        }

        public static decimal EvalDecimal(object dataItem, string name)
        {
            var o = Eval(dataItem, name);
            return o == null ? 0 : Convert.ToDecimal(o);
        }

        public static string EvalString(object dataItem, string name)
        {
            var o = Eval(dataItem, name);
            return o?.ToString() ?? string.Empty;
        }

        public static DateTime EvalDateTime(object dataItem, string name)
        {
            var o = Eval(dataItem, name);
            if (o == null)
            {
                return DateTime.MinValue;
            }
            return (DateTime)o;
        }

        public static bool EvalBool(object dataItem, string name)
        {
            var o = Eval(dataItem, name);
            return o != null && Convert.ToBoolean(o.ToString());
        }

        public static int ParseInt(object o)
        {
            return o == null ? 0 : ParseInt(o.ToString(), 0);
        }

        public static int ParseInt(string s)
        {
            return ParseInt(s, 0);
        }

        public static int ParseInt(string s, int defaultValue)
        {
            int i;
            return int.TryParse(s, out i) ? i : defaultValue;
        }

        public static decimal ParseDecimal(string s)
        {
            return ParseDecimal(s, 0);
        }

        public static decimal ParseDecimal(string s, decimal defaultValue)
        {
            decimal i;
            return decimal.TryParse(s, out i) ? i : defaultValue;
        }

        public static decimal GetDeliveryFee(List<CartInfo> cartInfoList, AddressInfo addressInfo, DeliveryInfo deliveryInfo)
        {
            if (cartInfoList == null || deliveryInfo == null) return 0;

            var startStandards = deliveryInfo.StartStandards;
            var startFees = deliveryInfo.StartFees;
            var addStandards = deliveryInfo.AddStandards;
            var addFees = deliveryInfo.AddFees;

            if (addressInfo != null)
            {
                var areaInfoList = Main.AreaDao.GetAreaInfoList(deliveryInfo.Id);
                foreach (var areaInfo in areaInfoList)
                {
                    var cities = areaInfo.Cities.Split(',').ToList();
                    foreach (var city in cities)
                    {
                        if (!addressInfo.Location.Contains(city)) continue;

                        startStandards = areaInfo.StartStandards;
                        startFees = areaInfo.StartFees;
                        addStandards = areaInfo.AddStandards;
                        addFees = areaInfo.AddFees;
                    }
                }
            }

            var count = 0;
            foreach (var cartInfo in cartInfoList)
            {
                if (cartInfo.IsDelivery)
                {
                    count += cartInfo.Count;
                }
            }

            var deliveryFee = startFees;

            if (count > startStandards)
            {
                var add = count - startStandards;
                if (addStandards > 0 && addFees > 0)
                {
                    deliveryFee += Math.Floor(Convert.ToDecimal(add / addStandards)) * addFees;
                }
            }

            return deliveryFee;
        }

        public static string GetStateText(bool isPaied, string state)
        {
            if (!isPaied) return "未支付";

            if (EqualsIgnoreCase(state, nameof(OrderState.Done)))
            {
                return "已完成";
            }

            return "已支付";
        }

        public static string GetChannelText(string channel)
        {
            if (EqualsIgnoreCase(channel, nameof(PaymentChannel.Alipay)))
            {
                return "支付宝";
            }
            if (EqualsIgnoreCase(channel, nameof(PaymentChannel.Weixin)))
            {
                return "微信支付";
            }
            if (EqualsIgnoreCase(channel, nameof(PaymentChannel.Jdpay)))
            {
                return "京东支付";
            }

            return string.Empty;
        }

        public static string ReplaceNewline(string inputString, string replacement)
        {
            if (string.IsNullOrEmpty(inputString)) return string.Empty;
            var retVal = new StringBuilder();
            inputString = inputString.Trim();
            foreach (var t in inputString)
            {
                switch (t)
                {
                    case '\n':
                        retVal.Append(replacement);
                        break;
                    case '\r':
                        break;
                    default:
                        retVal.Append(t);
                        break;
                }
            }
            return retVal.ToString();
        }

        public static string SwalError(string title, string text)
        {
            var script = $@"swal({{
  title: '{title}',
  text: '{ReplaceNewline(text, string.Empty)}',
  icon: 'error',
  button: '关 闭',
}});";

            return script;
        }

        public static string SwalWarning(string title, string text, string btnCancel, string btnSubmit, string scripts)
        {
            var script = $@"swal({{
  title: '{title}',
  text: '{ReplaceNewline(text, string.Empty)}',
  icon: 'warning',
  buttons: {{
    cancel: '{btnCancel}',
    catch: '{btnSubmit}'
  }}
}})
.then(function(willDelete){{
  if (willDelete) {{
    {scripts}
  }}
}});";
            return script;
        }
    }
}
