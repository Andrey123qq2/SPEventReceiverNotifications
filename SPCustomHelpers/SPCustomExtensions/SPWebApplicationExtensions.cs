using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Collections.Generic;
using System.Linq;

namespace SPCustomHelpers.SPCustomExtensions
{
    public static class SPWebApplicationExtensions
    {
        public static List<SPSite> GetSitesWithFeature(this SPWebApplication webApp, string featureName)
        {
            var sites = webApp.Sites
                .Where(s =>
                {
                    var feature = s.Features.FirstOrDefault(f => f.Definition.DisplayName == featureName);
                    return feature != null;
                }).ToList();
            return sites;
        }
    }

}
