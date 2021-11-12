using Microsoft.SharePoint;
using System;
using System.Text.RegularExpressions;

namespace SPItemFieldHelpers
{
    public class SPItemFieldWrapperString : SPItemFieldWrapper
    {

        public SPItemFieldWrapperString(SPListItem listItem, string fieldTitle, SPItemEventProperties properties) : 
            base(listItem, fieldTitle, properties)
        {
        }
        protected override string GetValueBeforeForCompare()
        {
            if (String.IsNullOrEmpty(ValueBeforeRaw?.ToString()))
                return String.Empty;
            else
                return RemoveEmptyElements((string)ValueBeforeRaw);
        }
        protected override string GetValueAfterForCompare()
        {
            if (String.IsNullOrEmpty(ValueAfterRaw?.ToString()))
                return String.Empty;
            else
                return RemoveEmptyElements((string)ValueAfterRaw);
        }
        public override string GetValueAfterFriendly()
        {
            return GetFriendlyFieldValue(ValueAfterRaw);
        }
        public override string GetValueBeforeFriendly()
        {
            return GetFriendlyFieldValue(ValueBeforeRaw);
        }
        private string GetFriendlyFieldValue(dynamic fieldValue)
        {
            if (fieldValue == null)
                return String.Empty;
            return Regex.Replace(fieldValue, @"(href|src)=""/", @"$1=""" + Item.Web.Site.Url + "/");
        }
        private string RemoveEmptyElements(string value)
        {
            string newValue = value.Replace("\r\n", "\n");
            newValue = Regex.Replace(newValue, "<img src=\"[^>]+blank.gif\">", String.Empty);
            newValue = Regex.Replace(newValue, "^<[^>]+></[^>]+>$", String.Empty);
            return newValue;
        }

    }
}