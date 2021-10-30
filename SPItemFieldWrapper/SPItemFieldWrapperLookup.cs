using Microsoft.SharePoint;
using SPCustomHelpers.SPHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPItemFieldHelpers
{
    public class SPItemFieldWrapperLookup : SPItemFieldWrapper
    {
        private readonly SPFieldLookupValue _lookupValueBefore;
        private readonly SPFieldLookupValue _lookupValueAfter;
        public SPItemFieldWrapperLookup(SPListItem listItem, string fieldTitle, SPItemEventProperties properties) :
            base(listItem, fieldTitle, properties)
        {
            _lookupValueBefore = GetLookupValueByDynamic(ValueBeforeRaw);
            _lookupValueAfter = GetLookupValueByDynamic(ValueAfterRaw);
        }
        protected override string GetValueBeforeForCompare()
        {
            if (String.IsNullOrEmpty(ValueBeforeRaw?.ToString()))
                return String.Empty;
            else
            {
                return _lookupValueBefore.LookupId.ToString();
            }
        }
        protected override string GetValueAfterForCompare()
        {
            if (String.IsNullOrEmpty(ValueAfterRaw?.ToString()))
                return String.Empty;
            else
            {
                return _lookupValueAfter.LookupId.ToString();
            }
        }
        public override string GetValueAfterFriendly()
        {
            return _lookupValueAfter.LookupValue;
        }
        public override string GetValueBeforeFriendly()
        {
            return _lookupValueBefore.LookupValue;
        }
        private SPFieldLookupValue GetLookupValueByDynamic(dynamic fieldValue)
        {
            if (int.TryParse(fieldValue, out int fieldValueInt))
                return GetLookupValue(fieldValueInt);
            else
                return GetLookupValue(fieldValue);
        }
        private SPFieldLookupValue GetLookupValue(string value)
        {
            var lookupValue = new SPFieldLookupValue(value);
            return lookupValue;
        }
        private SPFieldLookupValue GetLookupValue(int value)
        {
            var lookupValue = SPFieldHelpers.GetSPFieldLookupValueById(Field, value);
            return lookupValue;
        }
    }
}