using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Types;
using QuantitySystem.Quantities.DimensionlessQuantities;
using QuantitySystem.Quantities.BaseQuantities;

namespace QsRoot.Processor
{
    public class Curve
    {
        QsFunction iv;
        public Curve(QsFunction s)
        {
            iv = s;
        }

        public AnyQuantity<double> GetValue(AnyQuantity<double> s)
        {
            return iv.Invoke(s);
        }

    }
}
