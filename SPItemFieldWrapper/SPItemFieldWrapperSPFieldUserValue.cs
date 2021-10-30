using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPItemFieldHelpers
{
    public class SPItemFieldWrapperUser : SPItemFieldWrapper
    {
        public SPItemFieldWrapperUser(SPListItem listItem, string fieldTitle, SPItemEventProperties properties) : 
            base(listItem, fieldTitle, properties)
        {
        }
        protected override string GetValueBeforeForCompare()
        {
            SPFieldUserValue fieldValueBeforeUser = new SPFieldUserValue(Item.Web, ValueBeforeRaw.ToString());
            if (String.IsNullOrEmpty(ValueBeforeRaw?.ToString()))
                return String.Empty;
            else
                return fieldValueBeforeUser.User != null ? fieldValueBeforeUser.User.LoginName : fieldValueBeforeUser.LookupValue;
        }
        protected override string GetValueAfterForCompare()
        {
            if (String.IsNullOrEmpty(ValueAfterRaw?.ToString()))
                return String.Empty;
            SPFieldUserValue fieldValueAfterUser = (ValueAfterRaw == null) ?
                new SPFieldUserValue() :
                new SPFieldUserValue(Item.Web, ValueAfterRaw.ToString());
            if (!String.IsNullOrEmpty(fieldValueAfterUser.User?.LoginName))
                return fieldValueAfterUser.User.LoginName;
            if (!String.IsNullOrEmpty(fieldValueAfterUser.LookupValue))
                return fieldValueAfterUser.LookupValue;
            return new SPFieldUserValue(Item.Web, ValueAfterRaw.ToString()).User.LoginName;
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
            try
            {
                return Item.Web.EnsureUser(new SPFieldUserValue(Item.Web, fieldValue.ToString()).LookupValue).Name;
            }
            catch
            {
                return new SPFieldUserValue(Item.Web, fieldValue.ToString()).User?.Name ?? String.Empty;
            }
        }
    }
}