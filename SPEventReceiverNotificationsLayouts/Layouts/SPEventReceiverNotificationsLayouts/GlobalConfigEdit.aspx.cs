using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using SPCustomHelpers;
using SPEventReceiverNotificationsLib;
using System;

namespace SPEventReceiverNotificationsLayouts.Layouts.SPEventReceiverNotificationsLayouts
{
    public partial class GlobalConfigEdit : LayoutsPageBase
    {
        private SPSite _currentSite;
        private ConfigItemGlobal _globalConf;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitParams();
            if (IsPostBack)
                return;
            BindDataToSimpleFields();
        }

        private void InitParams()
        {
            _currentSite = SPContext.Current.Site;
            _globalConf = PropertyBagConfHelper<ConfigItemGlobal>.Get(
                _currentSite.RootWeb.AllProperties,
                CommonConstants.SITE_PROPERTY_JSON_CONF
            );
        }
        #region BindData to Page
        private void BindDataToSimpleFields()
        {
        }
        #endregion
        #region SaveData From Page to SPList
        private void GetDataFromSimpleFieldsToConf()
        {
        }
        #endregion
        #region Base buttons
        protected void ButtonOK_EventHandler(object sender, EventArgs e)
        {
            GetDataFromSimpleFieldsToConf();
            PropertyBagConfHelper<ConfigItemGlobal>.Set(
                _currentSite.RootWeb.AllProperties,
                CommonConstants.SITE_PROPERTY_JSON_CONF, _globalConf
            );
            _currentSite.RootWeb.Update();
            RedirectToPreviousPageBySource();
        }
        protected void ButtonCANCEL_EventHandler(object sender, EventArgs e)
        {
            RedirectToPreviousPageBySource();
        }
        #endregion
        private void RedirectToPreviousPageBySource()
        {
            string sourceUrl = Context.Request.QueryString["Source"];
            string previousUrl = String.IsNullOrEmpty(sourceUrl) ? SPContext.Current.Web.Url : sourceUrl;
            Response.Redirect(previousUrl);
        }

    }
}
