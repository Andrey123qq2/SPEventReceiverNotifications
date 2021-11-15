using SPEventReceiverNotificationsLib.Infrastructure;
using System.Text.RegularExpressions;
using ERItemContextAlias = SPCustomHelpers.ERItemContext
    <
        System.Collections.Generic.List<SPEventReceiverNotificationsLib.ConfigItem>,
        SPEventReceiverNotificationsLib.ConfigItemGlobal
    >;

namespace SPEventReceiverNotificationsLib.ConfFilters
{
    public class ChangedBySvcAccountsFilter : IConfFilter
    {
        private readonly ConfigItem _conf;
        private readonly ERItemContextAlias _context;
        public ChangedBySvcAccountsFilter(ConfigItem conf, ERItemContextAlias context)
        {
            _conf = conf;
            _context = context;
        }
        public bool Passed()
        {
            //TODO: get regexp grom global conf and get disabling from local conf
            string accountMatch = @"app@sharepoint|svc_";
            return !Regex.IsMatch(_context.EventProperties.UserDisplayName, accountMatch);
        }
    }
}
