using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Qs;
using Qs.Types;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QsRoot.Processor
{
    
    /// <summary>
    /// Z80 Class is for testing purposes only
    /// there are no important thing here
    /// </summary>
    public class Z80
    {

        public static QsScalar R { get; set; }

        
        static Z80()
        {
            R = QsScalar.Zero;
        }

        public QsValue AF { get; set; }
        public QsValue BC { get; set; }
        public QsValue DE { get; set; }
        public QsValue HL { get; set; }

        public QsValue IX { get; set; }
        public QsValue IY { get; set; }

        public QsValue SP { get; set; }
        public QsValue PC { get; set; }

        public Z80()
        {
            AF = QsScalar.Zero;
            BC = QsScalar.Zero;
            DE = QsScalar.Zero;
            HL = QsScalar.Zero;
            IX = QsScalar.Zero;
            IY = QsScalar.Zero;
            SP = QsScalar.Zero;
            PC = QsScalar.Zero;
        }

        public double Sum(double a, double b) 
        { 
            return a + b; 
        }

        public static Length<double> SumQ(Length<double> x, Length<double> y)
        {
            return (Length<double>)(x + y);
        }


        public static AnyQuantity<double> Square(AnyQuantity<double> x)
        {
            return x * x;
        }



        public static QsVector Func(QsFunction f, DimensionlessQuantity<double> from, DimensionlessQuantity<double> to)
        {
            var increment = (to - from) / (40).ToQuantity();
            QsVector v = new QsVector(40);

            for (AnyQuantity<double> dt = from; dt <= to; dt += increment)
            {
                v.AddComponent(f.Invoke(dt).ToScalar());
            }

            return v;
        }

        public QsValue RPC
        {
            get
            {
                return R + PC;
            }
        }

        public double Accumulate(params double[] all)
        {
            return all.Sum();
        }


        public static Z80 LoadPC(int step)
        {
            var z = new Z80();
            z.PC = ((double)step).ToQuantity().ToScalar();
            return z;
        }

        public static double length(QsVector v)
        {
            return v.Count;
        }
    }
}
