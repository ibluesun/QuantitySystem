using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.RuntimeTypes;
using Qs.Runtime;
using QuantitySystem.Quantities.BaseQuantities;

namespace Qs.Modules
{
    public static class QsMath
    {

        public static QsValue Sinh(QsParameter val)
        {
            if (val.IsKnown)
            {
                if (val.Value is QsScalar)
                {
                    AnyQuantity<double> q = ((QsScalar)val.Value).Quantity;

                    if (q.Dimension.IsDimensionless)
                    {
                        double r = System.Math.Sinh(q.Value);
                        return r.ToQuantity().ToScalarValue();
                    }
                    else
                    {
                        throw new QsInvalidInputException("Non dimensionless number");
                    }
                }
                else if (val.Value is QsVector)
                {
                    QsVector vec = (QsVector)val.Value;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar var in vec)
                    {
                        if (var.Quantity.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Sinh(var.Quantity.Value);
                            rv.AddComponent(r.ToQuantity().ToScalar());
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless component");
                        }
                    }

                    return rv;
                }
                else if (val.Value is QsMatrix)
                {
                    QsMatrix mat = (QsMatrix)val.Value;
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
            if (val.IsKnown)
            {
                if (val.Value is QsScalar)
                {
                    AnyQuantity<double> q = ((QsScalar)val.Value).Quantity;

                    if (q.Dimension.IsDimensionless)
                    {
                        double r = System.Math.Cosh(q.Value);
                        return r.ToQuantity().ToScalarValue();
                    }
                    else
                    {
                        throw new QsInvalidInputException("Non dimensionless number");
                    }
                }
                else if (val.Value is QsVector)
                {
                    QsVector vec = (QsVector)val.Value;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar var in vec)
                    {
                        if (var.Quantity.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Cosh(var.Quantity.Value);
                            rv.AddComponent(r.ToQuantity().ToScalar());
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless component");
                        }
                    }

                    return rv;
                }
                else if (val.Value is QsMatrix)
                {
                    QsMatrix mat = (QsMatrix)val.Value;
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
            if (val.IsKnown)
            {
                if (val.Value is QsScalar)
                {
                    AnyQuantity<double> q = ((QsScalar)val.Value).Quantity;

                    if (q.Dimension.IsDimensionless)
                    {
                        double r = System.Math.Sin(q.Value);
                        return r.ToQuantity().ToScalarValue();
                    }
                    else
                    {
                        throw new QsInvalidInputException("Non dimensionless number");
                    }
                }
                else if (val.Value is QsVector)
                {
                    QsVector vec = (QsVector)val.Value;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar var in vec)
                    {
                        if (var.Quantity.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Sin(var.Quantity.Value);
                            rv.AddComponent(r.ToQuantity().ToScalar());
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless component");
                        }
                    }

                    return rv;
                }
                else if (val.Value is QsMatrix)
                {
                    QsMatrix mat = (QsMatrix)val.Value;
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
            if (val.IsKnown)
            {
                if (val.Value is QsScalar)
                {
                    AnyQuantity<double> q = ((QsScalar)val.Value).Quantity;

                    if (q.Dimension.IsDimensionless)
                    {
                        double r = System.Math.Cos(q.Value);
                        return r.ToQuantity().ToScalarValue();
                    }
                    else
                    {
                        throw new QsInvalidInputException("Non dimensionless number");
                    }
                }
                else if (val.Value is QsVector)
                {
                    QsVector vec = (QsVector)val.Value;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar var in vec)
                    {
                        if (var.Quantity.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Cos(var.Quantity.Value);
                            rv.AddComponent(r.ToQuantity().ToScalar());
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless component");
                        }
                    }

                    return rv;
                }
                else if (val.Value is QsMatrix)
                {
                    QsMatrix mat = (QsMatrix)val.Value;
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
                return Math.PI.ToQuantity().ToScalarValue();
            }
        }
    }
}
