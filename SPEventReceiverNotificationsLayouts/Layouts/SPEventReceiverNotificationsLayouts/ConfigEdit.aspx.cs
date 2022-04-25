using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using SPCustomHelpers;
using SPCustomHelpers.SPHelpers;
using SPEventReceiverNotificationsLib;
using SPEventReceiverNotificationsLib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace SPEventReceiverNotificationsLayouts.Layouts.SPEventReceiverNotificationsLayouts
{
    public partial class ConfigEdit : LayoutsPageBase
    {
        private string _confName;
        private SPList _pageSPList;
        private List<ConfigItem> _ListConf;
        private ConfigItem _ListConfByName;
        private ConfigItemGlobal _globalConf;
        private SPFieldCollection _listFields;
        private readonly string _currentUrl = HttpContext.Current.Request.RawUrl;
        private string _parentUrl;
        private Guid _listGuid;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitParams();
            if (IsPostBack)
                return;
            BindDataToPageInfoElements();
            BindDataToTableMainParams();
            BindDataToTableFields();
        }
        private void InitParams()
        {
            _parentUrl = GetParentUrl();
            _listGuid = new Guid(Request.QueryString["List"]);
            _confName = Request.QueryString["ConfName"];
            _pageSPList = SPListHelpers.GetSPList(SPContext.Current.Web.Url, _listGuid);
            _listFields = _pageSPList.Fields;
            _ListConf = PropertyBagConfHelper<List<ConfigItem>>.Get(
                _pageSPList.RootFolder.Properties,
                CommonConstants.LIST_PROPERTY_JSON_CONF
            );
            _ListConfByName = _ListConf.FirstOrDefault(c => c.Title == _confName);
            if (_ListConfByName == null)
                _ListConfByName = new ConfigItem();
            _globalConf = PropertyBagConfHelper<ConfigItemGlobal>.Get(
                _pageSPList.ParentWeb.Site.RootWeb.AllProperties,
                CommonConstants.SITE_PROPERTY_JSON_CONF
            );
        }
        #region BindData to Page
        private void BindDataToPageInfoElements()
        {
            ListConfigsLink.HRef = _parentUrl;
            InfoLabel1.Text = "ListName: \"" + _pageSPList.Title + "\"";
        }
        private void BindDataToTableMainParams()
        {
            ConfigNameTextBox.Text = _ListConfByName.Title;
            EnableCheckBox.Checked = _ListConfByName.Enable;
            NotesTextBox.Text = _ListConfByName.Notes;
            CheckBoxDisableGlobalAccountExclusion.Checked = _ListConfByName.DisableGlobalAccountExclusion;
            TextBoxToMails.Text = String.Join(";", _ListConfByName.ToMails);
            TextBoxCCMails.Text = String.Join(";", _ListConfByName.CCMails);
            TextBoxBCCMails.Text = String.Join(";", _ListConfByName.BCCMails);
            TextBoxExcludedMails.Text = String.Join(";", _ListConfByName.ExcludedManagersMails);
            TextBoxReplyToTemplate.Text = _ListConfByName.ReplyToTemplate;
            CheckBoxSingleRecipientMode.Checked = _ListConfByName.SingleMode;
            TextBoxSubjectTemplate.Text = _ListConfByName.SubjectTemplate;
            TextBoxBodyTemplate.Text = _ListConfByName.BodyTemplate;
            ConfModified.Text = _ListConfByName.ConfModified;
            ConfModifiedBy.Text = _ListConfByName.ConfModifiedBy;
            DropDownListSendType.DataSource = Enum.GetNames(typeof(SenderType));
            DropDownListSendType.DataBind();
            DropDownListSendType.SelectedValue = _ListConfByName.SendType;
            DropDownListEventType.DataSource = Enum.GetNames(typeof(EventReceiverType));
            DropDownListEventType.DataBind();
            DropDownListEventType.SelectedValue = _ListConfByName.EventType;
            CheckBoxListContentTypeFilter.DataSource = _pageSPList.ContentTypes.Cast<SPContentType>().Select(c => c.Name).ToArray();
            CheckBoxListContentTypeFilter.DataBind();
            CheckBoxListContentTypeFilter.Items
                ?.Cast<ListItem>()
                .ToList()
                .ForEach(i => i.Selected = _ListConfByName.ContentTypeFilter != null && _ListConfByName.ContentTypeFilter.Contains(i.Text));
            TextBoxFieldValuesFitler.Text = _ListConfByName.FieldsValuesFilter;
        }
        private void BindDataToTableFields()
        {
            TableFields.DataSource = GetDataForTableFields();
            TableFields.DataBind();
        }
        private DataTable GetDataForTableFields()
        {
            DataTable fieldsDataTable = GetEmptyDataTableForTableFields();
            var dataRows = GetDataRowsForTableFields();
            dataRows.ForEach(r => fieldsDataTable.Rows.Add(r));
            return fieldsDataTable;
        }
        private DataTable GetEmptyDataTableForTableFields()
        {
            DataTable fieldsDataTable = new DataTable();
            fieldsDataTable.Columns.Add(new DataColumn("FieldName", typeof(string)));
            fieldsDataTable.Columns.Add(new DataColumn("FieldIntName", typeof(string)));
            fieldsDataTable.Columns.Add(new DataColumn("IsUserField", typeof(bool)));
            fieldsDataTable.Columns.Add(new DataColumn("ToFields", typeof(bool)));
            fieldsDataTable.Columns.Add(new DataColumn("CCFields", typeof(bool)));
            fieldsDataTable.Columns.Add(new DataColumn("BCCFields", typeof(bool)));
            fieldsDataTable.Columns.Add(new DataColumn("MailFieldsManagers", typeof(bool)));
            return fieldsDataTable;
        }
        private List<Object[]> GetDataRowsForTableFields()
        {
            List<Object[]> dataRowsList = new List<Object[]>();
            foreach (SPField field in _listFields)
            {
                if (field.Hidden)
                    continue;
                //if ()
                List<object> dataRow = new List<object> { };
                string fieldTitle = field.Title;
                string fieldIntName = field.InternalName;
                bool isUserField = field.TypeAsString.Contains("User");
                // Order should be same as in AddColumnsToDataTable
                // data for column FieldName
                dataRow.Add(fieldTitle);
                dataRow.Add(fieldIntName);
                dataRow.Add(isUserField);
                dataRow.Add(_ListConfByName.ToFields.Contains(fieldIntName));
                dataRow.Add(_ListConfByName.CCFields.Contains(fieldIntName));
                dataRow.Add(_ListConfByName.BCCFields.Contains(fieldIntName));
                dataRow.Add(_ListConfByName.MailFieldsManagers.Contains(fieldIntName));
                dataRowsList.Add(dataRow.ToArray());
            };
            return dataRowsList;
        }

        #endregion
        #region SaveData From Page to SPList
        private void GetDataFromTableMainParamsToConfByName()
        {
            _ListConfByName.Title = ConfigNameTextBox.Text;
            _ListConfByName.Enable = EnableCheckBox.Checked;
            _ListConfByName.Notes = NotesTextBox.Text;
            _ListConfByName.DisableGlobalAccountExclusion = CheckBoxDisableGlobalAccountExclusion.Checked;
            _ListConfByName.ConfModified = DateTime.Now.ToString();
            _ListConfByName.ConfModifiedBy = HttpContext.Current.User.Identity.Name;
            _ListConfByName.ToMails = Regex.Split(TextBoxToMails.Text, @"\s?;\s?|\s?,\s?").ToList();
            _ListConfByName.CCMails = Regex.Split(TextBoxCCMails.Text, @"\s?;\s?|\s?,\s?").ToList();
            _ListConfByName.BCCMails = Regex.Split(TextBoxBCCMails.Text, @"\s?;\s?|\s?,\s?").ToList();
            _ListConfByName.ExcludedManagersMails = Regex.Split(TextBoxExcludedMails.Text, @"\s?;\s?|\s?,\s?").ToList();
            _ListConfByName.ReplyToTemplate = TextBoxReplyToTemplate.Text;
            _ListConfByName.SingleMode = CheckBoxSingleRecipientMode.Checked;
            _ListConfByName.SubjectTemplate = TextBoxSubjectTemplate.Text;
            _ListConfByName.BodyTemplate = TextBoxBodyTemplate.Text;
            _ListConfByName.SendType = DropDownListSendType.SelectedValue;
            _ListConfByName.EventType = DropDownListEventType.SelectedValue;
            _ListConfByName.ContentTypeFilter = CheckBoxListContentTypeFilter.Items
                ?.Cast<ListItem>()
                .ToList()
                .Where(i => i.Selected)
                .Select(i => i.Text)
                .ToList();
            _ListConfByName.FieldsValuesFilter = TextBoxFieldValuesFitler.Text;
            //CheckBoxListContentTypeFilter
        }
        private void GetDataFromTableFieldsToConf()
        {
            var fieldsTableRows = TableFields.Rows;
            var headerCount = TableFields.HeaderRow.Cells.Count;
            for (int i = 2; i < headerCount; i++)
            {
                //Dictionary<string, string> valueList = new Dictionary<string, string>();
                List<string> valueList = new List<string>();
                string ctrId = "";
                foreach (GridViewRow row in fieldsTableRows)
                {
                    var fieldIntName = ((Label)(row.Cells[1].FindControl("LabelFieldIntName"))).Text;
                    var cellControls = row.Cells[i].Controls;
                    foreach (var ctr in cellControls)
                    {
                        //if (ctr is TextBox box)
                        //{
                        //    ctrId = box.ID;
                        //    string srcField = box.Text;
                        //    if (!String.IsNullOrEmpty(srcField))
                        //        valueList.Add(fieldName, srcField);
                        //}
                        if (ctr is CheckBox checkbox)
                        {
                            ctrId = checkbox.ID;
                            if (checkbox.Checked)
                                valueList.Add(fieldIntName);
                        }
                    }
                }
                _ListConfByName.GetType().GetProperty(ctrId)?.SetValue(_ListConfByName, valueList);
            }
        }
        protected void ButtonOK_EventHandler(object sender, EventArgs e)
        {
            _ListConf.Remove(_ListConfByName);
            GetDataFromTableMainParamsToConfByName();
            GetDataFromTableFieldsToConf();
            _ListConf.Add(_ListConfByName);
            PropertyBagConfHelper<List<ConfigItem>>.Set(
                _pageSPList.RootFolder.Properties,
                CommonConstants.LIST_PROPERTY_JSON_CONF, _ListConf
            );
            _pageSPList.Update();
            RedirectToParentPage();
        }
        protected void ButtonREMOVE_EventHandler(object sender, EventArgs e)
        {
            _ListConf.Remove(_ListConfByName);
            PropertyBagConfHelper<List<ConfigItem>>.Set(
                _pageSPList.RootFolder.Properties,
                CommonConstants.LIST_PROPERTY_JSON_CONF, _ListConf
            );
            _pageSPList.Update();
            RedirectToParentPage();
        }
        protected void ButtonCANCEL_EventHandler(object sender, EventArgs e)
        {
            RedirectToParentPage();
        }
        #endregion
        #region Helper methods
        private void RedirectToParentPage()
        {
            Response.Redirect(_parentUrl);
        }
        private string GetParentUrl()
        {
            var parentUrlTemp1 = Regex.Replace(_currentUrl, "ConfigEdit", "ListConfigs", RegexOptions.IgnoreCase);
            var parentUrlTemp2 = HttpUtility.ParseQueryString(parentUrlTemp1);
            parentUrlTemp2.Remove("ConfName");
            var parentUrl = HttpUtility.UrlDecode(parentUrlTemp2.ToString());
            return parentUrl;
        }
        #endregion
        #region EventHandlers
        protected void ButtonBodyTemplateCreate_Click(object sender, EventArgs e)
        {
            string fieldTemplate = DropDownListEventType.SelectedValue == "ItemUpdating" ?
                CommonConstants.SETTINGS_BODY_FIELD_TEMPLATE_UPDATINGTYPE :
                CommonConstants.SETTINGS_BODY_FIELD_TEMPLATE_ADDTYPE;
            string globalTemplate = _globalConf.BodyTemplate;
            if (String.IsNullOrEmpty(globalTemplate))
                globalTemplate = "{0}";
            var templateBodyBuilder = new StringBuilder();
            TableFields.Rows
                .Cast<GridViewRow>()
                .ToList()
                .Where(r => ((CheckBox)r.Cells[0].FindControl("CheckBoxForBodyTemplate")).Checked)
                .ToList()
                .ForEach(r =>
                {
                    var fieldName = ((Label)r.Cells[1].FindControl("LabelFieldName")).Text;
                    var fieldIntName = ((Label)r.Cells[1].FindControl("LabelFieldIntName")).Text;
                    templateBodyBuilder.Append(string.Format(fieldTemplate, fieldName, fieldIntName));
                });
            TextBoxBodyTemplate.Text = string.Format(globalTemplate, templateBodyBuilder.ToString());
        }
        #endregion
    }
}