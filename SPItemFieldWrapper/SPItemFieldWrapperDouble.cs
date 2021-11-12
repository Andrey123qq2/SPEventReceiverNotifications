using Microsoft.SharePoint;
using System;
using System.Text.RegularExpressions;

namespace SPItemFieldHelpers
{
    public class SPItemFieldWrapperDouble : SPItemFieldWrapper
    {
        public SPItemFieldWrapperDouble(SPListItem listItem, string fieldTitle, SPItemEventProperties properties) : 
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
                return Regex.Replace(ValueAfterRaw.ToString(), @"\.", ",");
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
            var spItemField = Item.ParentList.Fields.GetField(Title);
            if (spItemField is SPFieldNumber number && number.ShowAsPercentage)
                return Item.ParentList.Fields.GetField(Title).GetFieldValueAsText(fieldValue.ToString().Replace(@",", "."));
            else
                return Regex.Replace(fieldValue.ToString(), @"\.", ",");
        }
    }
}