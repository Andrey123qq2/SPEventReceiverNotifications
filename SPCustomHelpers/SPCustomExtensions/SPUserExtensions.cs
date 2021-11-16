using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Linq;

namespace SPCustomHelpers.SPCustomExtensions
{
    public static class SPUserExtensions
    {
        public static List<SPUser> GetUserManagers(this SPUser user)
        {
            List<SPUser> userManagers = new List<SPUser>();
            SPServiceContext spServiceContext = SPServiceContext.GetContext(user.ParentWeb.Site);
            UserProfileManager userProfileManager = new UserProfileManager(spServiceContext);
            if (!userProfileManager.UserExists(user.LoginName))
                return userManagers;

            UserProfile userProfile = userProfileManager.GetUserProfile(user.LoginName);
            userManagers = userProfile.GetManagers()
                .Select(p => user.ParentWeb.EnsureUser(p.AccountName))
                .ToList();
            return userManagers;
        }
    }
}