using Microsoft.SharePoint;
using System;
using System.Linq;

namespace SPItemFieldHelpers
{
    public class SPItemFieldWrapperUserMulti : SPItemFieldWrapper
    {
        public SPItemFieldWrapperUserMulti(SPListItem listItem, string fieldTitle, SPItemEventProperties properties) : 
            base(listItem, fieldTitle, properties)
        {
        }
        protected override string GetValueBeforeForCompare()
        {
            if (String.IsNullOrEmpty(ValueBeforeRaw?.ToString()))
                return String.Empty;
            else
                return String.Join(",", Array.ConvertAll(
                    (SPFieldUserValue[])ValueBeforeRaw.ToArray(), 
                    p => (p.User != null) ? p.User.LoginName : p.LookupValue));
        }
        protected override string GetValueAfterForCompare()
        {
            if (String.IsNullOrEmpty(ValueAfterRaw?.ToString()))
                return String.Empty;
            else
                return String.Join(",", Array.ConvertAll(
                    (new SPFieldUserValueCollection(Item.Web, ValueAfterRaw.ToString())).ToArray(), 
                    p => (p.User != null) ? p.User.LoginName : p.LookupValue));
        }
        public override string GetValueAfterFriendly()
        {
            return GetFriendlyFieldValue(ValueAfterRaw);
        }
        public override string GetValueBeforeFriendly()
        {
            return GetFriendlyFieldValue(ValueBeforeRaw);
        }
        private string GetFriendlyFieldValue(dynamic fieldValue)
        {
            if (fieldValue == null)
                return String.Empty;
            SPFieldUserValueCollection fieldValueUsers = new SPFieldUserValueCollection(Item.Web, fieldValue.ToString());
            return String.Join(",", Array.ConvertAll(
                (new SPFieldUserValueCollection(Item.Web, fieldValue.ToString())).ToArray(),
                p => (p.User != null) ? p.User.LoginName : Item.Web.EnsureUser(p.LookupValue)?.Name ?? p.LookupValue));
        }
    }
}