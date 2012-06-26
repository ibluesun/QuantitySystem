using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Types;
using QuantitySystem.Quantities.DimensionlessQuantities;
using QuantitySystem.Quantities.BaseQuantities;
using SymbolicAlgebra;
using Qs;

namespace QsRoot.Processor
{
    public class Curve
    {
        readonly SymbolicVariable fx;
        readonly SymbolicVariable fy;
        readonly SymbolicVariable fz;

        public Curve(SymbolicVariable x, SymbolicVariable y, SymbolicVariable z)
        {
            fx = x;
            fy = y;
            fz = z;
        }

        public QsVector Point(double t)
        {
            var x = fx.Execute(t);
            var y = fy.Execute(t);
            var z = fz.Execute(t);

            return new QsVector(x.ToQuantity().ToScalar(), y.ToQuantity().ToScalar(), z.ToQuantity().ToScalar());

        }

        public static Curve GetCurve(SymbolicVariable x, SymbolicVariable y, SymbolicVariable z)
        {
            return new Curve(x, y, z);
        }


    }
}
