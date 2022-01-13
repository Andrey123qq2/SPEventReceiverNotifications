namespace SPEventReceiverNotificationsLib
{
    public class CommonConstants
    {
        public static readonly string LIST_PROPERTY_JSON_CONF = "er_splistsnotifications_json_conf";
        public static readonly string SITE_PROPERTY_JSON_CONF = "er_splistsnotificationsglobal_json_conf";
        public static readonly string ERROR_MESSAGE_TEMPLATE = "ER custom SPListsNotifications Exception: {0}, {1} " + "[ {2} ].";
        public static readonly string SETTINGS_BODY_FIELD_TEMPLATE_ADDTYPE = "\t\t<!-- {0} -->\r\n\t\t<p><span data-intname=\"{1}\" data-showalways=\"false\" >{{NAME}}: {{NEWVALUE}}</span></p>\r\n";
        public static readonly string SETTINGS_BODY_FIELD_TEMPLATE_UPDATINGTYPE = "\t\t<!-- {0} -->\r\n\t\t<p><span data-intname=\"{1}\" data-showalways=\"false\" >{{NAME}}: <s>{{PREVVALUE}}</s> {{NEWVALUE}}</span></p>\r\n";
    }
}