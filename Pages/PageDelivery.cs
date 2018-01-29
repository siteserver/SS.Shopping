using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Shopping.Core;
using SS.Shopping.Model;

namespace SS.Shopping.Pages
{
    public class PageDelivery : Page
    {
        public Literal LtlMessage;
        public Repeater RptContents;

        private int _siteId;

        public static string GetRedirectUrl(int siteId)
        {
            return Main.FilesApi.GetPluginUrl($"{nameof(PageDelivery)}.aspx?siteId={siteId}");
        }

        public void Page_Load(object sender, EventArgs e)
        {
            _siteId = Utils.ParseInt(Request.QueryString["siteId"]);

            if (!Main.AdminApi.IsSiteAuthorized(_siteId))
            {
                Response.Write("<h1>未授权访问</h1>");
                Response.End();
                return;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["delete"]) &&
                !string.IsNullOrEmpty(Request.QueryString["deliveryId"]))
            {
                var deliveryId = Utils.ParseInt(Request.QueryString["deliveryId"]);
                Main.DeliveryDao.Delete(deliveryId);
                LtlMessage.Text = Utils.GetMessageHtml("删除成功！", true);
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["up"]) &&
                !string.IsNullOrEmpty(Request.QueryString["deliveryId"]))
            {
                var deliveryId = Utils.ParseInt(Request.QueryString["deliveryId"]);
                Main.DeliveryDao.UpdateTaxisToUp(_siteId, deliveryId);
                LtlMessage.Text = Utils.GetMessageHtml("排序生成！", true);
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["down"]) &&
               !string.IsNullOrEmpty(Request.QueryString["deliveryId"]))
            {
                var deliveryId = Utils.ParseInt(Request.QueryString["deliveryId"]);
                Main.DeliveryDao.UpdateTaxisToDown(_siteId, deliveryId);
                LtlMessage.Text = Utils.GetMessageHtml("排序生成！", true);
            }
            else
            {
                Main.DeliveryDao.DeleteDeliveryNameIsEmpty();
            }

            if (IsPostBack) return;

            RptContents.DataSource = Main.DeliveryDao.GetDeliveryInfoList(_siteId);
            RptContents.ItemDataBound += RptContents_ItemDataBound;
            RptContents.DataBind();
        }

        public void BtnAdd_OnClick(object sender, EventArgs e)
        {
            var deliveryInfo = new DeliveryInfo
            {
                PublishmentSystemId = _siteId
            };
            deliveryInfo.Id = Main.DeliveryDao.Insert(deliveryInfo);
            Response.Redirect(PageDeliveryAdd.GetRedirectUrl(_siteId, deliveryInfo.Id));
        }

        private static void RptAreas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var areaInfo = (AreaInfo)e.Item.DataItem;

            var ltlCities = (Literal)e.Item.FindControl("ltlCities");
            var ltlStartStandards = (Literal)e.Item.FindControl("ltlStartStandards");
            var ltlStartFees = (Literal)e.Item.FindControl("ltlStartFees");
            var ltlAddStandards = (Literal)e.Item.FindControl("ltlAddStandards");
            var ltlAddFees = (Literal)e.Item.FindControl("ltlAddFees");

            ltlCities.Text = areaInfo.Cities;

            ltlStartStandards.Text = areaInfo.StartStandards.ToString();
            ltlStartFees.Text = areaInfo.StartFees.ToString("N2");
            ltlAddStandards.Text = areaInfo.AddStandards.ToString();
            ltlAddFees.Text = areaInfo.AddFees.ToString("N2");
        }

        private void RptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var deliveryInfo = (DeliveryInfo)e.Item.DataItem;

            var ltlDeliveryName = (Literal)e.Item.FindControl("ltlDeliveryName");
            var ltlDeliveryType = (Literal)e.Item.FindControl("ltlDeliveryType");
            var ltlStartStandards = (Literal)e.Item.FindControl("ltlStartStandards");
            var ltlStartFees = (Literal)e.Item.FindControl("ltlStartFees");
            var ltlAddStandards = (Literal)e.Item.FindControl("ltlAddStandards");
            var ltlAddFees = (Literal)e.Item.FindControl("ltlAddFees");
            var rptAreas = (Repeater)e.Item.FindControl("rptAreas");
            var ltlActions = (Literal)e.Item.FindControl("ltlActions");

            ltlDeliveryName.Text = deliveryInfo.DeliveryName;
            ltlDeliveryType.Text = deliveryInfo.DeliveryType;

            ltlStartStandards.Text = deliveryInfo.StartStandards.ToString();
            ltlStartFees.Text = deliveryInfo.StartFees.ToString("N2");
            ltlAddStandards.Text = deliveryInfo.AddStandards.ToString();
            ltlAddFees.Text = deliveryInfo.AddFees.ToString("N2");

            rptAreas.DataSource = Main.AreaDao.GetAreaInfoList(deliveryInfo.Id);
            rptAreas.ItemDataBound += RptAreas_ItemDataBound;
            rptAreas.DataBind();

            ltlActions.Text =
                $@"<a class=""m-r-10"" href=""{PageDeliveryAdd.GetRedirectUrl(_siteId, deliveryInfo.Id)}"">编 辑</a>
                    <a class=""m-r-10"" href=""{GetRedirectUrl(_siteId)}&up={true}&deliveryId={deliveryInfo.Id}"">上 升</a>
                    <a class=""m-r-10"" href=""{GetRedirectUrl(_siteId)}&down={true}&deliveryId={deliveryInfo.Id}"">下 降</a>
                    <a class=""m-r-10"" href=""javascript:;"" onclick=""{Utils.SwalWarning("删除运费",
                    $"此操作将删除运费“{deliveryInfo.DeliveryName}”，确定吗？", "取 消", "删 除",
                    $"location.href='{GetRedirectUrl(_siteId)}&delete={true}&deliveryId={deliveryInfo.Id}'")};return false;"">删 除</a>";
        }
    }
}
