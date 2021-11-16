using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SPItemFieldHelpers
{
    public static class SPItemFieldWrapperUserFieldExtension
    {
        public static List<SPPrincipal> GetPrincipals(this SPItemFieldWrapper fieldWrapper)
        {
            List<SPPrincipal> fieldsPrincipals = new List<SPPrincipal>();
            string userLogin;
            SPListItem item = fieldWrapper.Item;
            dynamic fieldValue = fieldWrapper.ValueAfterRaw != null ? fieldWrapper.ValueAfterRaw : fieldWrapper.ValueBeforeRaw;
            if ((fieldValue.GetType().Name == "Int32") || (fieldValue.GetType().Name == "String" && Regex.IsMatch(fieldValue, @"^\d+$")))
            {
                SPPrincipal principal = item.ParentList.ParentWeb.SiteUsers.GetByID(int.Parse(fieldValue.ToString()));
                fieldsPrincipals.Add(principal);
            }
            else
            {
                SPFieldUserValueCollection fieldValueUsers = new SPFieldUserValueCollection(item.Web, fieldValue.ToString());
                foreach (SPFieldUserValue fieldUser in fieldValueUsers)
                {
                    SPPrincipal principal;
                    if (fieldUser.User != null && fieldUser.User.LoginName != "")
                        userLogin = fieldUser.User.LoginName;
                    else
                        userLogin = fieldUser.LookupValue;
                    userLogin = userLogin.Substring(userLogin.IndexOf("\\") + 1);
                    try
                    {
                        principal = item.Web.EnsureUser(userLogin);
                    }
                    catch
                    {
                        try
                        {
                            principal = item.ParentList.ParentWeb.SiteGroups.GetByName(userLogin);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    fieldsPrincipals.Add(principal);
                }
            }
            return fieldsPrincipals;
        }

        //private static readonly IDictionary<Type, Func<SPItemFieldWrapper, List<SPPrincipal>>> _typesToMethodsMap
        //    = new Dictionary<Type, Func<SPItemFieldWrapper, List<SPPrincipal>>>()
        //    {
        //        { typeof(SPItemFieldWrapperUser),  x => GetPrincipalsInternal((SPItemFieldWrapperUser)x)},
        //        { typeof(SPItemFieldWrapperUserValueCollection),  x => GetPrincipalsInternal((SPItemFieldWrapperUserValueCollection)x)}
        //    };
        //public static List<SPPrincipal> GetPrincipals(this SPItemFieldWrapper fieldWrapper)
        //{
        //    return _typesToMethodsMap[fieldWrapper.GetType()](fieldWrapper);
        //}
        //public static List<SPPrincipal> GetPrincipalsInternal(SPItemFieldWrapperUser fieldWrapperUser)
        //{
        //    SPPrincipal principal = fieldWrapperUser.Item.ParentList.ParentWeb.SiteUsers.GetByID(int.Parse(fieldWrapperUser.ValueAfterRaw.ToString()));
        //    return new List<SPPrincipal>() { principal };
        //}
        //public static List<SPPrincipal> GetPrincipalsInternal(SPItemFieldWrapperUserValueCollection fieldWrapperUser)
        //{
        //    List<SPPrincipal> fieldsPrincipals = new List<SPPrincipal>();
        //    SPFieldUserValueCollection fieldValueUsers = 
        //        new SPFieldUserValueCollection(fieldWrapperUser.Item.Web, fieldWrapperUser.ValueAfterRaw.ToString());
        //    foreach (SPFieldUserValue fieldUser in fieldValueUsers)
        //    {
        //        string userLoginFull = (!String.IsNullOrEmpty(fieldUser.User?.LoginName)) ?
        //            fieldUser.User.LoginName :
        //            fieldUser.LookupValue;
        //        string userLogin = userLoginFull.Substring(userLoginFull.IndexOf("\\") + 1);
        //        SPPrincipal principal;
        //        try
        //        {
        //            principal = fieldWrapperUser.Item.Web.EnsureUser(userLogin);
        //        }
        //        catch
        //        {
        //            try
        //            {
        //                principal = fieldWrapperUser.Item.ParentList.ParentWeb.SiteGroups.GetByName(userLogin);
        //            }
        //            catch
        //            {
        //                continue;
        //            }
        //        }
        //        fieldsPrincipals.Add(principal);
        //    }
        //    return fieldsPrincipals;
        //}
    }
}
