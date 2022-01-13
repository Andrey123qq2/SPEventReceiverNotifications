using SPCustomHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPEventReceiverNotificationsLib.SendersHTMLBodyAndSubject
{
    class ReplyToCreator
    {
        protected ERItemContext<List<ConfigItem>, ConfigItemGlobal> _context;
        private readonly string _replytoTemplate;
        public ReplyToCreator(string replytoTemplate, ERItemContext<List<ConfigItem>, ConfigItemGlobal> context)
        {
            _context = context;
            _replytoTemplate = replytoTemplate;
        }
        public string GetReplyto()
        {
            StringBuilder replyToBuilder = new StringBuilder(_replytoTemplate);
            replyToBuilder
                .Replace("{ITEMID}", _context.CurrentItem.ID.ToString())
                .Replace("{LISTGUIDBASE64}", Convert.ToBase64String(_context.CurrentItem.ParentList.ID.ToByteArray()).Replace("=", String.Empty));
            return replyToBuilder.ToString();
        }
    }
}
