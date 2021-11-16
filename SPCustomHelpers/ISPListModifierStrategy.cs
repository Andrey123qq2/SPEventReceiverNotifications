using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPCustomHelpers
{
    public interface ISPListContextStrategy<T>
    {
        void Execute(SPListContext<T> context);
    }

}
