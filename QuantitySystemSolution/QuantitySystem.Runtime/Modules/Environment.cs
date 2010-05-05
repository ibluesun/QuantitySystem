using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Types;
using Qs.Runtime;

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

        public static QsValue ProcessorCount
        {
            get
            {
                return System.Environment.ProcessorCount.ToQuantity().ToScalar();
            }
        }
    }
}
