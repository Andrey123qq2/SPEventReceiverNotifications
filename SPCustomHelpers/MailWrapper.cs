using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace SPCustomHelpers
{
    public class MailWrapper : ISender
    {
        private readonly string _to;
        private readonly string _cc;
        private readonly string _bcc;
        private readonly string _replyto;
        private readonly string _subject;
        private readonly string _body;
        private readonly StringDictionary _headers;
        private readonly bool _impossibleToSend = false;
        private readonly SPWeb _web;
        public MailWrapper(List<string> to, List<string> cc, List<string> bcc, string subject, string body, SPWeb web, string replyto)
        {
            _to = String.Join(",", to);
            _cc = String.Join(",", cc);
            _bcc = String.Join(",", bcc);
            _subject = subject;
            _body = body;
            _web = web;
            _replyto = replyto;
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
                { "reply-to", _replyto},
                { "content-type", "text/html" }
            };
            if (!String.IsNullOrEmpty(_replyto))
                headers.Add("from", _replyto);
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