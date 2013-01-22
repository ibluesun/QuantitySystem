using System.Collections.Generic;
using System.Globalization;
using Qs.Types;
using System;

namespace Qs.Runtime
{
    public partial class QsSequence
    {
        public QsValue StdDeviation(int fromIndex, int toIndex)
        {
            var n = toIndex - fromIndex + 1;
            if (this.Parameters.Length > 0)
            {
                throw new QsException("Standard Deviation with symbolic quantities (I think you went so far ^_^ )", new NotImplementedException());
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


        #region Average Functions
        public QsValue StdDeviation(int fromIndex, int toIndex, QsValue arg0)
        {
            

            var n = toIndex - fromIndex + 1;
            FixIndices(ref fromIndex, ref toIndex);
            var mean = Average(fromIndex, toIndex, arg0);
            var Two = "2".ToScalarValue();
            QsValue Total = (GetElementValue(fromIndex, arg0) - mean).PowerOperation(Two);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                var p = GetElementValue(i, arg0) - mean;
                var pp2 = p.PowerOperation(Two);
                Total = Total + pp2;
            }
            var count = new QsScalar { NumericalQuantity = Qs.ToQuantity((double)n) };
            
            return Total / count;
        }
        public QsValue StdDeviation(int fromIndex, int toIndex, QsValue arg0, QsValue arg1)
        {
            

            var n = toIndex - fromIndex + 1;
            FixIndices(ref fromIndex, ref toIndex);
            var mean = Average(fromIndex, toIndex, arg0, arg1);
            var Two = "2".ToScalarValue();
            QsValue Total = (GetElementValue(fromIndex, arg0, arg1) - mean).PowerOperation(Two);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                var p = GetElementValue(i, arg0, arg1) - mean;
                var pp2 = p.PowerOperation(Two);
                Total = Total + pp2;
            }
            var count = new QsScalar { NumericalQuantity = Qs.ToQuantity((double)n) };
            
            return Total / count;
        }
        public QsValue StdDeviation(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2)
        {
            

            var n = toIndex - fromIndex + 1;
            FixIndices(ref fromIndex, ref toIndex);
            var mean = Average(fromIndex, toIndex, arg0, arg1, arg2);
            var Two = "2".ToScalarValue();
            QsValue Total = (GetElementValue(fromIndex, arg0, arg1, arg2) - mean).PowerOperation(Two);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                var p = GetElementValue(i, arg0, arg1, arg2) - mean;
                var pp2 = p.PowerOperation(Two);
                Total = Total + pp2;
            }
            var count = new QsScalar { NumericalQuantity = Qs.ToQuantity((double)n) };
            
            return Total / count;
        }
        public QsValue StdDeviation(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3)
        {
            

            var n = toIndex - fromIndex + 1;
            FixIndices(ref fromIndex, ref toIndex);
            var mean = Average(fromIndex, toIndex, arg0, arg1, arg2, arg3);
            var Two = "2".ToScalarValue();
            QsValue Total = (GetElementValue(fromIndex, arg0, arg1, arg2, arg3) - mean).PowerOperation(Two);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                var p = GetElementValue(i, arg0, arg1, arg2, arg3) - mean;
                var pp2 = p.PowerOperation(Two);
                Total = Total + pp2;
            }
            var count = new QsScalar { NumericalQuantity = Qs.ToQuantity((double)n) };
            
            return Total / count;
        }
        public QsValue StdDeviation(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4)
        {
            

            var n = toIndex - fromIndex + 1;
            FixIndices(ref fromIndex, ref toIndex);
            var mean = Average(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4);
            var Two = "2".ToScalarValue();
            QsValue Total = (GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4) - mean).PowerOperation(Two);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                var p = GetElementValue(i, arg0, arg1, arg2, arg3, arg4) - mean;
                var pp2 = p.PowerOperation(Two);
                Total = Total + pp2;
            }
            var count = new QsScalar { NumericalQuantity = Qs.ToQuantity((double)n) };
            
            return Total / count;
        }
        public QsValue StdDeviation(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5)
        {
            

            var n = toIndex - fromIndex + 1;
            FixIndices(ref fromIndex, ref toIndex);
            var mean = Average(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            var Two = "2".ToScalarValue();
            QsValue Total = (GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5) - mean).PowerOperation(Two);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                var p = GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5) - mean;
                var pp2 = p.PowerOperation(Two);
                Total = Total + pp2;
            }
            var count = new QsScalar { NumericalQuantity = Qs.ToQuantity((double)n) };
            
            return Total / count;
        }

        public QsValue StdDeviation(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5, QsValue arg6)
        {
            

            var n = toIndex - fromIndex + 1;
            FixIndices(ref fromIndex, ref toIndex);
            var mean = Average(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            var Two = "2".ToScalarValue();
            QsValue Total = (GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6) - mean).PowerOperation(Two);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                var p = GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6) - mean;
                var pp2 = p.PowerOperation(Two);
                Total = Total + pp2;
            }
            var count = new QsScalar { NumericalQuantity = Qs.ToQuantity((double)n) };
            
            return Total / count;
        }
        #endregion

    }
}
