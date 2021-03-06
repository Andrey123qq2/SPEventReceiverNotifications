using SPEventReceiverNotificationsLib.Infrastructure;
using ERItemContextAlias = SPCustomHelpers.ERItemContext
    <
        System.Collections.Generic.List<SPEventReceiverNotificationsLib.ConfigItem>,
        SPEventReceiverNotificationsLib.ConfigItemGlobal
    >;

namespace SPEventReceiverNotificationsLib.ConfFilters
{
    public class ContentTypeFilter : IConfFilter
    {
        private readonly ConfigItem _conf;
        private readonly ERItemContextAlias _context;
        public ContentTypeFilter(ConfigItem conf, ERItemContextAlias context)
        {
            _conf = conf;
            _context = context;
        }
        public bool Passed()
        {
            return !_conf.ContentTypeFilter.Contains(_context.CurrentItem.ContentType.Name);
        }
    }
}