using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace Qs.RuntimeTypes
{
    public partial class QsMatrix
    {

        #region Scalar operations


        /// <summary>
        /// Matrix / scalar
        /// </summary>
        /// <param name="scalarQuantity"></param>
        /// <returns></returns>
        public QsMatrix DivideScalar(QsScalar scalar)
        {
            QsMatrix Total = new QsMatrix();
            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                List<QsScalar> row = new List<QsScalar>(ColumnsCount);

                for (int IX = 0; IX < this.ColumnsCount; IX++)
                {
                    row.Add(this[IY, IX] / scalar);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;

        }


        /// <summary>
        /// Matrix - scalar
        /// </summary>
        /// <param name="scalarQuantity"></param>
        /// <returns></returns>
        public QsMatrix SubtractScalar(QsScalar scalar)
        {
            QsMatrix Total = new QsMatrix();
            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                List<QsScalar> row = new List<QsScalar>(ColumnsCount);

                for (int IX = 0; IX < this.ColumnsCount; IX++)
                {
                    row.Add(this[IY, IX] - scalar);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;
        }

        /// <summary>
        /// Matrix powered to scalar.
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public QsMatrix PowerScalar(QsScalar scalar)
        {
            QsMatrix Total = this;

            int count = Qs.IntegerFromQsValue(scalar);
            for (int i = 1; i < count; i++)
            {
                Total = Total * Total;
            }

            return Total;
        }

        #endregion



        

        #region Matrix oeprators
        public static QsMatrix operator *(QsMatrix a, QsScalar b)
        {
            return b.MultiplyMatrix(a);
        }

        public static QsMatrix operator *(QsMatrix a, QsMatrix b)
        {
            return a.MultiplyMatrix(b);
        }

        public static QsMatrix operator +(QsMatrix a, QsScalar b)
        {
            return b.AddMatrix(a);
        }

        public static QsMatrix operator +(QsMatrix a, QsMatrix b)
        {
            return a.AddMatrix(b);
        }

        public static QsMatrix operator -(QsMatrix a, QsScalar b)
        {
            return b.SubtractMatrix(a);
        }

        public static QsMatrix operator -(QsMatrix a, QsMatrix b)
        {
            return a.SubtractMatrix(b);
        }
        
        #endregion


        /*
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

        */



        #region QsValue operations
        public override QsValue AddOperation(QsValue value)
        {
            if (value is QsScalar)
            {
                var s = value as QsScalar;
                return s.AddMatrix(this);
            }
            else if (value is QsVector)
            {
                throw new NotSupportedException(); 
            }
            else if (value is QsMatrix)
            {
                return this.AddMatrix((QsMatrix)value);
            }
            else
            {
                throw new NotSupportedException(); 
            }
        }

        public override QsValue SubtractOperation(QsValue value)
        {
            if (value is QsScalar)
            {
                var s = value as QsScalar;
                return s.SubtractMatrix(this);
            }
            else if (value is QsVector)
            {
                throw new NotSupportedException();
            }
            else if (value is QsMatrix)
            {
                return this.SubtractMatrix((QsMatrix)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override QsValue MultiplyOperation(QsValue value)
        {
            if (value is QsScalar)
            {
                var s = value as QsScalar;
                return s.MultiplyMatrix(this);
            }
            else if (value is QsVector)
            {
                throw new NotSupportedException();
            }
            else if (value is QsMatrix)
            {
                return this.MultiplyMatrix((QsMatrix)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override QsValue DotProductOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue CrossProductOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue DivideOperation(QsValue value)
        {
            if (value is QsScalar)
            {
                var s = value as QsScalar;
                return this.DivideScalar(s);
            }
            else if (value is QsVector)
            {
                throw new NotSupportedException();
            }
            else if (value is QsMatrix)
            {
                throw new NotSupportedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override QsValue PowerOperation(QsValue value)
        {
            if (value is QsScalar)
            {
                var s = value as QsScalar;
                return this.PowerScalar(s);
            }
            else if (value is QsVector)
            {
                throw new NotSupportedException();
            }
            else if (value is QsMatrix)
            {
                throw new NotSupportedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override QsValue NormOperation()
        {
            throw new NotImplementedException();
        }

        public override QsValue AbsOperation()
        {
            return this.Determinant().Sum();

        }
        #endregion

    }
}
