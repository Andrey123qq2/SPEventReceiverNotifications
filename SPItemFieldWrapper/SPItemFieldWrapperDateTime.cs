using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPItemFieldHelpers
{
    public class SPItemFieldWrapperDateTime : SPItemFieldWrapper
    {
        public SPItemFieldWrapperDateTime(SPListItem listItem, string fieldTitle, SPItemEventProperties properties) : 
            base(listItem, fieldTitle, properties)
        {
        }

        protected override string GetValueBeforeForCompare()
        {
            dynamic valueBeforeRawConverted = ConvertBeforeValueToLocalTimeByConditions();
            if (String.IsNullOrEmpty(valueBeforeRawConverted?.ToString()))
                return String.Empty;
            if (Item.ParentList.BaseTemplate == SPListTemplateType.Events ||
                Item.ParentList.BaseTemplate == SPListTemplateType.TasksWithTimelineAndHierarchy
            )
                return valueBeforeRawConverted.ToString("yyyy-MM-ddTHH:mm:ssZ");
            else
                return valueBeforeRawConverted.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
        protected override string GetValueAfterForCompare()
        {
            if (String.IsNullOrEmpty(ValueAfterRaw?.ToString()))
                return String.Empty;
            if (ValueAfterRaw != null && ValueAfterRaw.GetType().Name != "DateTime")
                return ValueAfterRaw.ToString();
            else
                return ValueAfterRaw.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
        //TODO: refactor to appropriate particular code
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
            dynamic fieldDateTime = Item.ParentList.Fields.GetField(Title);
            string friendlyFieldValue;
            string fieldValueAsString = fieldValue?.ToString();
            if (String.IsNullOrEmpty(fieldValueAsString))
                return String.Empty;

            if (
                Item.ParentList.BaseTemplate == SPListTemplateType.Events ||
                Item.ParentList.BaseTemplate == SPListTemplateType.TasksWithTimelineAndHierarchy
            )
                friendlyFieldValue = DateTime.Parse(fieldValue.ToString().Replace("Z", "")).ToString();
            else
                friendlyFieldValue =
                    fieldValueAsString.Contains("Z") ?
                    DateTime.Parse(fieldValueAsString).ToLocalTime().ToString() : 
                    DateTime.Parse(fieldValueAsString).ToString();
            if (fieldDateTime.DisplayFormat.ToString() == "DateOnly")
                friendlyFieldValue = Regex.Replace(friendlyFieldValue, @"\s[\d:]+$", "");
            return friendlyFieldValue;
        }
        private dynamic ConvertBeforeValueToLocalTimeByConditions()
        {
            dynamic fieldDateTime = Item.ParentList.Fields.GetField(Title);
            dynamic valueToLocalTimeByConditions = ValueBeforeRaw;
            if (
                 fieldDateTime.DisplayFormat.ToString() == "DateOnly" &&
                 ValueAfterRaw != null &&
                 Regex.IsMatch(ValueAfterRaw.ToString(), @"T00:00:00Z$") &&
                 Item.ParentList.BaseTemplate != SPListTemplateType.Events &&
                 Item.ParentList.BaseTemplate != SPListTemplateType.TasksWithTimelineAndHierarchy
             )
                valueToLocalTimeByConditions =
                    (ValueBeforeRaw != null && ValueBeforeRaw.ToString() != String.Empty) ?
                    ValueBeforeRaw.ToLocalTime() :
                    null;
            return valueToLocalTimeByConditions;
        }
    }
}