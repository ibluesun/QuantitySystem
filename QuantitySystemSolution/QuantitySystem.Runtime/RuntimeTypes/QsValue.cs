﻿using System;
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
            return a.Equality(b);
        }
        public static bool operator !=(QsValue a, QsValue b)
        {
            return a.Inequality(b);
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
        #endregion

        #region Helper Methods

        /// <summary>
        /// Parse quantity text.
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns>QsScalar on the form of QsValue</returns>
        public static QsValue ParseScalar(string quantity)
        {
            return new QsScalar { Quantity = quantity.ToQuantity() };
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
                v.AddComponent(new QsScalar { Quantity = q.ToQuantity() });
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
