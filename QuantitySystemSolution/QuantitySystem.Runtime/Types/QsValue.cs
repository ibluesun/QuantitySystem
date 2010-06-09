using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Qs.Types
{
    /// <summary>
    /// Base class for all QsValues Scalar, Vector, and Matrix, and later Tensor.
    /// </summary>
    public abstract class QsValue: IComparable
    {

        #region the must inherit functions.
        abstract public QsValue Identity{get;}

        /// <summary>
        /// QsValue + QsValue 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        abstract public QsValue AddOperation(QsValue value);

        /// <summary>
        /// QsValue - QsValue 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        abstract public QsValue SubtractOperation(QsValue value);

        /// <summary>
        /// QsValue * QsValue 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        abstract public QsValue MultiplyOperation(QsValue value);

        /// <summary>
        /// QsValue /  QsValue 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        abstract public QsValue DivideOperation(QsValue value);

        /// <summary>
        /// QsValue ^ QsValue 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        abstract public QsValue PowerOperation(QsValue value);


        /// <summary>
        /// QsValue % QsValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        abstract public QsValue ModuloOperation(QsValue value);


        #region Relational operations
        abstract public bool LessThan(QsValue value);
        abstract public bool GreaterThan(QsValue value);
        abstract public bool LessThanOrEqual(QsValue value);
        abstract public bool GreaterThanOrEqual(QsValue value);

        abstract public bool Equality(QsValue value);

        abstract public bool Inequality(QsValue value);
        #endregion

        /// <summary>
        /// QsValue . QsValue 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        abstract public QsValue DotProductOperation(QsValue value);

        /// <summary>
        /// QsValue ^. QsValue
        /// Power of multiple dot product operations.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        virtual public QsValue PowerDotOperation(QsValue value)
        {
            QsValue Total = this.Identity;

            int count = Qs.IntegerFromQsValue((QsScalar)value);

            for (int i = 1; i <= count; i++)
            {
                Total = Total.DotProductOperation(this);
            }

            return Total;

        }

        /// <summary>
        /// QsValue x QsValue 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        abstract public QsValue CrossProductOperation(QsValue value);



        /// <summary>
        /// Perform the tensor product (tensor outer product)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        abstract public QsValue TensorProductOperation(QsValue value);

        /// <summary>
        /// QsValue ^x QsValue
        /// Power of multiple cross product operations
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        virtual public QsValue PowerCrossOperation(QsValue value)
        {
            QsValue Total = this.Identity;

            int count = Qs.IntegerFromQsValue((QsScalar)value);

            for (int i = 1; i <= count; i++)
            {
                Total =  Total.CrossProductOperation(this);
            }

            return Total;

        }

        /// <summary>
        /// || QsValue ||
        /// </summary>
        /// <returns></returns>
        abstract public QsValue NormOperation();

        /// <summary>
        /// | QsValue |
        /// </summary>
        /// <returns></returns>
        abstract public QsValue AbsOperation();


        abstract public QsValue RightShiftOperation(QsValue times);
        abstract public QsValue LeftShiftOperation(QsValue times);

        /// <summary>
        /// Any QsValue that contain sub items can be accessed by this method
        /// </summary>
        /// <param name="indexes"></param>
        /// <returns></returns>
        abstract public QsValue GetIndexedItem(int[] indices);
        #endregion


        #region Overloaded operators
        public static QsValue operator +(QsValue a, QsValue b)
        {
            return a.AddOperation(b);
        }

        public static QsValue operator -(QsValue a, QsValue b)
        {
            return a.SubtractOperation(b);
        }

        public static QsValue operator *(QsValue a, QsValue b)
        {
            return a.MultiplyOperation(b);
        }

        public static QsValue operator /(QsValue a, QsValue b)
        {
            return a.DivideOperation(b);
        }

        public static QsValue operator %(QsValue a, QsValue b)
        {
            return a.ModuloOperation(b);
        }

        public static QsValue LeftShiftOperator(QsValue a, QsValue b)
        {
            return a.LeftShiftOperation(b);
        }

        public static QsValue RightShiftOperator(QsValue a, QsValue b)
        {
            return a.RightShiftOperation(b);
        }



        #region Relational operators
        public static bool operator <(QsValue a, QsValue b)
        {
            
            return a.LessThan(b);
        }
        public static bool operator <=(QsValue a, QsValue b)
        {
            return a.LessThanOrEqual(b);
        }
        public static bool operator >(QsValue a, QsValue b)
        {
            return a.GreaterThan(b);
        }
        public static bool operator >=(QsValue a, QsValue b)
        {
            return a.GreaterThanOrEqual(b);
        }
        public static bool operator ==(QsValue a, QsValue b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equality(b);
        }
        public static bool operator !=(QsValue a, QsValue b)
        {
            return !(a == b);

        }

        #endregion

        #endregion

        #region QsValue Operations not found in C# operators [Called by the abstracted functions]
        public static QsValue Pow(QsValue a, QsValue b)
        {
            return a.PowerOperation(b);
        }

        public static QsValue PowDot(QsValue a, QsValue b)
        {
            return a.PowerDotOperation(b);
        }

        public static QsValue PowCross(QsValue a, QsValue b)
        {
            return a.PowerCrossOperation(b);
        }

        public static QsValue DotProduct(QsValue a, QsValue b)
        {
            return a.DotProductOperation(b);
        }

        public static QsValue CrossProduct(QsValue a, QsValue b)
        {
            return a.CrossProductOperation(b);
        }

        public static QsValue TensorProduct(QsValue a, QsValue b)
        {
            return a.TensorProductOperation(b);
        }

        #endregion



        #region Helper Methods

        /// <summary>
        /// Parse quantity text.
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns>QsScalar on the form of QsValue</returns>
        public static QsValue ParseScalar(string quantity)
        {
            return new QsScalar { NumericalQuantity = quantity.ToQuantity() };
        }

        /// <summary>
        /// Parse Vector text.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>QsVector on the form of QsValue.</returns>
        public static QsValue ParseVector(string vector)
        {
            string[] qs = vector.Split(',');
            QsVector v = new QsVector(qs.Length);
            foreach (string q in qs)
            {
                v.AddComponent(new QsScalar { NumericalQuantity = q.ToQuantity() });
            }
            return v;
        }

        /// <summary>
        /// Form Vector From QsValues that are scalars or vector.
        /// </summary>
        /// <param name="values">Scalars</param>
        /// <returns></returns>
        public static QsValue VectorFromValues(params QsValue[] values)
        {
            QsVector vec = new QsVector(values.Length);

            foreach (var val in values)
            {
                if (val is QsScalar)
                {
                    vec.AddComponent((QsScalar)val);
                }
                else if (val is QsVector)
                {
                    var vc = val as QsVector;
                    foreach (var component in vc)
                    {
                        vec.AddComponent(component);
                    }
                }
                else
                {
                    throw new QsException("Component is not a scalar or vector value.");
                }
            }

            return vec;
        }


        /// <summary>
        /// Create A matrix from a row values by aligning values to the left in
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static QsValue MatrixRowFromValues(params QsValue[] values)
        {
            // what about [g g] and g is tensor
            // in this case return value should be tensor
            //   from the higher degree of above g
            if (values[0] is QsTensor)
            {
                if (values.Count() == 1) return values[0];

                // in rare cases or event known cases by me Ahmed Sadek and because of parsing 
                // sometimes I enclose tensor variable inside matrix  [g g <|4 3|> ]
                // so in this case i will the return as it is.

                QsTensor tensor = new QsTensor();
                foreach (var inTensor in values)
                {
                    if (inTensor.GetType() != typeof(QsTensor))
                        throw new QsException("Non Tensor in a matrix of tensors is not valid expression.");
                    else
                        tensor.AddInnerTensor((QsTensor)inTensor);
                }
                return tensor;
            }



            QsMatrix m = new QsMatrix();
            
            foreach (var v in values)
            {
                if (v is QsScalar)
                {
                    if (m.RowsCount == 0)
                        m.AddVector((QsVector)VectorFromValues(v));
                    else
                    {
                        m.Rows[0].AddComponent((QsScalar)v);
                    }
                }

                if (v is QsVector)
                {
                    if (m.RowsCount == 0)
                        m.AddVector(((QsVector)v).Clone() as QsVector);
                    else if (m.RowsCount == 1)
                    {
                        m.Rows[0].AddComponents((QsVector)v);
                    }
                    else
                    {
                        throw new QsInvalidOperationException("Couldn't adding vector to multi row matrix");
                    }
                }

                if (v is QsMatrix)
                {
                    if (m.RowsCount == 0)
                    {
                        m = null;
                        m = QsMatrix.CopyMatrix((QsMatrix)v);
                    }
                    else if (m.RowsCount == ((QsMatrix)v).RowsCount)
                    {
                        foreach (var col in ((QsMatrix)v).Columns)
                        {
                            m.AddColumnVector(col);
                        }
                    }
                    else
                    {
                        throw new QsInvalidOperationException("Couldn't adding different row matrices");
                    }
                }
            }

            return m;
        }

        /// <summary>
        /// The function receive rows from vectors or matrices or may be tensors
        /// </summary>
        /// <param name="values">Vectors</param>
        /// <returns></returns>
        public static QsValue MatrixFromValues(params QsValue[] values)
        {


            if (values[0] is QsTensor)
            {
                if (values.Count() == 1) return values[0];

                // treat the case of tensors
                QsTensor tensor = new QsTensor();
                foreach (var tn in values)
                {
                    if (tn.GetType() != typeof(QsTensor))
                        throw new QsException("Non Tensor in a matrix of tensors is not valid expression.");
                    else
                        tensor.AddInnerTensor((QsTensor)tn);
                }
                return tensor;
            }

            QsMatrix mat = new QsMatrix();

            foreach (var val in values)
            {
                if (val is QsVector)
                {
                    mat.AddVector((QsVector)val);
                } 
                else if (val is QsMatrix)
                {
                    foreach (var vc in ((QsMatrix)val))
                    {
                        mat.AddVector((QsVector)vc);
                    }
                }
                else
                {
                    throw new QsInvalidOperationException("Value to be added is not a vector nor a matrix.");
                }
            }

            return mat;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="values">Matrices or Tensors</param>
        /// <returns></returns>
        public static QsValue TensorFromValues(params QsValue[] values)
        {
            QsTensor tens = new QsTensor();

            foreach (var val in values)
            {
                if (val is QsMatrix)
                {
                    tens.AddMatrix((QsMatrix)val);
                }
                else if (val is QsTensor)
                {
                    tens.AddInnerTensor((QsTensor)val);
                }
                else
                {
                    throw new QsException("Component is not a matrix value.");
                }
            }

            return tens;
        }


        #endregion


        /// <summary>
        /// help any inherited class to be converted to its parent class QsValue
        /// </summary>
        /// <returns></returns>
        public QsValue ToQsValue()
        {
            return this;
        }


        #region IComparable Members

        public int CompareTo(object obj)
        {
            QsValue value = obj as QsValue;
            if (obj != null)
            {
                if (this.LessThan(value)) return -1;
                if (this.Equality(value)) return 0;
                if (this.GreaterThan(value)) return 1;
                throw new QsException("Comparison failed between " + this.GetType().Name + " and " + value.GetType().Name);
            }
            else
                throw new QsException("Comparison with values other than QsValue is not permitted");
        }

        #endregion
    }
}
