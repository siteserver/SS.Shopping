using System;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Shopping.Core.Model;

namespace SS.Shopping.Core.Pages
{
	public class PageOrder : Page
	{
	    public Literal LtlPageTitle;
	    public Literal LtlMessage;

        public DropDownList DdlSearchState;
        public TextBox TbSearchKeyword;

        public Repeater RptContents;

        public Button BtnDelete;
        public Button BtnStateAll;

        public PlaceHolder PhModalView;
        public Literal LtlModalViewMessage;
        public Literal LtlGuid;
        public Literal LtlRealName;
        public Literal LtlMobile;
        public Literal LtlTel;
        public Literal LtlLocation;
        public Literal LtlAddress;
        public Literal LtlZipCode;
        public Literal LtlChannel;
        public Literal LtlAmount;
        public Literal LtlAddDate;
        public Literal LtlState;
	    public Repeater RptCarts;
        public Button BtnState;

	    public PlaceHolder PhModalState;
	    public DropDownList DdlState;

        private int _siteId;
	    private bool _isPayed;
	    private string _state;
	    private string _keyword;

        public static string GetRedirectUrl(int siteId, bool isPayed, string state, string keyword)
        {
            return $"{nameof(PageOrder)}.aspx?siteId={siteId}&isPayed={isPayed}&state={state}&keyword={keyword}";
        }

	    public void Page_Load(object sender, EventArgs e)
	    {
	        var request = SiteServer.Plugin.Context.AuthenticatedRequest;
	        _siteId = request.GetQueryInt("siteId");
            _isPayed = request.GetQueryBool("isPayed");
            _state = request.GetQueryString("state");
            _keyword = request.GetQueryString("keyword");

            if (!request.AdminPermissions.HasSitePermissions(_siteId, Main.PluginId))
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
	            Main.OrderRepository.Delete(list);
	            LtlMessage.Text = Utils.GetMessageHtml("删除成功！", true);
	        }

	        //SpContents.ControlToPaginate = RptContents;
	        //SpContents.ItemsPerPage = 30;
         //   SpContents.SelectCommand = OrderRepository.GetSelectStringBySearch(_siteId, _isPayed, _state, _keyword);
         //   SpContents.SortField = nameof(OrderInfo.Id);
	        //SpContents.SortMode = "DESC";
	        RptContents.ItemDataBound += RptContents_ItemDataBound;

	        if (IsPostBack) return;

	        LtlPageTitle.Text = Utils.GetStateText(_isPayed, _state) + "订单管理";

            DdlSearchState.Items.Add(new ListItem(Utils.GetStateText(false, string.Empty), $"{false}_"));
            DdlSearchState.Items.Add(new ListItem(Utils.GetStateText(true, string.Empty), $"{true}_"));
            DdlSearchState.Items.Add(new ListItem(Utils.GetStateText(true, nameof(OrderState.Done)), $"{true}_{nameof(OrderState.Done)}"));
            Utils.SelectListItems(DdlSearchState, $"{_isPayed}_{_state}");
	        TbSearchKeyword.Text = _keyword;

            //SpContents.DataBind();

