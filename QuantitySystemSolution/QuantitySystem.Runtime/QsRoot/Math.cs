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
    public static class Math
    {

        static Angle<double> ZeroAngle = (Angle<double>)Unit.ParseQuantity("0<rad>");


        #region Constants
        public static QsValue PI
        {
            get
            {
                return System.Math.PI.ToQuantity().ToScalarValue();
            }
        }
        #endregion

        #region Functions
        public static QsValue Sinh(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    AnyQuantity<double> q = ((QsScalar)val.ParameterValue).NumericalQuantity;

                    if (q.Dimension.IsDimensionless)
                    {
                        double r = System.Math.Sinh((ZeroAngle + q).Value);
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
                            double r = System.Math.Sinh((ZeroAngle + var.NumericalQuantity).Value);
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
                        rm.AddVector((QsVector)Sinh(QsParameter.MakeParameter(vec, string.Empty)));

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

        public static QsValue Cosh(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    AnyQuantity<double> q = ((QsScalar)val.ParameterValue).NumericalQuantity;

                    if (q.Dimension.IsDimensionless)
                    {
                        double r = System.Math.Cosh((ZeroAngle + q).Value);
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
                            double r = System.Math.Cosh((ZeroAngle + var.NumericalQuantity).Value);
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
                        rm.AddVector((QsVector)Cosh(QsParameter.MakeParameter(vec, string.Empty)));

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

        public static QsValue Sin(QsParameter val)
        {
            
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    AnyQuantity<double> q = ((QsScalar)val.ParameterValue).NumericalQuantity;

                    if (q.Dimension.IsDimensionless)
                    {
                        double r = System.Math.Sin((ZeroAngle + q).Value);
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
                            double r = System.Math.Sin((ZeroAngle + var.NumericalQuantity).Value);
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
                        rm.AddVector((QsVector)Sin(QsParameter.MakeParameter(vec, string.Empty)));

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

        public static QsValue Cos(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    AnyQuantity<double> q = ((QsScalar)val.ParameterValue).NumericalQuantity;

                    if (q.Dimension.IsDimensionless)
                    {
                        double r = System.Math.Cos((ZeroAngle + q).Value);
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
                            double r = System.Math.Cos((ZeroAngle+var.NumericalQuantity).Value);
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
                        rm.AddVector((QsVector)Cos(QsParameter.MakeParameter(vec, string.Empty)));

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


        public static QsValue Log(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    AnyQuantity<double> q = ((QsScalar)val.ParameterValue).NumericalQuantity;

                    if (q.Dimension.IsDimensionless)
                    {
                        double r = System.Math.Log(q.Value);
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
                            double r = System.Math.Log(var.NumericalQuantity.Value);
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

        public static QsValue Log10(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    AnyQuantity<double> q = ((QsScalar)val.ParameterValue).NumericalQuantity;

                    if (q.Dimension.IsDimensionless)
                    {
                        double r = System.Math.Log10(q.Value);
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
                            double r = System.Math.Log10(var.NumericalQuantity.Value);
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
                        rm.AddVector((QsVector)Log10(QsParameter.MakeParameter(vec, string.Empty)));

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

        public static QsValue Floor(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    AnyQuantity<double> q = ((QsScalar)val.ParameterValue).NumericalQuantity;

                    if (q.Dimension.IsDimensionless)
                    {
                        double r = System.Math.Floor(q.Value);
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
                            double r = System.Math.Floor(var.NumericalQuantity.Value);
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
                        rm.AddVector((QsVector)Floor(QsParameter.MakeParameter(vec, string.Empty)));

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


        public static QsValue Ceiling(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    AnyQuantity<double> q = ((QsScalar)val.ParameterValue).NumericalQuantity;

                    if (q.Dimension.IsDimensionless)
                    {
                        double r = System.Math.Ceiling(q.Value);
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
                            double r = System.Math.Ceiling(var.NumericalQuantity.Value);
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
                        rm.AddVector((QsVector)Ceiling(QsParameter.MakeParameter(vec, string.Empty)));

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


        public static QsValue Exp(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    AnyQuantity<double> q = ((QsScalar)val.ParameterValue).NumericalQuantity;

                    if (q.Dimension.IsDimensionless)
                    {
                        double r = System.Math.Exp(q.Value);
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
                            double r = System.Math.Exp(var.NumericalQuantity.Value);
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
                        rm.AddVector((QsVector)Exp(QsParameter.MakeParameter(vec, string.Empty)));

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

            Contract.Requires(xs.ScalarType == ScalarTypes.NumericalQuantity);

            Contract.Requires(ys.ScalarType == ScalarTypes.NumericalQuantity);

            double r = System.Math.Atan2(ys.NumericalQuantity.Value, xs.NumericalQuantity.Value);

            return r.ToQuantity().ToScalarValue();

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
