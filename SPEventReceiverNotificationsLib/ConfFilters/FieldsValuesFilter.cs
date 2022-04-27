using Newtonsoft.Json;
using SPEventReceiverNotificationsLib.Infrastructure;
using SPItemFieldHelpers;
using System.Collections.Generic;
using System.Linq;
using ERItemContextAlias = SPCustomHelpers.ERItemContext
    <
        System.Collections.Generic.List<SPEventReceiverNotificationsLib.ConfigItem>,
        SPEventReceiverNotificationsLib.ConfigItemGlobal
    >;

namespace SPEventReceiverNotificationsLib.ConfFilters
{
    public class FieldsValuesFilter : IConfFilter
    {
        private readonly ConfigItem _conf;
        private readonly ERItemContextAlias _context;
        public FieldsValuesFilter(ConfigItem conf, ERItemContextAlias context)
        {
            _conf = conf;
            _context = context;
        }
        public bool Passed()
        {
            if (string.IsNullOrEmpty(_conf.FieldsValuesFilter))
                return true;
            List<FieldsValuesFilterSingle> fieldsValuesFilter = 
                JsonConvert.DeserializeObject<List<FieldsValuesFilterSingle>>(_conf.FieldsValuesFilter);
            bool filterResult = false;
            bool continueForEach = true;
            foreach (var filter in fieldsValuesFilter)
            {
                if (!continueForEach) break;
                var fieldWrapper = SPItemFieldWrapperFactory.Create(_context.CurrentItem, filter.FieldName, _context.EventProperties);
                filterResult = fieldWrapper.GetValueAfterFriendly() == filter.Value;
                continueForEach = !(filterResult ^ filter.AndMode);
            };
            return filterResult;
        }
    }
}
