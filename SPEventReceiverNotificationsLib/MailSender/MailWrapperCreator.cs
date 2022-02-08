using Microsoft.SharePoint;
using SPCustomHelpers;
using SPCustomHelpers.SPCustomExtensions;
using SPEventReceiverNotificationsLib.Infrastructure;
using SPEventReceiverNotificationsLib.SendersHTMLBodyAndSubject;
using SPItemFieldHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var to = GetToMails();
            var cc = GetCCMails();
            var bcc = GetBCCMails();
            var subject = GetSubject();
            var body = _senderBody.Body;
            var replyto = GetReplyToAddress();
            MailWrapper mailWrapper = new MailWrapper(to, cc, bcc, subject, body, _context.CurrentItem.ParentList.ParentWeb, replyto);
            return mailWrapper;
        }
        public List<ISender> CreateSenderMulti()
        {
            var to = GetToMails()
                .Concat(GetCCMails())
                .Concat(GetBCCMails())
                .Select(m => m.ToLower())
                .Distinct()
                .ToList();
            var cc = new List<string>();
            var bcc = new List<string>();
            var subject = GetSubject();
            var body = _senderBody.Body;
            var replyto = GetReplyToAddress();
            List<ISender> separateSenders = to
                .Select(m => new MailWrapper(new List<string> { m }, cc, bcc, subject, body, _context.CurrentItem.ParentList.ParentWeb, replyto))
                .Cast<ISender>()
                .ToList();
            return separateSenders;
        }
        private List<string> GetToMails()
        {
            var toMails = GetMailsFromUserFields(_config.ToFields)
                .Concat(_config.ToMails)
                .Concat(GetMailsFromManagersUserFields(_config.MailFieldsManagers))
                .Concat(_context.ERConfGlobal.ToMails)
                .Where(m => !String.IsNullOrEmpty(m))
                .Select(m => m.ToLower())
                .ToList();
            return toMails;
        }
        private List<string> GetCCMails()
        {
            var ccMails = GetMailsFromUserFields(_config.CCFields)
                .Concat(_config.CCMails)
                .Concat(_context.ERConfGlobal.CCMails)
                .Where(m => !String.IsNullOrEmpty(m))
                .Select(m => m.ToLower())
                .ToList();
            return ccMails;
        }
        private List<string> GetBCCMails()
        {
            var bccMails = GetMailsFromUserFields(_config.BCCFields)
                .Concat(_config.BCCMails)
                .Concat(_context.ERConfGlobal.BCCMails)
                .Where(m => !String.IsNullOrEmpty(m))
                .Select(m => m.ToLower())
                .ToList();
            return bccMails;
        }
        private string GetReplyToAddress()
        {
            var replytoCreator = new ReplyToCreator(_config.ReplyToTemplate, _context);
            string replyto = replytoCreator.GetReplyto();
            return replyto;
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
            var managersMails = fields
                .Select(f => SPItemFieldWrapperFactory.Create(_context.CurrentItem, f, _properties))
                .SelectMany(w => w.GetPrincipals())
                .Where(p => p.GetType() == typeof(SPUser))
                .SelectMany(p => ((SPUser)p).GetUserManagers())
                .SelectMany(p => p.GetMails())
                .Select(m => m.ToLower())
                .Except(_config.ExcludedManagersMails.Select(e => e.ToLower()).ToList())
                .ToList();
            return managersMails;
        }
    }
}