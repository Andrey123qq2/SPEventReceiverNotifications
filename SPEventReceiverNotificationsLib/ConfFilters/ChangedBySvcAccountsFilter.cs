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
            if (_conf.DisableGlobalAccountExclusion)
                return true;
            string accountMatch = _context.ERConfGlobal.AccountsExclusionsRegexp;
            return !Regex.IsMatch(_context.EventProperties.UserDisplayName, accountMatch);
        }
    }
}
