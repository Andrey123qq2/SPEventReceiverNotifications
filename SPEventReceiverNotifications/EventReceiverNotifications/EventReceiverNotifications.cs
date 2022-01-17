using Microsoft.SharePoint;
using SPCustomHelpers;
using SPEventReceiverNotificationsLib;
using SPEventReceiverNotificationsLib.ConfFilters;
using SPEventReceiverNotificationsLib.Infrastructure;
using SPEventReceiverNotificationsLib.SendersHTMLBodyAndSubject;
using System;
using System.Collections.Generic;
using System.Linq;
using ERItemContextAlias = SPCustomHelpers.ERItemContext
    <
        System.Collections.Generic.List<SPEventReceiverNotificationsLib.ConfigItem>,
        SPEventReceiverNotificationsLib.ConfigItemGlobal
    >;

namespace SPEventReceiverNotifications.EventReceiverNotifications
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class EventReceiverNotifications : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being updated.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);
            SPSecurity.RunWithElevatedPrivileges(delegate () { GetItemContextAndProcess(properties); });
        }
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
                SPSecurity.RunWithElevatedPrivileges(delegate () { GetItemContextAndProcess(properties); });
        }
        public override void ItemAttachmentAdded(SPItemEventProperties properties)
        {
            base.ItemAttachmentAdded(properties);
            SPSecurity.RunWithElevatedPrivileges(delegate () { GetItemContextAndProcess(properties); });
        }
        private void GetItemContextAndProcess(SPItemEventProperties properties)
        {
            ERItemContextAlias erRItemContext;
            try
            {
                erRItemContext = new ERItemContextAlias(
                    properties,
                    CommonConstants.LIST_PROPERTY_JSON_CONF,
                    CommonConstants.SITE_PROPERTY_JSON_CONF
                );
            }
            catch (ERItemListItemNullException) { return; }
            try
            {
                ProcessListConfigs(erRItemContext);
            }
            catch (Exception ex)
            {
                var message = String.Format(
                    CommonConstants.ERROR_MESSAGE_TEMPLATE,
                    erRItemContext.CurrentItem.ParentList.ID,
                    erRItemContext.CurrentItem.ID,
                    ex.ToString()
                );
                SPLogger.WriteLog(SPLogger.Category.Unexpected, "CustomER Exception", message);
            }
        }
        private void ProcessListConfigs(ERItemContextAlias context)
        {
            context.ERConf
                .Where(conf => conf.Enable)
                .Where(conf => conf.EventType == context.EventType)
                .Where(conf => AreFiltersPassed(conf, context))
                //.AsParallel().ForAll(conf =>
                .ToList()
                .ForEach(conf => SendNotificationByConf(conf, context));
        }
        private void SendNotificationByConf(ConfigItem conf, ERItemContextAlias context)
        {
            var bodyMacrosResolver = new BodyMacrosResolver(context);
            var bodyCreator = new HTMLBodyCreator(conf.BodyTemplate, context, bodyMacrosResolver);
            SenderBody senderBody = bodyCreator.CreateSenderBody();
            if (!senderBody.HasChangedFields)
                return;
            SenderType sendType = (SenderType)Enum.Parse(typeof(SenderType), conf.SendType);
            ISenderCreator senderCreator = SenderCreatorFactory.GetCreator(sendType, context, conf, senderBody);
            if (conf.SingleMode)
            {
                List<ISender> sendersSingleMode = senderCreator.CreateSenderMulti();
                //Parallel.ForEach(sendersSingleMode, s => s.SendNotification());
                sendersSingleMode
                    .ToList()
                    .ForEach(s => s.SendNotification());
            }
            else
            {
                ISender sender = senderCreator.CreateSender();
                sender.SendNotification();
            }
        }
        private bool AreFiltersPassed(ConfigItem conf, ERItemContextAlias context)
        {
            List<IConfFilter> _confFilters = new List<IConfFilter> {
                new ContentTypeFilter (conf, context),
                new ChangedBySvcAccountsFilter(conf, context)
            };
            return !_confFilters.Any(f => !f.Passed());
        }
    }
}