using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.RuntimeTypes;

namespace Qs.Modules
{
    public static class Environment
    {
        public static QsValue TickCount
        {
            get
            {
                return System.Environment.TickCount.ToQuantity("ms").ToScalarValue();
            }
        }

    }
}
