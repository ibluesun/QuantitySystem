using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qs.RuntimeTypes
{
    /// <summary>
    /// Base class for all QsValues Scalar, Vector, and Matrix, and later Tensor.
    /// </summary>
    public abstract class QsValue
    {


        #region the must inherit functions.
        abstract public QsValue AddOperation(QsValue value);

        abstract public QsValue SubtractOperation(QsValue value);
        abstract public QsValue MultiplyOperation(QsValue value);

        abstract public QsValue DivideOperation(QsValue value);

        abstract public QsValue PowerOperation(QsValue value);

        abstract public QsValue DotProductOperation(QsValue value);

        abstract public QsValue CrossProductOperation(QsValue value);

        abstract public QsValue NormOperation();

        abstract public QsValue AbsOperation();

        #endregion


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

        public static QsValue Pow(QsValue a, QsValue b)
        {
            return a.PowerOperation(b);
        }

        public static QsValue DotProduct(QsValue a, QsValue b)
        {
            return a.DotProductOperation(b);
        }

        public static QsValue CrossProduct(QsValue a, QsValue b)
        {
            return a.CrossProductOperation(b);
        }
        

        #region Helper Methods

        public static QsValue ParseScalar(string quantity)
        {
            return new QsScalar { Quantity = quantity.ToQuantity() };
        }

        public static QsValue ParseVector(string vector)
        {
            string[] qs = vector.Split(',');
            QsVector v = new QsVector(qs.Length);
            foreach (string q in qs)
            {
                v.AddComponent(new QsScalar { Quantity = q.ToQuantity() });
            }
            return v;
        }

        /// <summary>
        /// 
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
                    throw new QsException("Component is not a scalar value.");
                }
            }

            return vec;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values">Vectors</param>
        /// <returns></returns>
        public static QsValue MatrixFromValues(params QsValue[] values)
        {
            QsMatrix mat = new QsMatrix();

            foreach (var val in values)
            {
                if (val is QsVector)
                {
                    mat.AddVector((QsVector)val);
                }
                else
                {
                    throw new QsException("Component is not a vector value.");
                }
            }

            return mat;
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


    }
}
