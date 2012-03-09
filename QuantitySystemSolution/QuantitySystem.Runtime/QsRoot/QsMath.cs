using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Types;
using Qs.Runtime;
using QuantitySystem.Quantities.BaseQuantities;
using Qs;
using QuantitySystem.Units;
using QuantitySystem.Units.Metric.SI;
using QuantitySystem.Quantities.DimensionlessQuantities;
using System.Diagnostics.Contracts;

namespace QsRoot
{
    public static partial class QsMath
    {

        #region Functions


        public static QsValue Log(QsParameter val, QsParameter newBase)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    AnyQuantity<double> q = ((QsScalar)val.ParameterValue).NumericalQuantity;

                    if (q.Dimension.IsDimensionless)
                    {
                        double r = System.Math.Log(q.Value, ((QsScalar)newBase.ParameterValue).NumericalQuantity.Value);
                        return r.ToQuantity().ToScalarValue();
                    }
                    else
                    {
                        throw new QsInvalidInputException("Non dimensionless number");
                    }
                }
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar var in vec)
                    {
                        if (var.NumericalQuantity.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Log(var.NumericalQuantity.Value, ((QsScalar)newBase.ParameterValue).NumericalQuantity.Value);
                            rv.AddComponent(r.ToQuantity().ToScalar());
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless component");
                        }
                    }

                    return rv;
                }
                else if (val.ParameterValue is QsMatrix)
                {
                    QsMatrix mat = (QsMatrix)val.ParameterValue;
                    QsMatrix rm = new QsMatrix();

                    foreach (var vec in mat.Rows)
                    {
                        rm.AddVector((QsVector)Log(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                //not known may be ordinary string
                return null;
            }

        }


        public static QsValue Atan2(QsParameter x, QsParameter y)
        {
            var xs = x.QsNativeValue as QsScalar;
            var ys = y.QsNativeValue as QsScalar;

            double r = System.Math.Atan2(ys.NumericalQuantity.Value, xs.NumericalQuantity.Value);

            return r.ToQuantity().ToScalarValue();

        }

        public static QsValue Sqrt(QsParameter x)
        {
            
            
                var xs = x.QsNativeValue as QsValue;

                return xs.PowerOperation("0.5".ToScalar());
            
            
        }

        #endregion

        public static QsValue Max(QsParameter value)
        {
            if (value.QsNativeValue is QsScalar) return value.QsNativeValue;
            if (value.QsNativeValue is QsVector)
            {
                var vector = value.QsNativeValue as QsVector;
                return (QsValue)vector.Max();
            }
            if (value.QsNativeValue is QsMatrix)
            {
                var matrix = value.QsNativeValue as QsMatrix;
                return (QsValue)matrix.ToArray().Max();
            }

            throw new QsException("Not implemented for above matrix");

        }

        public static QsValue Min(QsParameter value)
        {
            
            if (value.QsNativeValue is QsScalar) return value.QsNativeValue;
            if (value.QsNativeValue is QsVector)
            {
                var vector = value.QsNativeValue as QsVector;
                return (QsValue)vector.Min();
            }
            if (value.QsNativeValue is QsMatrix)
            {
                var matrix = value.QsNativeValue as QsMatrix;
                return (QsValue)matrix.ToArray().Min();
            }

            throw new QsException("Not implemented for above matrix");

        }

    }

}
