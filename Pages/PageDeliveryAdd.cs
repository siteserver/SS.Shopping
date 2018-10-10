using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Shopping.Core;
using SS.Shopping.Model;

namespace SS.Shopping.Pages
{
    public class PageDeliveryAdd : Page
    {
        public Literal LtlMessage;

        public TextBox TbDeliveryName;
        public DropDownList DdlDeliveryType;
        public TextBox TbStartStandards;
        public TextBox TbStartFees;
        public TextBox TbAddStandards;
        public TextBox TbAddFees;
        public Repeater RptAreas;
        public Button BtnAreaAdd;

        public PlaceHolder PhModalAreas;
        public Button BtnAreas;

        private int _siteId;
        private int _deliveryId;
        private int _areaId;

        public static string GetRedirectUrl(int siteId, int deliveryId)
        {
            return $"{nameof(PageDeliveryAdd)}.aspx?siteId={siteId}&deliveryId={deliveryId}";
        }

        public void Page_Load(object sender, EventArgs e)
        {
            _siteId = Utils.ParseInt(Request.QueryString["siteId"]);
            _deliveryId = Utils.ParseInt(Request.QueryString["deliveryId"]);
            _areaId = Utils.ParseInt(Request.QueryString["areaId"]);

            if (!Main.Instance.Request.AdminPermissions.HasSitePermissions(_siteId, Main.Instance.Id))
            {
                Response.Write("<h1>未授权访问</h1>");
                Response.End();
                return;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["delete"]) &&
                !string.IsNullOrEmpty(Request.QueryString["idCollection"]))
            {
                var array = Request.QueryString["idCollection"].Split(',');
                var list = array.Select(s => Utils.ParseInt(s)).ToList();
                Main.OrderDao.Delete(list);
                LtlMessage.Text = Utils.GetMessageHtml("删除成功！", true);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["addArea"]))
            {
                var areaInfo = new AreaInfo
                {
                    DeliveryId = _deliveryId
                };
                areaInfo.Id = Main.AreaDao.Insert(areaInfo);
                Response.Redirect($@"{GetRedirectUrl(_siteId, _deliveryId)}");
                return;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["removeArea"]))
            {
                Main.AreaDao.Delete(_areaId);
                Response.Redirect($@"{GetRedirectUrl(_siteId, _deliveryId)}");
                return;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["saveCities"]))
            {
                var areaInfo = Main.AreaDao.GetAreaInfo(_areaId);
                areaInfo.Cities = Request.QueryString["selectedCities"];
                Main.AreaDao.Update(areaInfo);
                Response.Redirect($@"{GetRedirectUrl(_siteId, _deliveryId)}");
                return;
            }

            if (IsPostBack) return;

            var deliveryInfo = Main.DeliveryDao.GetDeliveryInfo(_deliveryId);

            TbDeliveryName.Text = deliveryInfo.DeliveryName;
            Utils.SelectListItems(DdlDeliveryType, deliveryInfo.DeliveryType);
            TbStartStandards.Text = deliveryInfo.StartStandards.ToString();
            TbStartFees.Text = deliveryInfo.StartFees.ToString("N2");
            TbAddStandards.Text = deliveryInfo.AddStandards.ToString();
            TbAddFees.Text = deliveryInfo.AddFees.ToString("N2");

            RptAreas.DataSource = Main.AreaDao.GetAreaInfoList(_deliveryId);
            RptAreas.ItemDataBound += RptAreas_ItemDataBound;
            RptAreas.DataBind();

            BtnAreaAdd.Attributes.Add("onclick", $@"location.href = '{GetRedirectUrl(_siteId, _deliveryId)}&addArea={true}';return false;");

            if (!string.IsNullOrEmpty(Request.QueryString["areas"]) && !string.IsNullOrEmpty(Request.QueryString["areaId"]))
            {
                PhModalAreas.Visible = true;
                BtnAreas.Attributes.Add("onclick", $"location.href='{GetRedirectUrl(_siteId, _deliveryId)}&saveCities={true}&areaId={_areaId}&selectedCities=' + selectedCities.join(',');return false;");
            }
        }

        public string GetSelectedCities()
        {
            var areaInfo = Main.AreaDao.GetAreaInfo(_areaId);
            if (string.IsNullOrEmpty(areaInfo.Cities))
            {
                return "[]";
            }
            var builder = new StringBuilder();
            foreach (var city in areaInfo.Cities.Split(','))
            {
                builder.Append($"'{city}',");
            }
            builder.Length--;
            return $"[{builder}]";
        }

        private void RptAreas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var areaInfo = (AreaInfo)e.Item.DataItem;

            var hfAreaId = (HiddenField)e.Item.FindControl("hfAreaId");
            var ltlCities = (Literal)e.Item.FindControl("ltlCities");
            var tbStartStandards = (TextBox)e.Item.FindControl("tbStartStandards");
            var tbStartFees = (TextBox)e.Item.FindControl("tbStartFees");
            var tbAddStandards = (TextBox)e.Item.FindControl("tbAddStandards");
            var tbAddFees = (TextBox)e.Item.FindControl("tbAddFees");

            hfAreaId.Value = areaInfo.Id.ToString();
            ltlCities.Text = areaInfo.Cities;
            ltlCities.Text = $@"
