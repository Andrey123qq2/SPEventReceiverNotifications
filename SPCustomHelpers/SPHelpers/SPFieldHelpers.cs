using Microsoft.SharePoint;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SPCustomHelpers.SPHelpers
{
    public class SPFieldHelpers
    {
        public static SPFieldLookupValue GetSPFieldLookupValueById(SPField listField, int lookupId)
        {
            string rootSiteUrl = listField.ParentList.ParentWeb.Site.Url;
            XElement fieldSchemaXml = XElement.Parse(listField.SchemaXml);
            string lookupListId = fieldSchemaXml.Attribute("List").Value;
            string lookupWebId = fieldSchemaXml.Attribute("WebId")?.Value;
            SPList lookupList;
            if (lookupWebId != null)
                lookupList = SPListHelpers.GetSPList(rootSiteUrl, new Guid(lookupWebId), new Guid(lookupListId));
            else
                lookupList = SPListHelpers.GetSPList(listField.ParentList.ParentWeb, new Guid(lookupListId));
            SPListItem lookupItem = lookupList.GetItemById(lookupId);
            var lookupValue = new SPFieldLookupValue(lookupItem.ID, lookupItem["Title"].ToString());
            return lookupValue;
        }
    }
}
