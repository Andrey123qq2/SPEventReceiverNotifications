using Microsoft.SharePoint;
using System;

namespace SPItemFieldHelpers
{
    class SPItemFieldWrapperFilteredLookup : SPItemFieldWrapper
    {
        private readonly SPFieldLookupValue _lookupValueAfter;
        public SPItemFieldWrapperFilteredLookup(SPListItem listItem, string fieldTitle, SPItemEventProperties properties) :
            base(listItem, fieldTitle, properties)
        {
            _lookupValueAfter = ValueAfterRaw?.GetType().Name == "SPFieldLookupValue" ? ValueAfterRaw : GetLookupValue(ValueAfterRaw);
        }
        protected override string GetValueBeforeForCompare()
        {
            if (String.IsNullOrEmpty(ValueBeforeRaw?.ToString()))
                return String.Empty;
            else
                return ValueBeforeRaw.LookupId.ToString();
        }
        protected override string GetValueAfterForCompare()
        {
            if (String.IsNullOrEmpty(ValueAfterRaw?.ToString()))
                return String.Empty;
            else
                return _lookupValueAfter.LookupId.ToString();
        }
        public override string GetValueAfterFriendly()
        {
            if (String.IsNullOrEmpty(ValueAfterRaw?.ToString()))
                return String.Empty;
            else
                return _lookupValueAfter.LookupValue;
        }
        public override string GetValueBeforeFriendly()
        {
            if (String.IsNullOrEmpty(ValueBeforeRaw?.ToString()))
                return String.Empty;
            else
                return ((SPFieldLookupValue)ValueBeforeRaw)?.LookupValue;
        }
        private SPFieldLookupValue GetLookupValue(string value)
        {
            var lookupValue = new SPFieldLookupValue(value);
            return lookupValue;
        }
    }
}