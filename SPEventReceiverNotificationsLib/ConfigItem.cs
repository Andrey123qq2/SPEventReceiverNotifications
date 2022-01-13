using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPEventReceiverNotificationsLib
{
    public class ConfigItem
    {
        public string Title { get; set; }
        public bool Enable { get; set; }
        public string ConfModified { get; set; }
        public string Notes { get; set; }
        public string SendType { get; set; }
        public string EventType { get; set; }
        public bool SingleMode { get; set; }
        public List<string> ContentTypeFilter { get; set; } = new List<string>();
        public bool DisableGlobalAccountExclusion { get; set; }
        public List<string> ToMails { get; set; } = new List<string>();
        public List<string> CCMails { get; set; } = new List<string>();
        public List<string> BCCMails { get; set; } = new List<string>();
        public List<string> ExcludedManagersMails { get; set; } = new List<string>();
        public string ReplyToTemplate { get; set; }
        public string SubjectTemplate { get; set; }
        public string BodyTemplate { get; set; }
        public List<string> ToFields { get; set; } = new List<string>();
        public List<string> CCFields { get; set; } = new List<string>();
        public List<string> BCCFields { get; set; } = new List<string>();
        public List<string> MailFieldsManagers { get; set; } = new List<string>();
    }
}