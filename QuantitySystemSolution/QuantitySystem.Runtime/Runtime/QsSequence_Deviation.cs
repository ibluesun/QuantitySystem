using System.Collections.Generic;
using System.Globalization;
using Qs.Types;
using System;

namespace Qs.Runtime
{
    public partial class QsSequence : SortedList<int, QsSequenceElement>, IEnumerable<QsValue>
    {
        public QsValue StdDeviation(int fromIndex, int toIndex)
        {
            var n = toIndex - fromIndex + 1;
            if (this.Parameters.Length > 0)
            {
                // this is a call to form symbolic element
                // like g[n](x) ..> x^n
                // and calling g[0++2] 
                //  the output should be x^0 + x^1 + x^2
                //  and be parsed into function  (QsFunction)

                throw new QsException("Not implemented", new NotImplementedException());

            }
            else
            {
                FixIndices(ref fromIndex, ref toIndex);

                var mean = Average(fromIndex, toIndex);

                var Two = "2".ToScalarValue();

                QsValue Total = (GetElementValue(fromIndex) - mean).PowerOperation(Two);


                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    var p = GetElementValue(i) - mean;
                    var pp2 = p.PowerOperation(Two);
                    Total = Total + pp2;
                }

                var count = new QsScalar { NumericalQuantity = Qs.ToQuantity((double)n) };

                return Total / count;            
            }
        }

    }
}
