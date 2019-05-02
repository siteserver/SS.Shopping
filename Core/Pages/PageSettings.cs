using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Shopping.Core.Model;

namespace SS.Shopping.Core.Pages
{
    public class PageSettings : Page
    {
        public Literal LtlMessage;
        public DropDownList DdlIsForceLogin;

        private int _siteId;
        private ConfigInfo _configInfo;

        public void Page_Load(object sender, EventArgs e)
        {
            var request = SiteServer.Plugin.Context.AuthenticatedRequest;
            _siteId = request.GetQueryInt("siteId");

            if (!request.AdminPermissions.HasSitePermissions(_siteId, Main.PluginId))
            {
                HttpContext.Current.Response.Write("<h1>未授权访问</h1>");
                HttpContext.Current.Response.End();
                return;
            }

            _configInfo = SiteServer.Plugin.Context.ConfigApi.GetConfig<ConfigInfo>(Main.PluginId, _siteId);
            if (_configInfo == null)
            {
                _configInfo = new ConfigInfo();
            }

            if (IsPostBack) return;

            Utils.SelectListItems(DdlIsForceLogin, _configInfo.IsForceLogin.ToString());
        }

        public void Submit_OnClick(object sender, EventArgs e)
        {
            if (!Page.IsPostBack || !Page.IsValid) return;

            _configInfo.IsForceLogin = Convert.ToBoolean(DdlIsForceLogin.SelectedValue);

            SiteServer.Plugin.Context.ConfigApi.SetConfig(Main.PluginId, _siteId, _configInfo);
            LtlMessage.Text = Utils.GetMessageHtml("购物设置修改成功！", true);
        }
    }
}