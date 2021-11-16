using Microsoft.SharePoint;
using SPCustomHelpers.SPCustomExtensions;
using System.Collections.Generic;
using System.Linq;

namespace SPCustomHelpers
{
    public class SPListContext<T>
    {
        public SPList CurrentList { get; }
        public T ListConf { get; }
        public bool DisableUpdatePermissions = false;
        private ISPListContextStrategy<T> _modifierStrategy;
        public SPListContext(SPList list, string confPopertyName = "")
        {
            CurrentList = list;
            ListConf = PropertyBagConfHelper<T>.Get(list.RootFolder.Properties, confPopertyName);
        }
        public void SetStrategy(ISPListContextStrategy<T> strategy)
        {
            _modifierStrategy = strategy;
        }
        public void ExecuteStrategy()
        {
            if (_modifierStrategy == null)
                return;
            _modifierStrategy.Execute(this);
        }
        public static List<SPListContext<T>> Factory(SPSite site, string confPopertyName)
        {
            List<SPListContext<T>> listsToChange = site.GetListsWithJSONConf(confPopertyName)
                .Select(l => new SPListContext<T>(l, confPopertyName))
                .ToList();
            return listsToChange;
        }
    }
}