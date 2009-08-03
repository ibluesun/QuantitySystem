using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qs.Runtime
{
    public enum QsParamType
    {
        AnyQuantity,
        Function,
        Sequence
    }

    public class QsParamInfo
    {
        public string Name { get; set; }
        public QsParamType Type { get; set; }
    }
}
