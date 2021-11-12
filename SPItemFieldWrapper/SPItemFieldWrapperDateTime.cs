using Microsoft.SharePoint;
using System;
using System.Text.RegularExpressions;

namespace SPItemFieldHelpers
{
    public class SPItemFieldWrapperDateTime : SPItemFieldWrapper
    {
        private readonly bool _dateOnlyFormat;
        private readonly bool _beforeValueInUniversalTime;
        private readonly bool _afterValueInUniversalTime;
        public SPItemFieldWrapperDateTime(SPListItem listItem, string fieldTitle, SPItemEventProperties properties) : 
            base(listItem, fieldTitle, properties)
        {
            SPFieldDateTime fieldDateTime = (SPFieldDateTime)Item.ParentList.Fields.GetField(Title);
            _dateOnlyFormat = fieldDateTime.DisplayFormat == SPDateTimeFieldFormatType.DateOnly;
            _beforeValueInUniversalTime = IsBeforeValueInUniversalTime();
            _afterValueInUniversalTime = IsAfterValueInUniversalTime();
        }
        protected override string GetValueBeforeForCompare()
        {
            string valueForCompare;
            if (String.IsNullOrEmpty(ValueBeforeRaw?.ToString()))
                valueForCompare = String.Empty;
            else if (_beforeValueInUniversalTime)
                valueForCompare = ValueBeforeRaw.ToString("yyyy-MM-ddTHH:mm:ssZ");
            else
                valueForCompare = ValueBeforeRaw.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            return valueForCompare;
        }
        protected override string GetValueAfterForCompare()
        {
            string valueForCompare;
            if (String.IsNullOrEmpty(ValueAfterRaw?.ToString()))
                valueForCompare = String.Empty;
            else if (_afterValueInUniversalTime)
                valueForCompare = DateTime
                    .Parse(ValueAfterRaw.ToString())
                    .ToUniversalTime()
                    .ToString("yyyy-MM-ddTHH:mm:ssZ");
            else
                valueForCompare = DateTime
                    .Parse(Regex.Replace(ValueAfterRaw.ToString(), "Z$", String.Empty))
                    .ToUniversalTime()
                    .ToString("yyyy-MM-ddTHH:mm:ssZ");
            return valueForCompare;
        }
        public override string GetValueBeforeFriendly()
        {
            string valueBeforeFriendly;
            if (String.IsNullOrEmpty(ValueBeforeRaw?.ToString()))
                valueBeforeFriendly = String.Empty;
            else if (_beforeValueInUniversalTime)
                valueBeforeFriendly = ValueBeforeRaw.ToLocalTime().ToString();
            else
                valueBeforeFriendly = ValueBeforeRaw.ToString();

            if (_dateOnlyFormat)
                valueBeforeFriendly = Regex.Replace(valueBeforeFriendly, @"\s[\d:]+$", String.Empty);
            return valueBeforeFriendly;
        }
        public override string GetValueAfterFriendly()
        {
            string valueAfterFriendly;
            if (String.IsNullOrEmpty(ValueAfterRaw?.ToString()))
                valueAfterFriendly = String.Empty;
            else if (_afterValueInUniversalTime)
                valueAfterFriendly = DateTime.Parse(ValueAfterRaw.ToString()).ToString();
            else
                valueAfterFriendly = DateTime.Parse(Regex.Replace(ValueAfterRaw.ToString(), "Z$", String.Empty)).ToString();

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
        private bool IsAfterValueInUniversalTime()
        {
            bool fieldValueInUniversalTime = Item.ParentList.BaseTemplate != SPListTemplateType.Events &&
                Item.ParentList.BaseTemplate != SPListTemplateType.TasksWithTimelineAndHierarchy &&
                ValueAfterRaw != null &&
                ValueAfterRaw?.GetType() != typeof(DateTime);
            return fieldValueInUniversalTime;
        }
    }
}