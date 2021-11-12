using Microsoft.SharePoint;
using SPCustomHelpers.SPCustomExtensions;
using System;

namespace SPItemFieldHelpers
{
    public abstract class SPItemFieldWrapper
    {
        public readonly string Title;
        public readonly string DisplayName;
        public readonly dynamic ValueBeforeRaw;
        public readonly dynamic ValueAfterRaw;
        public readonly SPListItem Item;
        public readonly SPField Field;
        public string FieldType;
        private readonly SPItemEventProperties _properties;
        public SPItemFieldWrapper(SPListItem listItem, string fieldTitle, SPItemEventProperties properties)
        {
            Item = listItem;
            Title = fieldTitle;
            Field = Item.Fields.GetField(Title);
            FieldType = Field.TypeAsString;
            DisplayName = Field.Title;
            _properties = properties;
            ValueBeforeRaw = Item.GetFieldValue(Title);
            ValueAfterRaw = (properties != null) ? Item.GetFieldValueAfter(_properties, Title) : ValueBeforeRaw;
        }
        protected abstract string GetValueBeforeForCompare();
        protected abstract string GetValueAfterForCompare();
        public abstract string GetValueAfterFriendly();
        public abstract string GetValueBeforeFriendly();
        public bool ValueIsChanged()
        {
            bool valueIsChanged;
            if (_properties != null)
            {
                string valueBeforeToStringForCompare = GetValueBeforeForCompare();
                string valueAfterToStringForCompare = GetValueAfterForCompare();
                valueIsChanged = (valueBeforeToStringForCompare != valueAfterToStringForCompare);
            }
            else
                valueIsChanged = !String.IsNullOrEmpty(ValueAfterRaw?.ToString());

            return valueIsChanged;
        }
    }
}