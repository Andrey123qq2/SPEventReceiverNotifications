using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPEventReceiverNotificationsLib
{
    public class ConfigItemGlobal
    {
        public List<string> ToMails { get; set; } = new List<string> { };
        public List<string> CCMails { get; set; } = new List<string> { };
        public List<string> BCCMails { get; set; } = new List<string> { };
        public string AccountsExclusionsRegexp { get; set; }
        public string BodyTemplate { get; set; }
    }
}