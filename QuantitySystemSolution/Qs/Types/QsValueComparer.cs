using System;
using System.Collections.Generic;
using System.Text;

namespace Qs.Types
{
    public class QsValueComparer : IComparer<QsValue>
    {
        public int Compare(QsValue x, QsValue y)
        {
            if (x == null && y == null) return 0;

            if (x.Equality(y)) return 0;

            if (x.LessThan(y)) return -1;
            if (x.GreaterThan(y)) return 1;

            throw new NotImplementedException();
        }
    }
}
