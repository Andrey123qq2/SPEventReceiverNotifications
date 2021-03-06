using Microsoft.SharePoint;
using System;

namespace SPItemFieldHelpers
{
    class SPItemFieldWrapperBoolean : SPItemFieldWrapper
    {
        public SPItemFieldWrapperBoolean(SPListItem listItem, string fieldTitle, SPItemEventProperties properties) :
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
            if (ValueAfterRaw == null)
                return String.Empty;
            return ValueAfterRaw.ToString();
        }
        public override string GetValueBeforeFriendly()
        {
            if (ValueBeforeRaw == null)
                return String.Empty;
            return ValueBeforeRaw.ToString();
        }
    }
}
