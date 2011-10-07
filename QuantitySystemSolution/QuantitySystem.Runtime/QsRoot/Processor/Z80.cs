using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Types;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;

namespace QsRoot.Processor
{
    
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
        
        
    }
}
