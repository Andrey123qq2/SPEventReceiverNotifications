using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using SPCustomHelpers;
using SPCustomHelpers.SPCustomExtensions;
using SPEventReceiverNotificationsLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPEventReceiverNotificationsLayouts.Layouts.SPEventReceiverNotificationsLayouts
{
    public partial class SiteAllConfigs : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (IsPostBack)
            //    return;
        }
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                BindDataToListsConfigsTable();
            }
            catch (Exception exception)
            {
                HandleException();
            }
            finally
            {
                divloadingImage.Visible = false;
                ListsConfigsTable.Visible = true;
                ListsConfigsTimer.Enabled = false;
            }
        }
        private void BindDataToListsConfigsTable()
        {
            SPSite site = new SPSite(SPContext.Current.Web.Url);
            var listsWithConf = site
                .GetListsWithJSONConf(CommonConstants.LIST_PROPERTY_JSON_CONF)
                .Where(l => !String.IsNullOrEmpty(l.RootFolder.Properties[CommonConstants.LIST_PROPERTY_JSON_CONF]?.ToString()))
                .ToList();
            var arrayForGridView = GetArrayForGridView(listsWithConf);
            ListsConfigsTable.DataSource = arrayForGridView;
            ListsConfigsTable.DataBind();
        }
        public Array GetArrayForGridView(List<SPList> lists)
        {
            Array arrayForGridView = lists
                .SelectMany(list =>
                        PropertyBagConfHelper<List<ConfigItem>>.Get(
                            list.RootFolder.Properties,
                            CommonConstants.LIST_PROPERTY_JSON_CONF)
                        .Select(conf => new { list, conf }).ToList()
                    )
                .Select(listAndConf => new
                {
                    ListTitle = listAndConf.list.Title,
                    ListUrl = listAndConf.list.DefaultViewUrl,
                    ConfModifiedDate = listAndConf.conf.ConfModified,
                    ConfEnabled = listAndConf.conf.Enable,
                    ConfTitle = listAndConf.conf.Title,
                    ConfUrl = String.Format(
                            listAndConf.list.ParentWeb.Url + "/_layouts/15/SPEventReceiverNotificationsLayouts/ConfigEdit.aspx?List={0}&ConfName={1}",
                            "{" + listAndConf.list.ID.ToString() + "}", listAndConf.conf.Title)
                })
                .ToArray();
            return arrayForGridView;
        }
        protected void HandleException()
        {
            divWebPartError.Visible = true;
        }
    }
}