using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using SPCustomHelpers;
using SPEventReceiverNotificationsLib;
using System;
using System.Linq;
using System.Text.RegularExpressions;

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
            BindDataToParamsTable();
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
        private void BindDataToParamsTable()
        {
            TextBoxToMails.Text = String.Join(";", _globalConf.ToMails);
            TextBoxCCMails.Text = String.Join(";", _globalConf.CCMails);
            TextBoxBCCMails.Text = String.Join(";", _globalConf.BCCMails);
            TextBoxAccountsExclusionsRegexp.Text = _globalConf.AccountsExclusionsRegexp;
            TextBoxBodyTemplate.Text = _globalConf.BodyTemplate;
        }
        #endregion
        #region SaveData From Page to SPList
        private void GetDataFromParamsTableToConf()
        {
            _globalConf.ToMails = Regex.Split(TextBoxToMails.Text, @"\s?;\s?|\s?,\s?").ToList();
            _globalConf.CCMails = Regex.Split(TextBoxCCMails.Text, @"\s?;\s?|\s?,\s?").ToList();
            _globalConf.BCCMails = Regex.Split(TextBoxBCCMails.Text, @"\s?;\s?|\s?,\s?").ToList();
            _globalConf.AccountsExclusionsRegexp = TextBoxAccountsExclusionsRegexp.Text;
            _globalConf.BodyTemplate = TextBoxBodyTemplate.Text;
        }
        #endregion
        #region Base buttons
        protected void ButtonOK_EventHandler(object sender, EventArgs e)
        {
            GetDataFromParamsTableToConf();
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
