using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.RuntimeTypes;
using Qs.Runtime;

namespace Qs.Modules
{
    public static class Matrix
    {
        public static QsValue L(QsParameter m)
        {
            return m.Quantity;
        }
    }
}
