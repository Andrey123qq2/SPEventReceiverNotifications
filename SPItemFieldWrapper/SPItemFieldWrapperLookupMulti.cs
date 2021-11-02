using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPItemFieldHelpers
{
    class SPItemFieldWrapperLookupMulti : SPItemFieldWrapper
    {
        public SPItemFieldWrapperLookupMulti(SPListItem listItem, string fieldTitle, SPItemEventProperties properties) :
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
            var valueAfterRawToCollection = new SPFieldLookupValueCollection(ValueAfterRaw.ToString());
            string valueAfterFriendly = ConvertCollectionToNamesString(valueAfterRawToCollection);
            return valueAfterFriendly;
        }
        public override string GetValueBeforeFriendly()
        {
            if (ValueBeforeRaw == null)
                return String.Empty;
            string valueBeforeFriendly = ConvertCollectionToNamesString(ValueBeforeRaw);
            return valueBeforeFriendly;
        }
        private string ConvertCollectionToNamesString(SPFieldLookupValueCollection collection)
        {
            string convertedCollection = String.Join(", ", collection.Cast<SPFieldLookupValue>().Select(p => p.LookupValue).ToList());
            return convertedCollection;
        }
    }
}
