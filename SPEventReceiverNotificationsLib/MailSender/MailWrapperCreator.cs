using Microsoft.SharePoint;
using SPEventReceiverNotificationsLib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPCustomHelpers;
using SPCustomHelpers.SPCustomExtensions;
using SPEventReceiverNotificationsLib.SendersHTMLBodyAndSubject;
using SPItemFieldHelpers;

namespace SPEventReceiverNotificationsLib.MailSender
{
    class MailWrapperCreator : ISenderCreator
    {
        private readonly ERItemContext<List<ConfigItem>, ConfigItemGlobal> _context;
        private readonly ConfigItem _config;
        private readonly SPItemEventProperties _properties;
        private readonly SenderBody _senderBody;
        public MailWrapperCreator(
            ERItemContext<List<ConfigItem>, ConfigItemGlobal> context,
            ConfigItem config,
            SenderBody senderBody
        )
        {
            _context = context;
            _config = config;
            _properties = _context.EventTypeAsync ? null : _context.EventProperties;
            _senderBody = senderBody;
        }
        public ISender CreateSender()
        {
            var to = GetMailsFromUserFields(_config.ToFields).Concat(_config.ToMails).ToList();
            var cc = GetMailsFromUserFields(_config.CCFields).Concat(_config.CCMails).ToList();
            var bcc = GetMailsFromUserFields(_config.BCCFields).Concat(_config.BCCMails).ToList();
            var subject = GetSubject();
            var body = _senderBody.Body;
            MailWrapper mailWrapper = new MailWrapper(to, cc, bcc, subject, body, _context.CurrentItem.ParentList.ParentWeb);
            return mailWrapper;
        }
        private string GetSubject()
        {
            var subjectCreator = new SubjectCreator(_config.SubjectTemplate, _context);
            string subject = subjectCreator.GetSubject();
            return subject;
        }
        private List<string> GetMailsFromUserFields(List<string> fields)
        {
            var mailsFromUserFields = fields
                .Select(f => SPItemFieldWrapperFactory.Create(_context.CurrentItem, f, _properties))
                .SelectMany(w => w.GetPrincipals())
                .SelectMany(p => p.GetMails())
                .ToList();
            return mailsFromUserFields;
        }
    }
}