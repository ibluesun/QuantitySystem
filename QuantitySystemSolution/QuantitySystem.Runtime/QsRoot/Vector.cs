using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Types;
using Qs;
using System.Diagnostics.Contracts;

namespace QsRoot
{
    public static class Vector
    {
        /// <summary>
        /// Returns <see cref="QsVector"/> from to...
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static QsValue Range(QsParameter from, QsParameter to)
        {
            Contract.Requires(from.QsNativeValue is QsScalar);
            Contract.Requires(to.QsNativeValue is QsScalar);

            double fd = ((QsScalar)from.QsNativeValue).NumericalQuantity.Value;
            double td = ((QsScalar)to.QsNativeValue).NumericalQuantity.Value;

            QsVector vec = new QsVector();

            if (td >= fd)
            {
                for (double vl = fd; vl <= td; vl++)
                {
                    vec.AddComponent(vl);
                }
            }
            else
            {
                for (double vl = fd; vl >= td; vl--)
                {
                    vec.AddComponent(vl);
                }

            }

            return vec;
        }

        /// <summary>
        /// Returns <see cref="QsVector"/> from to...  with specified interval
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static QsValue Range(QsParameter from, QsParameter to, QsParameter step)
        {
            Contract.Requires(from.QsNativeValue is QsScalar);
            Contract.Requires(to.QsNativeValue is QsScalar);
            Contract.Requires(step.QsNativeValue is QsScalar);


            double fd = ((QsScalar)from.QsNativeValue).NumericalQuantity.Value;
            double td = ((QsScalar)to.QsNativeValue).NumericalQuantity.Value;

            double stepd = ((QsScalar)step.QsNativeValue).NumericalQuantity.Value;

            QsVector vec = new QsVector();

            if (td >= fd)
            {
                for (double vl = fd; vl <= td; vl += stepd)
                {
                    vec.AddComponent(vl);
                }
            }
            else
            {
                for (double vl = fd; vl >= td; vl -= stepd)
                {
                    vec.AddComponent(vl);
                }
            }

            return vec;
        }


        /// <summary>
        /// Returns the count of vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static QsValue Count(QsParameter vector)
        {
            Contract.Requires(vector.QsNativeValue is QsVector);

            return ((QsVector)vector.QsNativeValue).Count.ToScalarValue();
        }

        /// <summary>
        /// Returns <see cref="QsVector"/> with constant value
        /// </summary>
        /// <param name="count"></param>
        /// <param name="constant"></param>
        /// <returns></returns>
        public static QsValue ConstantRange(QsParameter count, QsParameter constant)
        {
            Contract.Requires(count.QsNativeValue is QsScalar);
            Contract.Requires(constant.QsNativeValue is QsScalar);

            double countd = ((QsScalar)count.QsNativeValue).NumericalQuantity.Value;

            int icount = (int)countd;

            QsVector v = new QsVector(icount);

            for (int i = 0; i < icount; i++)
            {
                v.AddComponent((QsScalar)constant.QsNativeValue);
            }

            return v;
        }

    }
}