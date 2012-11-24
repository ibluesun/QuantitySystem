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



        /// <summary>
        /// Test method for returning array of objects
        /// handled in qs as a tuple containing the native objects
        /// </summary>
        /// <returns></returns>
        public static Z80[] GetProcessors()
        {
            Z80[] z = new Z80[5];

            Random r = new Random();
            for (int i = 0; i < 5; i++)
            {
                z[i] = new Z80() { PC = r.NextDouble().ToQuantity().ToScalar() };
            }

            return z;
        }

    }
}
