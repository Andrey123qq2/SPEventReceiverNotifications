using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SPCustomHelpers;
using SPItemFieldHelpers;

namespace SPEventReceiverNotificationsLib.SendersHTMLBodyAndSubject
{
    class HTMLBodyFieldCreator
    {
        public readonly bool ShowAlways;
        public readonly bool IsChanged;
        private readonly ERItemContext<List<ConfigItem>, ConfigItemGlobal> _context;
        private readonly string _fieldTemplate;
        private readonly Regex _fieldIntNameRegex = new Regex(@"(?<=(data-intname=""))\w+");
        private readonly Regex _fieldConstantRegex = new Regex(@"data-showalways=""true""");
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
            ShowAlways = _fieldConstantRegex.IsMatch(fieldTemplate);
            _fieldWrapper = _context.EventTypeAsync ? 
                SPItemFieldWrapperFactory.Create(_context.CurrentItem, _fieldIntName, null) :
                SPItemFieldWrapperFactory.Create(_context.CurrentItem, _fieldIntName, _context.EventProperties);
            _valueBeforeFriendly = _fieldWrapper.GetValueBeforeFriendly();
            _valueAfterFriendly = _fieldWrapper.GetValueAfterFriendly();
            IsChanged = CheckChangeConditions();
        }
        public string CreateHTMLBodyField()
        {
            string filledFieldTemplate = (!ShowAlways && !IsChanged) ?
                String.Format(_commentedFieldTemplate, _fieldTemplate) :
                ReplaceAllMacrosInFieldTemplate();
            return filledFieldTemplate;
        }
        private string ReplaceAllMacrosInFieldTemplate()
        {
            StringBuilder fieldTemplateBuilder = new StringBuilder(_fieldTemplate);
            StringBuilder convertedString = fieldTemplateBuilder
                .Replace("{NAME}", _fieldWrapper.DisplayName)
                .Replace("{PREVVALUE}", _valueBeforeFriendly)
                .Replace("{NEWVALUE}", _valueAfterFriendly);
            return convertedString.ToString();
        }
        private bool CheckChangeConditions()
        {
            bool checkForCommentCondition = _fieldTemplate.Contains("{PREVVALUE}") || !String.IsNullOrEmpty(_valueAfterFriendly);
            return checkForCommentCondition && _fieldWrapper.ValueIsChanged();
        }
    }
}