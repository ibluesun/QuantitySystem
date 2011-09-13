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

namespace QsRoot
{
    public static class Math
    {

        static Angle<double> zAngle = (Angle<double>)Unit.ParseQuantity("0<rad>");


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
                        double r = System.Math.Sinh((zAngle + q).Value);
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
                            double r = System.Math.Sinh((zAngle + var.NumericalQuantity).Value);
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
                        double r = System.Math.Cosh((zAngle + q).Value);
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
                            double r = System.Math.Cosh((zAngle + var.NumericalQuantity).Value);
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
                        double r = System.Math.Sin((zAngle + q).Value);
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
                            double r = System.Math.Sin((zAngle + var.NumericalQuantity).Value);
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
                        double r = System.Math.Cos((zAngle + q).Value);
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
                            double r = System.Math.Cos((zAngle+var.NumericalQuantity).Value);
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

        public static QsValue PI
        {
            get
            {
                return System.Math.PI.ToQuantity().ToScalarValue();
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
