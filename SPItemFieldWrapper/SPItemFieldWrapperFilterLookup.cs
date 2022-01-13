using Microsoft.SharePoint;
using System;

namespace SPItemFieldHelpers
{
    class SPItemFieldWrapperFilteredLookup : SPItemFieldWrapper
    {
        public SPItemFieldWrapperFilteredLookup(SPListItem listItem, string fieldTitle, SPItemEventProperties properties) :
            base(listItem, fieldTitle, properties)
        {
        }
        protected override string GetValueBeforeForCompare()
        {
            if (String.IsNullOrEmpty(ValueBeforeRaw?.ToString()))
                return String.Empty;
            else
                return ((SPFieldLookupValue)ValueBeforeRaw).LookupId.ToString();
        }
        protected override string GetValueAfterForCompare()
        {
            if (String.IsNullOrEmpty(ValueAfterRaw?.ToString()))
                return String.Empty;
            else
                return ((SPFieldLookupValue)ValueAfterRaw).LookupId.ToString();
        }
        public override string GetValueAfterFriendly()
        {
            return ((SPFieldLookupValue)ValueBeforeRaw).LookupValue;
        }
        public override string GetValueBeforeFriendly()
        {
            return ((SPFieldLookupValue)ValueAfterRaw).LookupValue;
        }
    }
}