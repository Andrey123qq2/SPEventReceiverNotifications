using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPCustomHelpers.SPCustomExtensions
{
    public static class SPPrincipalExtensions
    {
        public static List<string> GetMails(this SPPrincipal principal)
        {
            if (principal.GetType().Name == "SPUser")
                return new List<string> { ((SPUser)principal).Email };
            if (principal.GetType().Name == "SPGroup")
                return ((SPGroup)principal).Users
                    .Cast<SPPrincipal>()
                    .ToList()
                    .SelectMany(p => p.GetMails())
                    .ToList();
            return new List<string>();
        }
    }
}
