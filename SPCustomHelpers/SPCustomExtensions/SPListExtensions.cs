using Microsoft.SharePoint;

namespace SPCustomHelpers.SPCustomExtensions
{
    public static class SPListExtensions
    {
        public static SPListItemCollection QueryItems(this SPList list, string queryFitlerString)
        {
            SPQuery spQuery = new SPQuery
            {
                Query = queryFitlerString
            };
            SPListItemCollection items = list.GetItems(spQuery);
            return items;
        }
    }
}