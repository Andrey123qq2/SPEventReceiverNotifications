using Microsoft.SharePoint;
using SPCustomHelpers;
using SPCustomHelpers.SPCustomExtensions;
using SPEventReceiverNotificationsLib.Infrastructure;
using SPEventReceiverNotificationsLib.SendersHTMLBodyAndSubject;
using SPItemFieldHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var to = GetMailsFromUserFields(_config.ToFields)
                .Concat(_config.ToMails)
                .Concat(GetMailsFromManagersUserFields(_config.MailFieldsManagers))
                .Where(m => !String.IsNullOrEmpty(m))
                .ToList();
            var cc = GetMailsFromUserFields(_config.CCFields)
                .Concat(_config.CCMails)
                .Where(m => !String.IsNullOrEmpty(m))
                .ToList();
            var bcc = GetMailsFromUserFields(_config.BCCFields)
                .Concat(_config.BCCMails)
                .Where(m => !String.IsNullOrEmpty(m))
                .ToList();
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
        private List<string> GetMailsFromManagersUserFields(List<string> fields)
        {
            var mailsFromUserFields = fields
                .Select(f => SPItemFieldWrapperFactory.Create(_context.CurrentItem, f, _properties))
                .SelectMany(w => w.GetPrincipals())
                .Where(p => p.GetType() == typeof(SPUser))
                .SelectMany(p => ((SPUser)p).GetUserManagers())
                .SelectMany(p => p.GetMails())
                .ToList();
            return mailsFromUserFields;
        }
    }
}