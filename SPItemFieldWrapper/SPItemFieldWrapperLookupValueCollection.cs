using Microsoft.SharePoint;
using System;

namespace SPItemFieldHelpers
{
    public class SPItemFieldWrapperLookupValueCollection : SPItemFieldWrapper
    {
        public SPItemFieldWrapperLookupValueCollection(SPListItem listItem, string fieldTitle, SPItemEventProperties properties) : 
            base(listItem, fieldTitle, properties)
        {
        }
        protected override string GetValueBeforeForCompare()
        {
            if (String.IsNullOrEmpty(ValueBeforeRaw?.ToString()))
                return String.Empty;
            else
                return ValueBeforeRaw.ToString();
        }
        protected override string GetValueAfterForCompare()
        {
            if (String.IsNullOrEmpty(ValueAfterRaw?.ToString()))
                return String.Empty;
            else
                return ValueAfterRaw.ToString();
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
            return String.Join(",", Array.ConvertAll(
                new SPFieldLookupValueCollection(fieldValue).ToArray(), 
                p => p.LookupValue));
        }
    }
}
