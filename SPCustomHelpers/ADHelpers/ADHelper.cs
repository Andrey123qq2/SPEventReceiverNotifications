using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace SPCustomHelpers.ADHelpers
{
    public static class ADHelper
    {
        public static void AddMemberToGroup(string memberId, string groupName, PrincipalContext pc)
        {
            GroupPrincipal group = GroupPrincipal.FindByIdentity(pc, groupName);
            Principal principal = Principal.FindByIdentity(pc, memberId);
            if (group != null && principal != null && group.Members.Contains(principal))
                return;
            group?.Members.Add(pc, IdentityType.SamAccountName, memberId);
            group?.Save();
        }
        public static void RemoveMemberFromGroup(string memberId, string groupName, PrincipalContext pc)
        {
            GroupPrincipal group = GroupPrincipal.FindByIdentity(pc, groupName);
            group?.Members.Remove(pc, IdentityType.SamAccountName, memberId);
            group?.Save();
        }
        public static List<GroupPrincipal> GetUserGroups(UserPrincipal user)
        {
            List<GroupPrincipal> result = new List<GroupPrincipal>();
            if (user != null)
            {
                PrincipalSearchResult<Principal> groups = user.GetAuthorizationGroups();
                foreach (Principal p in groups)
                {
                    if (p is GroupPrincipal principal)
                        result.Add(principal);
                }
            }
            return result;
        }
        public static List<GroupPrincipal> GetGroupByDescription(string description, PrincipalContext pc)
        {
            List<GroupPrincipal> groups;
            string searcherFilter = String.Format("(&(objectClass=group)(description=*{0}*))", description);
            groups = GetDirectoryEntriesByFilter(searcherFilter)
                .Select(r => GroupPrincipal.FindByIdentity(pc, r.Name.Trim().Substring(3)))
                .ToList();
            return groups;
        }
        public static UserPrincipal GetUserByEmail(string email, PrincipalContext pc)
        {
            string searcherFilter = String.Format("(&(objectClass=user)(proxyaddresses=SMTP:{0}))", email);
            DirectoryEntry searchedEntry = GetDirectoryEntriesByFilter(searcherFilter).FirstOrDefault();
            UserPrincipal user = UserPrincipal.FindByIdentity(pc, searchedEntry.Properties["SamAccountName"][0].ToString());
            return user;
        }
        private static List<DirectoryEntry> GetDirectoryEntriesByFilter(string filter)
        {
            DirectorySearcher searcher = new DirectorySearcher
            {
                Filter = filter
            };
            SearchResultCollection searchResults = searcher.FindAll();
            List<DirectoryEntry> directoryEntries = searchResults
                .Cast<SearchResult>()
                .Select(r => r.GetDirectoryEntry())
                .ToList();
            return directoryEntries;
        }
    }
}
