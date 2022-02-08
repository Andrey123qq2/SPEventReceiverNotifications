using SPCustomHelpers;
using SPEventReceiverNotificationsLib.Infrastructure;
using SPItemFieldHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SPEventReceiverNotificationsLib.SendersHTMLBodyAndSubject
{
    class HTMLBodyFieldCreator : IBodyFieldCreator
    {
        public readonly bool ShowAlways;
        public readonly bool Constant;
        public readonly bool IsChanged;
        private readonly ERItemContext<List<ConfigItem>, ConfigItemGlobal> _context;
        private readonly string _fieldTemplate;
        private readonly Regex _fieldIntNameRegex = new Regex(@"(?<=(data-intname=""))\w+", RegexOptions.IgnoreCase);
        private readonly Regex _fieldShowAlwaysRegex = new Regex(@"data-showalways=""true""", RegexOptions.IgnoreCase);
        private readonly Regex _fieldConstantRegex = new Regex(@"data-constant=""true""", RegexOptions.IgnoreCase);
        private readonly string _commentedFieldTemplate = "<!-- {0} -->";
        private readonly string _fieldIntName;
        private readonly SPItemFieldWrapper _fieldWrapper;
        private readonly string _valueAfterFriendly;
        private readonly string _valueBeforeFriendly;
        public HTMLBodyFieldCreator(string fieldTemplate, ERItemContext<List<ConfigItem>, ConfigItemGlobal> context)
        {
            _context = context;
            _fieldTemplate = fieldTemplate;
            _fieldIntName = _fieldIntNameRegex.Match(fieldTemplate).ToString();
            ShowAlways = _fieldShowAlwaysRegex.IsMatch(fieldTemplate);
            Constant = _fieldConstantRegex.IsMatch(fieldTemplate);
            _fieldWrapper = _context.EventTypeAsync ? 
                SPItemFieldWrapperFactory.Create(_context.CurrentItem, _fieldIntName, null) :
                SPItemFieldWrapperFactory.Create(_context.CurrentItem, _fieldIntName, _context.EventProperties);
            _valueBeforeFriendly = _fieldWrapper.GetValueBeforeFriendly();
            _valueAfterFriendly = _fieldWrapper.GetValueAfterFriendly();
            IsChanged = CheckChangeConditions();
        }
        public string CreateBodyField()
        {
            string filledFieldTemplate = (!Constant && !ShowAlways && !IsChanged) ?
                String.Format(_commentedFieldTemplate, _fieldTemplate) :
                ReplaceAllMacrosInFieldTemplate();
            return filledFieldTemplate;
        }
        private string ReplaceAllMacrosInFieldTemplate()
        {
            StringBuilder fieldTemplateBuilder = new StringBuilder(_fieldTemplate);
            StringBuilder convertedString = fieldTemplateBuilder
                .Replace("{NAME}", _fieldWrapper.DisplayName)
                .Replace("{PREVVALUE}", IsShowAlwaysAndNotChanged() ? String.Empty : _valueBeforeFriendly)
                .Replace("{NEWVALUE}", IsShowAlwaysAndNotChanged() ? _valueBeforeFriendly : _valueAfterFriendly);
            return convertedString.ToString();
        }
        private bool CheckChangeConditions()
        {
            bool checkForChangeConditions = 
                (
                    _fieldTemplate.Contains("{PREVVALUE}") || 
                    !String.IsNullOrEmpty(_valueAfterFriendly)
                ) && _fieldWrapper.ValueIsChanged();
            return checkForChangeConditions;
        }
        private bool IsShowAlwaysAndNotChanged()
        {
            return (ShowAlways || Constant) && !_fieldWrapper.ValueIsChanged();
        }
    }
}