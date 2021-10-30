using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPEventReceiverNotificationsLib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPEventReceiverNotificationsLib.MailSender
{
    class MailWrapper : ISender
    {
        private readonly string _to;
        private readonly string _cc;
        private readonly string _bcc;
        private readonly string _subject;
        private readonly string _body;
        private readonly StringDictionary _headers;
        private readonly bool _impossibleToSend = false;
        private readonly SPWeb _web;
        public MailWrapper(List<string> to, List<string> cc, List<string> bcc, string subject, string body, SPWeb web)
        {
            _to = String.Join(",", to);
            _cc = String.Join(",", cc);
            _bcc = String.Join(",", bcc);
            _subject = subject;
            _body = body;
            _web = web;
            _headers = GetHeaders();
            _impossibleToSend = _body == String.Empty || (_to == String.Empty && _cc == String.Empty && _bcc == String.Empty);
        }
        private StringDictionary GetHeaders()
        {
            StringDictionary headers = new StringDictionary {
                { "to", _to },
                { "cc", _cc },
                { "bcc", _bcc },
                { "subject", _subject },
                { "content-type", "text/html" }
            };
            return headers;
        }
        public void SendNotification()
        {
            if (_impossibleToSend)
                return;
            SPUtility.SendEmail(_web, _headers, _body);
        }
    }
}