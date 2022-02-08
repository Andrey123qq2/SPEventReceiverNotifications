using SPCustomHelpers;
using SPItemFieldHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SPEventReceiverNotificationsLib.SendersHTMLBodyAndSubject
{
    class HTMLBodyRelatedItemFieldCreator : IBodyFieldCreator
    {
        private readonly ERItemContext<List<ConfigItem>, ConfigItemGlobal> _context;
        private readonly string _fieldTemplate;
        private readonly Regex _fieldIntNameRegex = new Regex(@"(?<=(data-intname=""))\w+", RegexOptions.IgnoreCase);
        private readonly string _fieldIntName;
        private readonly SPItemFieldWrapper _fieldWrapper;
        public HTMLBodyRelatedItemFieldCreator(string fieldTemplate, ERItemContext<List<ConfigItem>, ConfigItemGlobal> context)
        {
            _context = context;
            _fieldTemplate = fieldTemplate;
            _fieldIntName = _fieldIntNameRegex.Match(fieldTemplate).ToString();
            _fieldWrapper = _context.RelatedItem != null ? SPItemFieldWrapperFactory.Create(_context.RelatedItem, _fieldIntName, null) : null;
        }
        public string CreateBodyField()
        {
            string filledFieldTemplate = ReplaceAllMacrosInFieldTemplate();
            return filledFieldTemplate;
        }
        private string ReplaceAllMacrosInFieldTemplate()
        {
            StringBuilder fieldTemplateBuilder = new StringBuilder(_fieldTemplate);
            StringBuilder convertedString = fieldTemplateBuilder
                .Replace("{NAME}", _fieldWrapper?.DisplayName ?? String.Empty)
                .Replace("{VALUE}", _fieldWrapper?.GetValueBeforeFriendly() ?? String.Empty);
            return convertedString.ToString();
        }
    }
}
