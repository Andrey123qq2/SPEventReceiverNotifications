using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SPCustomHelpers;
using SPCustomHelpers.SPCustomExtensions;

namespace SPEventReceiverNotificationsLib.SendersHTMLBodyAndSubject
{
    public class HTMLBodyCreator
    {
        protected readonly ERItemContext<List<ConfigItem>, ConfigItemGlobal> _context;
        private readonly Regex _fieldTemplateRegex = new Regex("<span data-intname=[^>]+>.+?</span>");
        private readonly string _bodyTemplate;
        private Dictionary<string, HTMLBodyFieldCreator> _fieldsTemplatesMapCreators;
        private readonly StringBuilder _bodyBuilder;
        public HTMLBodyCreator(string bodyTemplate, ERItemContext<List<ConfigItem>, ConfigItemGlobal> context)
        {
            _context = context;
            _bodyTemplate = bodyTemplate;
            _bodyBuilder = new StringBuilder(_bodyTemplate);
        }
        public SenderBody CreateSenderBody()
        {
            _fieldsTemplatesMapCreators = GetFieldsTemplatesAndCreators();
            bool hasChangedFields = _fieldsTemplatesMapCreators.Values.Any(c => c.IsChanged && !c.ShowAlways);
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
            string attachmentUrl = _context.EventProperties.EventType == Microsoft.SharePoint.SPEventReceiverType.ItemAttachmentAdded ? 
                _context.CurrentItem.Web.Url + "/" + _context.EventProperties.AfterUrl?.ToString() :
                String.Empty;
            string attachmentName = Regex.Replace(attachmentUrl, @"^.*\/", "");
            _bodyBuilder
                .Replace("{ITEMURL}", _context.CurrentItem.GetFullUrl())
                .Replace("{ATTACHURL}", attachmentUrl)
                .Replace("{ATTACHNAME}", attachmentName);
        }
    }
}