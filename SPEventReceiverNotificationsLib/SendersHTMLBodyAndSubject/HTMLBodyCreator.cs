using SPCustomHelpers;
using SPEventReceiverNotificationsLib.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SPEventReceiverNotificationsLib.SendersHTMLBodyAndSubject
{
    public class HTMLBodyCreator
    {
        protected readonly ERItemContext<List<ConfigItem>, ConfigItemGlobal> _context;
        private readonly Regex _fieldTemplateRegex = new Regex("<span data-intname=[^>]+>.+?</span>");
        private readonly string _bodyTemplate;
        private Dictionary<string, HTMLBodyFieldCreator> _fieldsTemplatesMapCreators;
        private readonly StringBuilder _bodyBuilder;
        private readonly IBodyMacrosResolver _bodyMacrosResolver;
        public HTMLBodyCreator(string bodyTemplate, ERItemContext<List<ConfigItem>, ConfigItemGlobal> context, IBodyMacrosResolver bodyMacrosResolver)
        {
            _context = context;
            _bodyTemplate = bodyTemplate;
            _bodyBuilder = new StringBuilder(_bodyTemplate);
            _bodyMacrosResolver = bodyMacrosResolver;
        }
        public SenderBody CreateSenderBody()
        {
            _fieldsTemplatesMapCreators = GetFieldsTemplatesAndCreators();
            bool hasChangedFields = _fieldsTemplatesMapCreators.Values.Any(c => c.IsChanged && !c.Constant);
            BodyBuilderReplaceFieldsTemplates();
            BodyBuilderReplaceAllGlobalMacros();
            var senderBody = new SenderBody
            {
                Body = _bodyBuilder.ToString(),
                HasChangedFields = hasChangedFields
            };
            return senderBody;
        }
        private Dictionary<string, HTMLBodyFieldCreator> GetFieldsTemplatesAndCreators()
        {
            var fieldsTemplatesMapCreators = _fieldTemplateRegex
                .Matches(_bodyTemplate)
                .Cast<Match>()
                .Select(m => m.ToString())
                .ToDictionary(t => t, t => new HTMLBodyFieldCreator(t, _context));
            return fieldsTemplatesMapCreators;
        }
        private void BodyBuilderReplaceFieldsTemplates()
        {
            _fieldsTemplatesMapCreators
                .ToList()
                .ForEach(pair => _bodyBuilder.Replace(pair.Key, pair.Value.CreateHTMLBodyField()));
        }
        private void BodyBuilderReplaceAllGlobalMacros()
        {
            _bodyMacrosResolver.GetMacrosToValues()
                .ToList()
                .ForEach(pair => _bodyBuilder.Replace(pair.Key, pair.Value));
        }
    }
}