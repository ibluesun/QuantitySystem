using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace Qs.Runtime
{
    public partial class QsMatrix
    {

        #region Matrix oeprators
        public static QsMatrix operator *(QsMatrix a, QsMatrix b)
        {
            return a.MultiplyMatrix(b);
        }

        public static QsMatrix operator +(QsMatrix a, QsMatrix b)
        {
            return a.AddMatrix(b);
        }

        public static QsMatrix operator -(QsMatrix a, QsMatrix b)
        {
            return a.SubtractMatrix(b);
        }

        
        #endregion


        #region Scalar operators
        public static QsMatrix operator +(QsMatrix a, AnyQuantity<double> b)
        {
            return a.AddMatrixToScalar(b);
        }

        public static QsMatrix operator +(AnyQuantity<double> a, QsMatrix b)
        {
            return b.AddScalarToMatrix(a);
        }

        public static QsMatrix operator -(QsMatrix a, AnyQuantity<double> b)
        {
            return a.SubtractMatrixFromScalar(b);
        }

        public static QsMatrix operator -(AnyQuantity<double> a, QsMatrix b)
        {
            return b.SubtractScalarFromMatrix(a);
        }


        public static QsMatrix operator /(QsMatrix a, AnyQuantity<double> b)
        {
            return a.DivideMatrixByScalar(b);
        }

        public static QsMatrix operator /(AnyQuantity<double> a, QsMatrix b)
        {
            return b.DivideScalarByMatrix(a);
        }

        public static QsMatrix operator *(QsMatrix a, AnyQuantity<double> b)
        {
            return a.MultiplyMatrixByScalar(b);
        }

        public static QsMatrix operator *(AnyQuantity<double> a, QsMatrix b)
        {
            return b.MultiplyMatrixByScalar(a);
        }
        #endregion



    }
}