            BtnDelete.Attributes.Add("onclick", Utils.ReplaceNewline($@"
var ids = [];
$(""input[name='idCollection']:checked"").each(function () {{
    ids.push($(this).val());}}
);
if (ids.length > 0){{
    {Utils.SwalWarning("删除订单", "此操作将删除所选订单，确定吗？", "取 消", "删 除",
                $"location.href='{GetRedirectUrl(_siteId, _isPayed, _state, _keyword)}&delete={true}&idCollection=' + ids.join(',')")}
}} else {{
    {Utils.SwalError("请选择需要删除的订单！", string.Empty)}
}}
;return false;", string.Empty));

            BtnStateAll.Attributes.Add("onclick", Utils.ReplaceNewline($@"
var ids = [];
$(""input[name='idCollection']:checked"").each(function () {{
    ids.push($(this).val());}}
);
if (ids.length > 0){{
    location.href = '{GetRedirectUrl(_siteId, _isPayed, _state, _keyword)}&state={true}&idCollection=' + ids.join(',')
}} else {{
    {Utils.SwalError("请选择需要更改状态的订单！", string.Empty)}
}}
;return false;", string.Empty));

	        if (!string.IsNullOrEmpty(Request.QueryString["view"]))
	        {
	            PhModalView.Visible = true;

	            var id = Utils.ParseInt(Request.QueryString["id"]);

	            var orderInfo = Main.OrderRepository.GetOrderInfo(id);
	            if (!string.IsNullOrEmpty(orderInfo.Message))
	            {
	                LtlModalViewMessage.Text = Utils.GetMessageHtml(orderInfo.Message);
	            }

	            LtlGuid.Text = orderInfo.Guid;
	            LtlRealName.Text = orderInfo.RealName;
	            LtlMobile.Text = orderInfo.Mobile;
	            LtlTel.Text = orderInfo.Tel;
	            LtlLocation.Text = orderInfo.Location;
	            LtlAddress.Text = orderInfo.Address;
	            LtlZipCode.Text = orderInfo.ZipCode;
	            LtlChannel.Text = Utils.GetChannelText(orderInfo.Channel);
	            LtlAmount.Text =
	                $@"商品金额：¥{orderInfo.TotalFee:N2}元 运费：¥{orderInfo.ExpressCost:N2}元 合计：¥{orderInfo.TotalFee +
	                                                                                       orderInfo.ExpressCost:N2}元";
                if (orderInfo.AddDate != null)
                    LtlAddDate.Text = orderInfo.AddDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                LtlState.Text = Utils.GetStateText(orderInfo.IsPayed, orderInfo.State);

	            RptCarts.DataSource = Main.CartRepository.GetCartInfoList(orderInfo.Id);
	            RptCarts.ItemDataBound += RptCarts_ItemDataBound;
	            RptCarts.DataBind();

	            BtnState.Attributes.Add("onclick", Utils.ReplaceNewline($@"
location.href = '{GetRedirectUrl(_siteId, _isPayed, _state, _keyword)}&setState={true}&idCollection={id}';
return false;", string.Empty));
	        }
	        else if (!string.IsNullOrEmpty(Request.QueryString["setState"]) &&
	                 !string.IsNullOrEmpty(Request.QueryString["idCollection"]))
	        {
	            PhModalState.Visible = true;
	            DdlState.Items.Add(new ListItem(Utils.GetStateText(false, string.Empty), $"{false}_"));
	            DdlState.Items.Add(new ListItem(Utils.GetStateText(true, string.Empty), $"{true}_"));
	            DdlState.Items.Add(new ListItem(Utils.GetStateText(true, nameof(OrderState.Done)),
	                $"{true}_{nameof(OrderState.Done)}"));
	        }
	    }

        private static void RptCarts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var cartInfo = (CartInfo) e.Item.DataItem;

            var ltlTitle = (Literal)e.Item.FindControl("ltlTitle");
            var ltlImage = (Literal)e.Item.FindControl("ltlImage");
            var ltlCount = (Literal)e.Item.FindControl("ltlCount");

            ltlTitle.Text = $@"<a href=""{cartInfo.LinkUrl}"" target=""_blank"">{cartInfo.ProductName}</a>";
            ltlImage.Text = $@"<img src=""{cartInfo.ImageUrl}"" style=""max-height: 120px;"" />";
            ltlCount.Text = cartInfo.Count.ToString();
        }

        private void RptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var id = Utils.EvalInt(e.Item.DataItem, nameof(OrderInfo.Id));
            var guid = Utils.EvalString(e.Item.DataItem, nameof(OrderInfo.Guid));
            var realName = Utils.EvalString(e.Item.DataItem, nameof(OrderInfo.RealName));
            var mobile = Utils.EvalString(e.Item.DataItem, nameof(OrderInfo.Mobile));
            var location = Utils.EvalString(e.Item.DataItem, nameof(OrderInfo.Location));
            var totalFee = Utils.EvalDecimal(e.Item.DataItem, nameof(OrderInfo.TotalFee));
            var expressCost = Utils.EvalDecimal(e.Item.DataItem, nameof(OrderInfo.ExpressCost));
            var totalCount = Utils.EvalInt(e.Item.DataItem, nameof(OrderInfo.TotalCount));
            var channel = Utils.EvalString(e.Item.DataItem, nameof(OrderInfo.Channel));
            var isPayed = Utils.EvalBool(e.Item.DataItem, nameof(OrderInfo.IsPayed));
            var state = Utils.EvalString(e.Item.DataItem, nameof(OrderInfo.State));
            var addDate = Utils.EvalDateTime(e.Item.DataItem, nameof(OrderInfo.AddDate));

            var ltlGuid = (Literal)e.Item.FindControl("ltlGuid");
            var ltlRealName = (Literal)e.Item.FindControl("ltlRealName");
            var ltlMobile = (Literal) e.Item.FindControl("ltlMobile");
            var ltlLocation = (Literal)e.Item.FindControl("ltlLocation");
            var ltlChannel = (Literal)e.Item.FindControl("ltlChannel");
            var ltlAmount = (Literal)e.Item.FindControl("ltlAmount");
            var ltlTotalCount = (Literal)e.Item.FindControl("ltlTotalCount");
            var ltlState = (Literal)e.Item.FindControl("ltlState");
            var ltlDateTime = (Literal)e.Item.FindControl("ltlDateTime");

            ltlGuid.Text = $@"<a href=""{GetRedirectUrl(_siteId, _isPayed, _state, _keyword)}&view={true}&id={id}"">{guid}</a>";
            ltlRealName.Text = realName;
            ltlMobile.Text = mobile;
            ltlLocation.Text = location;
            ltlAmount.Text = $@"¥{totalFee + expressCost:N2}元";
            ltlChannel.Text = Utils.GetChannelText(channel);
            ltlTotalCount.Text = totalCount.ToString(CultureInfo.InvariantCulture);
            ltlState.Text = Utils.GetStateText(isPayed, state);
            ltlDateTime.Text = addDate.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public void BtnState_OnClick(object sender, EventArgs e)
        {
            var list = Request.QueryString["idCollection"].Split(',').Select(s => Utils.ParseInt(s)).ToList();
            var value = DdlState.SelectedValue;
            var isPayed = Convert.ToBoolean(value.Split('_')[0]);
            var state = value.Split('_')[1];

            foreach (var orderId in list)
            {
                Main.OrderRepository.UpdateIsPayedAndState(orderId, isPayed, state);
            }

            Response.Redirect(GetRedirectUrl(_siteId, _isPayed, _state, _keyword));
        }

        public void BtnSearch_OnClick(object sender, EventArgs e)
        {
            var value = DdlSearchState.SelectedValue;
            var isPayed = false;
            var state = string.Empty;
            if (!string.IsNullOrEmpty(value) && value.Contains("_"))
            {
                isPayed = Utils.ToBool(value.Split('_')[0]);
                state = value.Split('_')[1];
            }

            Response.Redirect(GetRedirectUrl(_siteId, isPayed, state, TbSearchKeyword.Text));
        }
    }
}