指定地区
<a href=""{GetRedirectUrl(_siteId, _deliveryId)}&areas={true}&areaId={areaInfo.Id}"">编辑</a>
<a href=""{GetRedirectUrl(_siteId, _deliveryId)}&removeArea={true}&areaId={areaInfo.Id}"">删除</a>
<br />
{(string.IsNullOrEmpty(areaInfo.Cities) ? "未选择任何区域" : areaInfo.Cities)}";

            tbStartStandards.Text = areaInfo.StartStandards.ToString();
            tbStartFees.Text = areaInfo.StartFees.ToString("N2");
            tbAddStandards.Text = areaInfo.AddStandards.ToString();
            tbAddFees.Text = areaInfo.AddFees.ToString("N2");
        }

        public void BtnSubmit_OnClick(object sender, EventArgs e)
        {
            if (SaveDelivery())
            {
                Response.Redirect(PageDelivery.GetRedirectUrl(_siteId));
            }
            else
            {
                LtlMessage.Text = Utils.GetMessageHtml("请正确填写运费设置", false);
            }
        }

        private bool SaveDelivery()
        {
            var deliveryInfo = Main.DeliveryDao.GetDeliveryInfo(_deliveryId);

            deliveryInfo.DeliveryName = TbDeliveryName.Text;
            deliveryInfo.DeliveryType = DdlDeliveryType.SelectedValue;
            deliveryInfo.StartStandards = Utils.ParseInt(TbStartStandards.Text);
            deliveryInfo.StartFees = Utils.ParseDecimal(TbStartFees.Text);
            deliveryInfo.AddStandards = Utils.ParseInt(TbAddStandards.Text);
            deliveryInfo.AddFees = Utils.ParseDecimal(TbAddFees.Text);

            foreach (RepeaterItem repeaterItem in RptAreas.Items)
            {
                if (repeaterItem.ItemType != ListItemType.Item && repeaterItem.ItemType != ListItemType.AlternatingItem)
                    continue;

                var hfAreaId = (HiddenField) repeaterItem.FindControl("hfAreaId");
                var tbStartStandards = (TextBox) repeaterItem.FindControl("tbStartStandards");
                var tbStartFees = (TextBox) repeaterItem.FindControl("tbStartFees");
                var tbAddStandards = (TextBox) repeaterItem.FindControl("tbAddStandards");
                var tbAddFees = (TextBox) repeaterItem.FindControl("tbAddFees");

                var areaId = Utils.ParseInt(hfAreaId.Value);
                var areaInfo = Main.AreaDao.GetAreaInfo(areaId);

                areaInfo.StartStandards = Utils.ParseInt(tbStartStandards.Text);
                areaInfo.StartFees = Utils.ParseDecimal(tbStartFees.Text);
                areaInfo.AddStandards = Utils.ParseInt(tbAddStandards.Text);
                areaInfo.AddFees = Utils.ParseDecimal(tbAddFees.Text);

                if (string.IsNullOrEmpty(areaInfo.Cities)) return false;

                Main.AreaDao.Update(areaInfo);
            }

            Main.DeliveryDao.Update(deliveryInfo);

            return true;
        }

        public void BtnCancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect(PageDelivery.GetRedirectUrl(_siteId));
        }
    }
}
