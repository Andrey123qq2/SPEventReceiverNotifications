using System.Collections.Generic;

namespace SPEventReceiverNotificationsLib.Infrastructure
{
    public interface IBodyMacrosResolver
    {
        Dictionary<string, string> GetMacrosToValues();
    }
}
