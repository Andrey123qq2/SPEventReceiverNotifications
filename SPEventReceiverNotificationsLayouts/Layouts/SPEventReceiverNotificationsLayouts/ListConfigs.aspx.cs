using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using SPCustomHelpers;
using SPCustomHelpers.SPHelpers;
using SPEventReceiverNotificationsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SPEventReceiverNotificationsLayouts.Layouts.SPEventReceiverNotificationsLayouts
{
    public partial class ListConfigs : LayoutsPageBase
    {
        private SPList _pageSPList;
        private List<ConfigItem> _ListConf;
        private readonly string _currentUrl = HttpContext.Current.Request.RawUrl;
        private string _parentUrl;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitParams();
            if (IsPostBack)
                return;
            BindDataToPageInfoElements();
            BindDataToParamsTable();
        }
        private void InitParams()
        {
            Guid listGuid = new Guid(Request.QueryString["List"]);
            _pageSPList = SPListHelpers.GetSPList(SPContext.Current.Web.Url, listGuid);
            _ListConf = PropertyBagConfHelper<List<ConfigItem>>.Get(
                _pageSPList.RootFolder.Properties,
                CommonConstants.LIST_PROPERTY_JSON_CONF
            );
            _parentUrl = Regex.Replace(_currentUrl, "SPEventReceiverNotificationsLayouts/ListConfigs", "listedit", RegexOptions.IgnoreCase);
        }
        private void BindDataToPageInfoElements()
        {
            SettingsLink.HRef = _parentUrl;
            InfoLabel1.Text = "List name: \"" + _pageSPList.Title + "\"";
            SiteConfigsLink.NavigateUrl = "/_layouts/15/SPListsNotifications/CommonConfigEdit.aspx?Source=" + _currentUrl;
        }
        private void BindDataToParamsTable()
        {
            ConfigsTable.DataSource = GetArrayForGridView();
            ConfigsTable.DataBind();
        }
        public Array GetArrayForGridView()
        {
            Array arrayForGridView = _ListConf.Select(conf => new
            {
                ConfName = conf.Title,
                ConfUrl = _currentUrl.Replace("ListConfigs", "ConfigEdit") + "&ConfName=" + conf.Title,
                ConfModifiedDate = conf.ConfModified,
                ConfEnabled = conf.Enable,
            }
            ).ToArray();
            return arrayForGridView;
        }
        protected void ButtonAdd_EventHandler(object sender, EventArgs e)
        {
            Response.Redirect(_currentUrl.Replace("ListConfigs", "ConfigEdit"));
        }
    }
}