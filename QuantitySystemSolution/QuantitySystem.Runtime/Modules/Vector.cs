using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Types;
using Microsoft.Scripting.Utils;

namespace Qs.Modules
{
    public static class Vector
    {
        public static QsValue Range(QsParameter from, QsParameter to)
        {
            ContractUtils.Requires(from.Quantity is QsScalar);
            ContractUtils.Requires(to.Quantity is QsScalar);

            double fd = ((QsScalar)from.Quantity).Quantity.Value;
            double td = ((QsScalar)to.Quantity).Quantity.Value;

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

        public static QsValue Range(QsParameter from, QsParameter to, QsParameter step)
        {
            ContractUtils.Requires(from.Quantity is QsScalar);
            ContractUtils.Requires(to.Quantity is QsScalar);
            ContractUtils.Requires(step.Quantity is QsScalar);


            double fd = ((QsScalar)from.Quantity).Quantity.Value;
            double td = ((QsScalar)to.Quantity).Quantity.Value;

            double stepd = ((QsScalar)step.Quantity).Quantity.Value;

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



        public static QsValue Count(QsParameter vector)
        {
            ContractUtils.Requires(vector.Quantity is QsVector);

            return ((QsVector)vector.Quantity).Count.ToScalarValue();
        }
    }
}
