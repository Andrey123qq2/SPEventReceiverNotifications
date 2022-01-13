using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPCustomHelpers.SPCustomExtensions
{
    public static class SPSiteExtensions
    {
        public static List<SPList> GetListsWithJSONConf(this SPSite site, string confFilter)
        {
            var listsWithJSONConf = new List<SPList>();
            site.AllWebs.Cast<SPWeb>().ToList().ForEach(w =>
            {
                w.Lists.Cast<SPList>().ToList()
                .Where(l => l.RootFolder.Properties.Contains(confFilter))
                .ToList()
                .ForEach(l => listsWithJSONConf.Add(l));
            });
            return listsWithJSONConf;
        }
        public static SPList GetListsById(this SPSite site, Guid id)
        {
            SPList webList = null;
            var siteAllWebs = site.AllWebs.Cast<SPWeb>().ToList();
            foreach (var web in siteAllWebs)
            {
                webList = web.Lists
                    .Cast<SPList>()
                    .ToList()
                    .FirstOrDefault(l => l.ID == id);
                if (webList != null)
                    break;
            };
            return webList;
        }
    }
}