// 
// This is an auto generated math class for Quantity system that hold the root functions
//  sin, cos , etc..
// but with ability to consume scalar vectors matrices, and later tensors also
//

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
using Qs.Numerics;

namespace QsRoot
{
    public static partial class QsMath
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
	    public static QsValue Sin(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Sin(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

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
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
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
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
                        if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Sin(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Sin((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Sin((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                    throw new QsException("This type is not supported for Sin function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Sin function");
            }
        }
		
	    public static QsValue Cos(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Cos(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

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
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
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
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
                        if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Cos(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Cos((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Cos((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                    throw new QsException("This type is not supported for Cos function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Cos function");
            }
        }
		
	    public static QsValue Tan(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Tan(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Tan((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Tan((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
                        if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Tan(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Tan((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Tan((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Tan(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Tan function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Tan function");
            }
        }
		
	    public static QsValue Sinh(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Sinh(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

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
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
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
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
                        if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Sinh(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Sinh((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Sinh((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                    throw new QsException("This type is not supported for Sinh function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Sinh function");
            }
        }
		
	    public static QsValue Cosh(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Cosh(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

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
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
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
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
                        if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Cosh(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Cosh((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Cosh((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                    throw new QsException("This type is not supported for Cosh function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Cosh function");
            }
        }
		
	    public static QsValue Tanh(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Tanh(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Tanh((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Tanh((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
                        if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Tanh(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Tanh((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Tanh((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Tanh(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Tanh function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Tanh function");
            }
        }
		
	    public static QsValue Acos(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Acos(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Acos((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Acos((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
                        if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Acos(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Acos((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Acos((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Acos(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Acos function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Acos function");
            }
        }
		
	    public static QsValue Asin(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Asin(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Asin((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Asin((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
                        if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Asin(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Asin((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Asin((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Asin(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Asin function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Asin function");
            }
        }
		
	    public static QsValue Atan(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Atan(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Atan((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Atan((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
                        if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Atan(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Atan((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Atan((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Atan(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Atan function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Atan function");
            }
        }
		
	    public static QsValue Log(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Log(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Log((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Log((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
                        if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Log(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Log((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Log((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                    throw new QsException("This type is not supported for Log function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Log function");
            }
        }
		
	    public static QsValue Log10(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Log10(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Log10((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Log10((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
                        if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Log10(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Log10((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Log10((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                    throw new QsException("This type is not supported for Log10 function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Log10 function");
            }
        }
		
	    public static QsValue Floor(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Floor(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Floor((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Floor((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
                        if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Floor(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Floor((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Floor((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                    throw new QsException("This type is not supported for Floor function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Floor function");
            }
        }
		
	    public static QsValue Ceiling(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Ceiling(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Ceiling((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Ceiling((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
                        if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Ceiling(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Ceiling((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Ceiling((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                    throw new QsException("This type is not supported for Ceiling function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Ceiling function");
            }
        }
		
	    public static QsValue Exp(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Exp(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Exp((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = System.Math.Exp((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
                        if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Exp(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Exp((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Math.Exp((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                    throw new QsException("This type is not supported for Exp function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Exp function");
            }
        }
		
		#endregion

		#region Rest of Functions in CoMath class
		
		public static QsValue Sec(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Sec(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Sec((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Sec((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
						if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Sec(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Sec((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Sec((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Sec(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Sec function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Sec function");
            }
        }
		
		
		public static QsValue Csc(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Csc(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Csc((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Csc((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
						if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Csc(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Csc((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Csc((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Csc(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Csc function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Csc function");
            }
        }
		
		
		public static QsValue Cot(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Cot(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Cot((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Cot((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
						if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Cot(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Cot((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Cot((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Cot(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Cot function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Cot function");
            }
        }
		
		
		public static QsValue Sech(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Sech(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Sech((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Sech((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
						if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Sech(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Sech((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Sech((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Sech(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Sech function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Sech function");
            }
        }
		
		
		public static QsValue Csch(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Csch(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Csch((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Csch((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
						if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Csch(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Csch((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Csch((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Csch(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Csch function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Csch function");
            }
        }
		
		
		public static QsValue Coth(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Coth(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Coth((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Coth((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
						if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Coth(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Coth((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Coth((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Coth(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Coth function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Coth function");
            }
        }
		
		
		public static QsValue Acosh(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Acosh(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Acosh((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Acosh((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
						if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Acosh(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Acosh((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Acosh((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Acosh(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Acosh function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Acosh function");
            }
        }
		
		
		public static QsValue Asinh(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Asinh(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Asinh((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Asinh((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
						if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Asinh(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Asinh((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Asinh((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Asinh(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Asinh function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Asinh function");
            }
        }
		
		
		public static QsValue Atanh(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Atanh(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Atanh((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Atanh((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
						if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Atanh(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Atanh((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Atanh((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Atanh(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Atanh function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Atanh function");
            }
        }
		
		
		public static QsValue Asec(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Asec(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Asec((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Asec((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
						if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Asec(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Asec((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Asec((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Asec(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Asec function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Asec function");
            }
        }
		
		
		public static QsValue Acsc(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Acsc(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Acsc((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Acsc((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
						if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Acsc(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Acsc((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Acsc((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Acsc(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Acsc function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Acsc function");
            }
        }
		
		
		public static QsValue Acot(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Acot(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Acot((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Acot((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
						if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Acot(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Acot((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Acot((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Acot(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Acot function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Acot function");
            }
        }
		
		
		public static QsValue Asech(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Asech(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Asech((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Asech((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
						if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Asech(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Asech((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Asech((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Asech(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Asech function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Asech function");
            }
        }
		
		
		public static QsValue Acsch(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Acsch(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Acsch((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Acsch((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
						if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Acsch(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Acsch((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Acsch((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Acsch(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Acsch function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Acsch function");
            }
        }
		
		
		public static QsValue Acoth(QsParameter val)
        {
            if (val.IsQsValue)
            {
                if (val.ParameterValue is QsScalar)
                {
                    QsScalar pval = (QsScalar)val.ParameterValue;
                    if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
                    {
                        return SymbolicAlgebra.SymbolicVariable.Parse("Acoth(" + pval.SymbolicQuantity.Value.ToString() + ")").ToQuantity().ToScalar();
                    }
                    else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
                    {
                        AnyQuantity<double> q = pval.NumericalQuantity;

                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Acoth((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
                    {
                        AnyQuantity<Rational> rr = pval.RationalQuantity;
                        AnyQuantity<double> q = rr.Value.Value.ToQuantity();
                        if (q.Dimension.IsDimensionless)
                        {
                            double r = Qs.CoMath.Acoth((ZeroAngle + q).Value);
                            return r.ToQuantity().ToScalarValue();
                        }
                        else
                        {
                            throw new QsInvalidInputException("Non dimensionless number");
                        }
                    }
                    else
                    {
                        throw new QsException("Not Supported Scalar Type");
                    }
				}
                else if (val.ParameterValue is QsVector)
                {
                    QsVector vec = (QsVector)val.ParameterValue;

                    QsVector rv = new QsVector(vec.Count);

                    foreach (QsScalar pval in vec)
                    {
						if(pval.ScalarType == ScalarTypes.SymbolicQuantity)
						{
							var r= SymbolicAlgebra.SymbolicVariable.Parse("Acoth(" + pval.SymbolicQuantity.Value.ToString() + ")");
							rv.AddComponent(r.ToQuantity().ToScalar());
						}
						else if (pval.ScalarType == ScalarTypes.NumericalQuantity)
						{
							AnyQuantity<double> q = pval.NumericalQuantity;

							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Acoth((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else if(pval.ScalarType == ScalarTypes.RationalNumberQuantity)
						{
							AnyQuantity<Rational> rr = pval.RationalQuantity;
							AnyQuantity<double> q = rr.Value.Value.ToQuantity();
							if (q.Dimension.IsDimensionless)
							{
								double r = Qs.CoMath.Acoth((ZeroAngle + q).Value);
								rv.AddComponent(r.ToQuantity().ToScalar());
							}
							else
							{
								throw new QsInvalidInputException("Non dimensionless number");
							}
						}
						else
						{
							throw new QsException("Not Supported Scalar Type");
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
                        rm.AddVector((QsVector)Acoth(QsParameter.MakeParameter(vec, string.Empty)));

                    }
                    return rm;
                }
                else
                {
                    throw new QsException("This type is not supported for Acoth function");
                }
            }
            else
            {
                
                throw new QsException("This type is not supported for Acoth function");
            }
        }
		
	#endregion
	}
}
