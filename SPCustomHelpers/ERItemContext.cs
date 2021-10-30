using Microsoft.SharePoint;
using SPCustomHelpers.SPCustomExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPCustomHelpers
{
    public class ERItemContext<T, E>
    {
        public readonly SPListItem CurrentItem;
        public readonly SPListItem RelatedItem;
        public readonly string ItemTitle;
        public readonly SPItemEventProperties EventProperties;
        public readonly string EventType;
        public readonly bool EventTypeAsync;
        public readonly T ERConf;
        public readonly E ERConfGlobal;
        public ERItemContext(SPItemEventProperties properties, string listRootFolderConfPropertyName = "", string siteConfPropertyName = "")
        {
            using (SPSite site = new SPSite(properties.WebUrl))
            using (SPWeb web = site.OpenWeb())
            {
                try
                {
                    CurrentItem = web.Lists[properties.ListId].GetItemById(properties.ListItemId);
                }
                catch
                {
                    CurrentItem = properties.ListItem;
                }
                if (CurrentItem == null)
                    throw new ERItemListItemNullException("ERItem ListItem not found");
            }
            RelatedItem = CurrentItem.GetFirstRelatedItem();
            EventProperties = properties;
            ItemTitle = (CurrentItem.Title != "" && CurrentItem.Title != null) ? CurrentItem.Title : CurrentItem["FileLeafRef"].ToString();
            EventType = properties.EventType.ToString();
            EventTypeAsync = !EventType.EndsWith("ing");

            if (listRootFolderConfPropertyName != String.Empty)
                ERConf = PropertyBagConfHelper<T>.Get(
                    CurrentItem.ParentList.RootFolder.Properties,
                    listRootFolderConfPropertyName
                );

            if (siteConfPropertyName != String.Empty)
                ERConfGlobal = PropertyBagConfHelper<E>.Get(
                    CurrentItem.ParentList.ParentWeb.Site.RootWeb.AllProperties,
                    siteConfPropertyName
                );
        }
    }
}