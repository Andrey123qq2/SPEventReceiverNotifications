using SPCustomHelpers;
using System.Collections.Generic;

namespace SPEventReceiverNotificationsLib.Infrastructure
{
    public interface ISenderCreator
    {
        ISender CreateSender();
        List<ISender> CreateSenderMulti();
    }
}