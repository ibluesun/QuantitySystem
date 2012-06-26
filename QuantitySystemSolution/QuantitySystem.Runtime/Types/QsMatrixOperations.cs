using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using Qs.Runtime;
using ParticleLexer;
using ParticleLexer.StandardTokens;

namespace Qs.Types
{
    public partial class QsMatrix : QsValue
    {

        #region Scalar operations

        /// <summary>
        /// Matrix / scalar
        /// </summary>
        /// <param name="scalarQuantity"></param>
        /// <returns></returns>
        public QsMatrix MultiplyScalar(QsScalar scalar)
        {
            QsMatrix Total = new QsMatrix();
            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                List<QsScalar> row = new List<QsScalar>(ColumnsCount);

                for (int IX = 0; IX < this.ColumnsCount; IX++)
                {
                    row.Add(this[IY, IX] * scalar);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;

        }

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
        /// Matrix % Scalar   Remainder of division of matrix
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns></returns>
        private QsValue ModuloScalar(QsScalar scalar)
        {
            QsMatrix Total = new QsMatrix();
            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                List<QsScalar> row = new List<QsScalar>(ColumnsCount);

                for (int IX = 0; IX < this.ColumnsCount; IX++)
                {
                    row.Add(this[IY, IX] % scalar);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;
        }

        /// <summary>
        /// Matrix + scalar
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns></returns>
        private QsMatrix AddScalar(QsScalar scalar)
        {
            QsMatrix Total = new QsMatrix();
            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                List<QsScalar> row = new List<QsScalar>(ColumnsCount);

                for (int IX = 0; IX < this.ColumnsCount; IX++)
                {
                    row.Add(this[IY, IX] + scalar);
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
        private QsMatrix SubtractScalar(QsScalar scalar)
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
        /// Matrix ^ scalar
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public QsMatrix PowerScalar(QsScalar value)
        {
            QsMatrix Total = (QsMatrix)this.Identity; //first get the identity matrix of this matrix.

            int count = Qs.IntegerFromQsValue(value);

            if (count > 0)
            {
                for (int i = 1; i <= count; i++)
                {
                    Total = Total.MultiplyMatrix(this);
                }
            }
            else if (count == 0)
            {
                return Total;   //which id identity already
            }
            else
            {
                count = Math.Abs(count);
                for (int i = 1; i <= count; i++)
                {
                    Total = Total.MultiplyMatrix(this.Inverse);    //multiply the inverses many times
                }
            }

            return Total;

        }



        /// <summary>
        /// Matrix elements ^. scalar
        /// </summary>
        /// <param name="scalarQuantity"></param>
        /// <returns></returns>
        public QsMatrix ElementsPowerScalar(QsScalar scalar)
        {
            QsMatrix Total = new QsMatrix();
            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                List<QsScalar> row = new List<QsScalar>(ColumnsCount);

                for (int IX = 0; IX < this.ColumnsCount; IX++)
                {
                    row.Add(this[IY, IX].PowerScalar(scalar));
                }

                Total.AddRow(row.ToArray());
            }
            return Total;
        }

        #endregion
        


        #region Matrix oeprators
        public static QsMatrix operator *(QsMatrix a, QsScalar b)
        {
            return a.MultiplyScalar(b);
        }

        public static QsMatrix operator *(QsMatrix a, QsMatrix b)
        {
            return a.MultiplyMatrix(b);
        }

        public static QsMatrix operator +(QsMatrix a, QsScalar b)
        {
            return a.AddScalar(b);
        }

        public static QsMatrix operator +(QsMatrix a, QsMatrix b)
        {
            return a.AddMatrix(b);
        }

        public static QsMatrix operator -(QsMatrix a, QsScalar b)
        {
            return a.SubtractScalar(b);
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

        public override QsValue Identity
        {
            get
            {
                if (IsSquared)
                {
                    return MakeIdentity(this.RowsCount);
                }
                else
                {
                    throw new QsMatrixException("No Identity matrix for non square matrix");
                }
            }

        }

        public override QsValue AddOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                return this.AddScalar((QsScalar)value);
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

        public override QsValue SubtractOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                return this.SubtractScalar((QsScalar)value);
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

        public override QsValue MultiplyOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;

            if (value is QsScalar)
            {
                return this.MultiplyScalar((QsScalar)value);
            }
            else if (value is QsVector)
            {
                // return value is column vector
                QsMatrix mvec =  this.MultiplyMatrix(((QsVector)value).ToCoVectorMatrix());

                // make it ordinary vector again.
                return mvec.Columns[0];
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

        public override QsValue DotProductOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                return this.MultiplyScalar((QsScalar)value);
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

        public override QsValue CrossProductOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue DivideOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                return this.DivideScalar((QsScalar)value);
            }
            else if (value is QsVector)
            {
                throw new NotSupportedException();
            }
            else if (value is QsMatrix)
            {
                return this.MultiplyMatrix(((QsMatrix)value).Inverse);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override QsValue ModuloOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                return this.ModuloScalar((QsScalar)value);
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


        public override QsValue PowerOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


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

            if (this.RowsCount < 4)
                return this.Determinant().Sum();
            else
                return Determinant(this);

        }


        public override QsValue PowerDotOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var s = value as QsScalar;
                return this.ElementsPowerScalar(s);
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

        /// <summary>
        /// for the tensor product '(*)'  operator
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue TensorProductOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsMatrix)
            {
                // Kronecker product
                var tm = (QsMatrix)value;

                QsMatrix result = new QsMatrix();

                List<QsMatrix> lm = new List<QsMatrix>();

                for (int i = 0; i < this.RowsCount; i++)
                {
                    QsMatrix rowM = null;
                    for (int j = 0; j < this.ColumnsCount; j++)
                    {
                        QsScalar element = this[i, j];
                        var imat =  (QsMatrix)(tm * element);
                        if (rowM == null) rowM = imat;
                        else rowM = rowM.AppendRightMatrix(imat);
                        
                    }
                    lm.Add(rowM);
                }

                // append vertically all matrices 

                foreach (var rm in lm)
                {

                    result = result.AppendLowerMatrix(rm);
                }

                return result;
            }
            throw new NotImplementedException();
        }


        /// <summary>
        /// '<<' left shift operator
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public override QsValue LeftShiftOperation(QsValue vl)
        {
            QsValue times;
            if (vl is QsReference) times = ((QsReference)vl).ContentValue;
            else times = vl;


            QsMatrix ShiftedMatrix = new QsMatrix();
            foreach (var vec in this)
            {
                ShiftedMatrix.AddVector((QsVector)vec.LeftShiftOperation(times));
            }
            return ShiftedMatrix;
        }


        /// <summary>
        /// '>>' Right Shift Operator
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public override QsValue RightShiftOperation(QsValue vl)
        {
            QsValue times;
            if (vl is QsReference) times = ((QsReference)vl).ContentValue;
            else times = vl;


            QsMatrix ShiftedMatrix = new QsMatrix();
            foreach (var vec in this)
            {
                ShiftedMatrix.AddVector((QsVector)vec.RightShiftOperation(times));
            }
            return ShiftedMatrix;
        }

        #endregion


        #region Relational Operations
        public override bool LessThan(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool GreaterThan(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool LessThanOrEqual(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool GreaterThanOrEqual(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool Equality(QsValue value)
        {
            //if ((object)value == null) return false;
            throw new NotImplementedException();
        }

        public override bool Inequality(QsValue value)
        {
            
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return scalar with two indices
        /// or Vector in one index
        /// </summary>
        /// <param name="indices"></param>
        /// <returns></returns>
        public override QsValue GetIndexedItem(QsParameter[] allIndices)
        {
            int[] indices = new int[allIndices.Length];
            for (int ix = 0; ix < indices.Length; ix++) indices[ix] = (int)((QsScalar)allIndices[ix].QsNativeValue).NumericalQuantity.Value;                

            int icount = indices.Count();
            if (icount == 2)
            {
                return this[indices[0], indices[1]];
            }
            else if (icount == 1)
            {
                return this[indices[0]];
            }
            else
            {
                throw new QsException("Matrices indices only up to two");
            }
        }

        public override void SetIndexedItem(QsParameter[] indices, QsValue value)
        {
            throw new NotImplementedException();
        }


        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue DifferentiateOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                QsMatrix n = new QsMatrix();
                foreach (var row in n.Rows)
                {
                    n.AddRow(row.DifferentiateScalar((QsScalar)value).ToArray());
                }
                return n;
            }
            else
            {
                return base.DifferentiateOperation(value);
            }
        }


        /// <summary>
        /// Removes row at index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public QsMatrix RemoveRow(int rowIndex)
        {
            QsMatrix m = new QsMatrix();
            for (int iy = 0; iy < this.RowsCount; iy++)
            {
                if (iy != rowIndex) m.AddVector(this[iy]);
            }
            return m;
        }


        /// <summary>
        /// Removes column at index
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public QsMatrix RemoveColumn(int columnIndex)
        {
            QsMatrix m = new QsMatrix();
            for (int ix = 0; ix < this.ColumnsCount; ix++)
            {
                if (ix != columnIndex) m.AddColumnVector(this.GetColumnVector(ix));
            }
            return m;
        }

        private QsMatrix _Cofactors;
        public QsMatrix Cofactors
        {
            get
            {
                if (object.ReferenceEquals( _Cofactors , null))
                {
                    _Cofactors = new QsMatrix();

                    bool FirstNegative = false;
                    for (int i = 0; i < RowsCount; i++)
                    {
                        QsVector v = new QsVector(this.ColumnsCount);
                        
                        for (int j = 0; j < ColumnsCount; j++)
                        {
                            var minor = this.RemoveRow(i).RemoveColumn(j);
                            var d = Determinant(minor);
                            if (Math.Pow(-1, i + j) < 0) d = QsScalar.Zero - d;
                            v.AddComponent(d);
                        }

                        FirstNegative = !FirstNegative; //
                        _Cofactors.AddVector(v);
                    }
                }
                return _Cofactors;
            }
        }

        /// <summary>
        /// Transpose of Cofactors matrix
        /// </summary>
        public QsMatrix Adjoint
        {
            get
            {
                return Cofactors.Transpose();
            }
        }

        public QsMatrix Inverse
        {
            get
            {
                return Adjoint.DivideScalar( Determinant(this));
            }
        }


        public override QsValue Execute(Token expression)
        {

            string operation = expression.TokenValue;

            if (operation.Equals("Transpose()", StringComparison.OrdinalIgnoreCase))
                return this.Transpose();

            if (operation.Equals("Identity", StringComparison.OrdinalIgnoreCase))
                return this.Identity;

            if (operation.Equals("Determinant", StringComparison.OrdinalIgnoreCase))
                return QsMatrix.Determinant(this);

            if (operation.Equals("Cofactors", StringComparison.OrdinalIgnoreCase))
                return this.Cofactors;

            if (operation.Equals("Adjoint", StringComparison.OrdinalIgnoreCase))
                return this.Adjoint;

            if (operation.Equals("Inverse", StringComparison.OrdinalIgnoreCase))
                return this.Inverse;


            throw new QsException("Not implemented or Unknow method for the matrix type");
        }
    }
}
