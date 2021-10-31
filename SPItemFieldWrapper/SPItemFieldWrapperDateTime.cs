using Microsoft.SharePoint;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPItemFieldHelpers
{
    public class SPItemFieldWrapperDateTime : SPItemFieldWrapper
    {
        private readonly bool _dateOnlyFormat;
        private readonly bool _beforeRawDateTimeObjectInUniversalTime;
        public SPItemFieldWrapperDateTime(SPListItem listItem, string fieldTitle, SPItemEventProperties properties) : 
            base(listItem, fieldTitle, properties)
        {
            SPFieldDateTime fieldDateTime = (SPFieldDateTime)Item.ParentList.Fields.GetField(Title);
            _dateOnlyFormat = fieldDateTime.DisplayFormat == SPDateTimeFieldFormatType.DateOnly;
            _beforeRawDateTimeObjectInUniversalTime = IsBeforeValueInUniversalTime();
        }
        protected override string GetValueBeforeForCompare()
        {
            if (String.IsNullOrEmpty(ValueBeforeRaw?.ToString()))
                return String.Empty;
            if (_beforeRawDateTimeObjectInUniversalTime)
                return ValueBeforeRaw.ToString("yyyy-MM-ddTHH:mm:ssZ");
            else
                return ValueBeforeRaw.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
        protected override string GetValueAfterForCompare()
        {
            if (String.IsNullOrEmpty(ValueAfterRaw?.ToString()))
                return String.Empty;
            if (ValueAfterRaw.GetType() == typeof(DateTime))
                return ValueAfterRaw.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            else
                return ValueAfterRaw.ToString();
        }
        public override string GetValueBeforeFriendly()
        {
            if (String.IsNullOrEmpty(ValueBeforeRaw?.ToString()))
                return String.Empty;
            string valueBeforeFriendly;
            if (_beforeRawDateTimeObjectInUniversalTime)
                valueBeforeFriendly = ValueBeforeRaw.ToLocalTime().ToString();
            else
                valueBeforeFriendly = ValueBeforeRaw.ToString();
            if (_dateOnlyFormat)
                valueBeforeFriendly = Regex.Replace(valueBeforeFriendly, @"\s[\d:]+$", String.Empty);
            return valueBeforeFriendly;
        }
        public override string GetValueAfterFriendly()
        {
            if (String.IsNullOrEmpty(ValueAfterRaw?.ToString()))
                return String.Empty;
            string valueAfterFriendly;
            if (ValueAfterRaw.GetType() == typeof(DateTime))
                valueAfterFriendly = ValueAfterRaw.ToString();
            else
                valueAfterFriendly = DateTime.Parse(ValueAfterRaw.ToString()).ToString();
            if (_dateOnlyFormat)
                valueAfterFriendly = Regex.Replace(valueAfterFriendly, @"\s[\d:]+$", String.Empty);
            return valueAfterFriendly;
        }
        private bool IsBeforeValueInUniversalTime()
        {
            bool fieldValueInUniversalTime = Item.ParentList.BaseTemplate != SPListTemplateType.Events &&
                Item.ParentList.BaseTemplate != SPListTemplateType.TasksWithTimelineAndHierarchy &&
                _dateOnlyFormat &&
                ValueAfterRaw != null &&
                Regex.IsMatch(ValueAfterRaw.ToString(), @"T00:00:00Z$");
            return fieldValueInUniversalTime;
        }
    }
}