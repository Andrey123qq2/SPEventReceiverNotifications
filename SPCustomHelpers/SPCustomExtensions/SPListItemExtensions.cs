using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPCustomHelpers.SPCustomExtensions
{
    public static class SPListItemExtensions
    {
        public static dynamic GetFieldValueAfter(this SPListItem item, SPItemEventProperties properties, string fieldTitle)
        {
            dynamic fieldValueAfter;
            string fieldInternalName;
            string fieldStaticName;
            try
            {
                fieldInternalName = item.ParentList.Fields[fieldTitle].InternalName;
                fieldStaticName = item.ParentList.Fields[fieldTitle].StaticName;
            }
            catch
            {
                fieldInternalName = fieldTitle;
                fieldStaticName = fieldTitle;
            }
            fieldValueAfter = properties.AfterProperties[fieldInternalName];
            if (fieldValueAfter == null)
            {
                fieldValueAfter = properties.AfterProperties[fieldTitle];
                if (fieldValueAfter == null)
                    fieldValueAfter = properties.AfterProperties[fieldStaticName];
            }
            return fieldValueAfter;
        }
        public static dynamic GetFieldValue(this SPListItem item, string fieldTitle)
        {
            if (!item.Fields.ContainsField(fieldTitle))
                return String.Empty;

            dynamic fieldValue;
            string fieldInternalName;
            try
            {
                fieldInternalName = item.ParentList.Fields[fieldTitle].InternalName;
            }
            catch
            {
                fieldInternalName = fieldTitle;
            }
            fieldValue = item[fieldInternalName];
            if (fieldValue == null)
                fieldValue = String.Empty;

            return fieldValue;
        }
        public static bool IsJustCreated(this SPListItem listItem)
        {
            DateTime itemTimeCreated = (DateTime)listItem["Created"];
            DateTime itemTimeModified = (DateTime)listItem["Modified"];
            double diffInSeconds = (itemTimeModified - itemTimeCreated).TotalSeconds;
            if (diffInSeconds < 2)
                return true;
            else
                return false;
        }

        public static SPListItem GetImpersonatedItem(this SPListItem item, string impUserLogin)
        {
            SPListItem impItem;
            SPUser ensureUser = item.Web.EnsureUser(impUserLogin);
            using (SPSite impSite = new SPSite(item.Web.Site.Url, ensureUser.UserToken))
            using (SPWeb impWeb = impSite.OpenWeb())
            {
                SPList impList = impWeb.GetList(item.ParentList.DefaultDisplayFormUrl);
                impItem = impList.GetItemById(item.ID);
            }
            return impItem;
        }
        public static bool IsFieldAppendOnlyNote(this SPListItem item, string fieldName)
        {
            SPField field = item.Fields.GetField(fieldName);
            return field.TypeAsString == "Note" && ((SPFieldMultiLineText)field).AppendOnly;
        }
        public static void UpdateFields(this SPListItem item, Dictionary<string, Object> fieldsWithNewValues, bool systemUpdate = true)
        {
            foreach (var pair in fieldsWithNewValues)
                item[pair.Key] = pair.Value;

            if (fieldsWithNewValues.Count > 0)
            {
                if (systemUpdate)
                    item.SystemUpdate();
                else
                    item.Update();
            }
        }
        public static List<string> GetFieldsNames(this SPListItem item)
        {
            var itemFieldsNames = item.Fields
                .Cast<SPField>()
                .Where(f => !f.ReadOnlyField && !f.Hidden)
                .Select(f => f.Title).ToList();
            return itemFieldsNames;
        }
        public static List<string> GetContentTypeFieldsNames(this SPListItem item)
        {
            var itemFieldsNames = item.ContentType.Fields
                .Cast<SPField>()
                .Where(f => !f.ReadOnlyField && !f.Hidden)
                .Select(f => f.Title).ToList();
            return itemFieldsNames;
        }
        public static SPListItem GetFirstRelatedItem(this SPListItem item)
        {
            var relatedItems = item.GetRelatedItems();
            var relatedItem = relatedItems.Count > 0 ? relatedItems[0] : null;
            return relatedItem;
        }
        public static List<SPListItem> GetRelatedItems(this SPListItem item)
        {
            List<SPListItem> relatedItems = new List<SPListItem>();
            dynamic relatedItemsItemObject;
            try
            {
                relatedItemsItemObject = item[SPBuiltInFieldId.RelatedItems];
            }
            catch {
                return relatedItems;
            }
            if (relatedItemsItemObject == null)
                return relatedItems;

            string relatedItemsString = relatedItemsItemObject.ToString();
            dynamic relatedItemsJson = JsonConvert.DeserializeObject(relatedItemsString);
            foreach (dynamic relItem in relatedItemsJson)
            {
                Guid relatedlistId = new Guid(relItem["ListId"].ToString());
                SPList relatedList = item.Web.Lists[relatedlistId];
                SPListItem relatedItem;
                try
                {
                    relatedItem = relatedList.GetItemById((int)relItem["ItemId"]);
                }
                catch (Exception)
                {
                    continue;
                }
                relatedItems.Add(relatedItem);
            }
            return relatedItems;
        }
        public static string GetFullUrl(this SPListItem item)
        {
            return item.Web.Site.Url + item.ParentList.DefaultDisplayFormUrl + "?ID=" + item.ID;
        }

    }
}
