using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPEventReceiverNotificationsLib
{
    public class CommonConstants
    {
        public static readonly string LIST_PROPERTY_JSON_CONF = "er_splistsnotifications_json_conf";
        public static readonly string SITE_PROPERTY_JSON_CONF = "er_splistsnotificationsglobal_json_conf";
        public static readonly string ERROR_MESSAGE_TEMPLATE = "ER custom SPListsNotifications Exception: {0}, {1} " + "[ {2} ].";
    }
}