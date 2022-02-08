using SPEventReceiverNotificationsLib.Infrastructure;
using System;
using System.Collections.Generic;
using ERItemContextAlias = SPCustomHelpers.ERItemContext
    <
        System.Collections.Generic.List<SPEventReceiverNotificationsLib.ConfigItem>,
        SPEventReceiverNotificationsLib.ConfigItemGlobal
    >;

namespace SPEventReceiverNotificationsLib.SendersHTMLBodyAndSubject
{
    class HTMLBodyFieldCreatorsFactory
    {
        private readonly static Dictionary<string, Func<string, ERItemContextAlias, IBodyFieldCreator>> _fieldTypeToCreatorsMap =
            new Dictionary<string, Func<string, ERItemContextAlias, IBodyFieldCreator>>()
            {
                { "currentItem", (fieldTemplate, context) => new HTMLBodyFieldCreator(fieldTemplate, context) },
                { "relatedItem", (fieldTemplate, context) => new HTMLBodyRelatedItemFieldCreator(fieldTemplate, context) }
            };

        public static IBodyFieldCreator GetCreator(string fieldTemplate, ERItemContextAlias context, string fieldType)
        {
            if (String.IsNullOrEmpty(fieldType))
                fieldType = "currentItem";
            return _fieldTypeToCreatorsMap[fieldType].Invoke(fieldTemplate, context);
        }
    }
}
