using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPEventReceiverNotificationsLib.Infrastructure
{
    public interface IConfFilter
    {
        bool Passed();
    }
}
