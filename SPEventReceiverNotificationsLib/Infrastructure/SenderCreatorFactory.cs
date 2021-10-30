using SPEventReceiverNotificationsLib.MailSender;
using SPEventReceiverNotificationsLib.SendersHTMLBodyAndSubject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERItemContextAlias = SPCustomHelpers.ERItemContext
    <
        System.Collections.Generic.List<SPEventReceiverNotificationsLib.ConfigItem>,
        SPEventReceiverNotificationsLib.ConfigItemGlobal
    >;

namespace SPEventReceiverNotificationsLib.Infrastructure
{
    public class SenderCreatorFactory
    {
        private readonly static Dictionary<SenderType, Func<ERItemContextAlias, ConfigItem, SenderBody, ISenderCreator>> _sendTypeToCreatorsMap =
            new Dictionary<SenderType, Func<ERItemContextAlias, ConfigItem, SenderBody, ISenderCreator>>()
            {
                { SenderType.Email, (context, config, senderBody) => new MailWrapperCreator(context, config, senderBody) }
            };

        public static ISenderCreator GetCreator(SenderType type, ERItemContextAlias context, ConfigItem config, SenderBody senderBody)
        {
            return _sendTypeToCreatorsMap[type].Invoke(context, config, senderBody);
        }
    }
}