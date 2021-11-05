using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SPItemFieldHelpers
{
    public class SPItemFieldWrapperFactory
    {
        private static readonly Dictionary<string, Func<SPListItem, String, SPItemEventProperties, SPItemFieldWrapper>> fieldTypeToWrappersMap = 
            new Dictionary<String, Func<SPListItem, String, SPItemEventProperties, SPItemFieldWrapper>>()
            {
                {  "DateTime",
                    (item, fieldTitle, properties) => new SPItemFieldWrapperDateTime(item, fieldTitle, properties)
                },
                {  "User",
                    (item, fieldTitle, properties) => new SPItemFieldWrapperUser(item, fieldTitle, properties)
                },
                {  "String",
                    (item, fieldTitle, properties) => new SPItemFieldWrapperString(item, fieldTitle, properties)
                },
                {  "Double",
                    (item, fieldTitle, properties) => new SPItemFieldWrapperDouble(item, fieldTitle, properties)
                },
                {  "Number",
                    (item, fieldTitle, properties) => new SPItemFieldWrapperDouble(item, fieldTitle, properties)
                },
                {  "LookupValueCollection",
                    (item, fieldTitle, properties) => new SPItemFieldWrapperLookupValueCollection(item, fieldTitle, properties)
                },
                {  "Lookup",
                    (item, fieldTitle, properties) => new SPItemFieldWrapperLookup(item, fieldTitle, properties)
                },
                {  "LookupMulti",
                    (item, fieldTitle, properties) => new SPItemFieldWrapperLookupMulti(item, fieldTitle, properties)
                },
                {  "UserValueCollection",
                    (item, fieldTitle, properties) => new SPItemFieldWrapperUserValueCollection(item, fieldTitle, properties)
                },
                {  "Boolean",
                    (item, fieldTitle, properties) => new SPItemFieldWrapperBoolean(item, fieldTitle, properties)
                },
                {  "Common",
                    (item, fieldTitle, properties) => new SPItemFieldWrapperCommon(item, fieldTitle, properties)
                }
            };
        public static SPItemFieldWrapper Create(SPListItem item, string fieldTitle, SPItemEventProperties properties)
        {
            string fieldType = item.Fields.GetField(fieldTitle).TypeAsString;
            if (!fieldTypeToWrappersMap.ContainsKey(fieldType))
                fieldType = "Common";
            return fieldTypeToWrappersMap[fieldType].Invoke(item, fieldTitle, properties);
        }
    }
}