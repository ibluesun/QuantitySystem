using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using Qs.Runtime;
using SymbolicAlgebra;
using System.Globalization;
using QuantitySystem.Units;
using Qs.Numerics;


namespace Qs.Types
{
    /// <summary>
    /// Wrapper for AnyQuantit&lt;double&gt; and it serve the basic number in the Qs
    /// </summary>
    public sealed class QsScalar : QsValue, IConvertible
    {

        #region Scalar Types Storage

        /// <summary>
        /// Tells the current storage type of the scalar.
        /// </summary>
        private readonly ScalarTypes _ScalarType;

        public ScalarTypes ScalarType
        {
            get { return _ScalarType; }
        } 

        /// <summary>
        /// Quantity that its storage is symbol.
        /// </summary>
        public AnyQuantity<SymbolicVariable> SymbolicQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// Double Number Quantity 
        /// Default behaviour.
        /// </summary>
        public AnyQuantity<double> NumericalQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// Quantity that its storage is a complex number
        /// </summary>
        public AnyQuantity<Complex> ComplexQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// Quantity that its storage is a Quaternion number
        /// </summary>
        public AnyQuantity<Quaternion> QuaternionQuantity
        {
            get;
            set;
        }


        /// <summary>
        /// Quantity that its storage is a Function.
        /// </summary>
        public AnyQuantity<QsFunction> FunctionQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// Hold qs operations like @ and \/ operations
        /// </summary>
        public QsOperation Operation
        {
            get;
            set;
        }

        /// <summary>
        /// Rational Number Quantity.
        /// </summary>
        public AnyQuantity<Rational> RationalQuantity
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Return the current scalar unit.
        /// </summary>
        public Unit Unit
        {
            get
            {
                switch (_ScalarType)
                {
                    case ScalarTypes.NumericalQuantity:
                        return NumericalQuantity.Unit;
                    case ScalarTypes.SymbolicQuantity:
                        return SymbolicQuantity.Unit;
                    case ScalarTypes.ComplexNumberQuantity:
                        return ComplexQuantity.Unit;
                    case ScalarTypes.QuaternionNumberQuantity:
                        return QuaternionQuantity.Unit;
                    case ScalarTypes.FunctionQuantity:
                        return FunctionQuantity.Unit;
                    case ScalarTypes.QsOperation:
                        return new Unit(typeof(QuantitySystem.Quantities.DimensionlessQuantities.DimensionlessQuantity<>));
                    case ScalarTypes.RationalNumberQuantity:
                        return RationalQuantity.Unit;
                    default:
                        throw new NotImplementedException("Not implemented for " + _ScalarType.ToString());
                }
            }
        }

        /// <summary>
        /// Try to convert the current quantity into symbolic quantity.
        /// </summary>
        /// <returns></returns>
        public AnyQuantity<SymbolicVariable> ToSymbolicQuantity()
        {
            switch(_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    AnyQuantity<SymbolicVariable> sv =
                        this.NumericalQuantity.Unit.GetThisUnitQuantity<SymbolicVariable>(
                        new SymbolicVariable(this.NumericalQuantity.Value.ToString(CultureInfo.InvariantCulture)));
                    return sv;
                case ScalarTypes.RationalNumberQuantity:
                    return 
                        this.RationalQuantity.Unit.GetThisUnitQuantity<SymbolicVariable>(
                        new SymbolicVariable(this.RationalQuantity.Value.Value.ToString(CultureInfo.InvariantCulture)));
                case ScalarTypes.SymbolicQuantity:
                    return (AnyQuantity<SymbolicVariable>)SymbolicQuantity.Clone();
                default:
                    throw new NotImplementedException();
            }
        }

        public QsScalar()
        {
            _ScalarType = ScalarTypes.NumericalQuantity;
        }

        public QsScalar(ScalarTypes scalarType)
        {
            _ScalarType = scalarType;
        }

        public override string ToString()
        {
            string scalar = string.Empty;
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    scalar = NumericalQuantity.ToString();
                    break;
                case ScalarTypes.ComplexNumberQuantity:
                    scalar = ComplexQuantity.ToString();
                    break;
                case ScalarTypes.QuaternionNumberQuantity:
                    scalar = QuaternionQuantity.ToString();
                    break;
                case ScalarTypes.SymbolicQuantity:
                    scalar = SymbolicQuantity.ToString();
                    break;
                case ScalarTypes.FunctionQuantity:
                    string plist = string.Join(", ", FunctionQuantity.Value.ParametersNames);
                    scalar = FunctionQuantity.ToString();
                    break;
                case ScalarTypes.QsOperation:
                    scalar = Operation.ToString();
                    break;
                case ScalarTypes.RationalNumberQuantity:
                    scalar = RationalQuantity.ToString();
                    break;
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " Operation not implemented yet");
            }

            return scalar;
        }

        /// <summary>
        /// This gives the inner value string representation of this scalar without any unit and in parer way.
        /// </summary>
        /// <returns></returns>
        public string ToParsableValuedString()
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    return NumericalQuantity.Value.ToString(CultureInfo.InvariantCulture);
                case ScalarTypes.ComplexNumberQuantity:
                    return ComplexQuantity.Value.ToQsSyntax();
                case ScalarTypes.QuaternionNumberQuantity:
                    return QuaternionQuantity.Value.ToQsSyntax();
                case ScalarTypes.SymbolicQuantity:
                    return SymbolicQuantity.Value.ToString();
                case ScalarTypes.FunctionQuantity:
                    return FunctionQuantity.Value.ToSymbolicVariable().ToString();
                case ScalarTypes.QsOperation:
                    return Operation.ToShortString();
                case ScalarTypes.RationalNumberQuantity:
                    return RationalQuantity.Value.ToQsSyntax();
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " Operation not implemented yet");
            }
        }

        public override string ToShortString()
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    return NumericalQuantity.ToShortString();
                case ScalarTypes.ComplexNumberQuantity:
                    return ComplexQuantity.ToShortString();
                case ScalarTypes.QuaternionNumberQuantity:
                    return QuaternionQuantity.ToShortString();
                case ScalarTypes.SymbolicQuantity:
                    return SymbolicQuantity.ToShortString();
                case ScalarTypes.FunctionQuantity:
                    return FunctionQuantity.Value.ToShortString();
                case ScalarTypes.QsOperation:
                    return Operation.ToShortString();
                case ScalarTypes.RationalNumberQuantity:
                   return RationalQuantity.ToShortString();
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " Operation not implemented yet");
            }
        }

        /// <summary>
        /// Text that is able to be parsed.
        /// </summary>
        /// <returns></returns>
        public string ToExpressionParsableString()
        {
            string rv = string.Empty;

            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    rv =  NumericalQuantity.ToShortString();
                    break;

                case ScalarTypes.ComplexNumberQuantity:  // return C{Real, Imaginary}
                    rv =  "C{" + ComplexQuantity.Value.Real.ToString(CultureInfo.InvariantCulture) + ", " 
                        + ComplexQuantity.Value.Imaginary.ToString(CultureInfo.InvariantCulture) + "}" 
                        + this.ComplexQuantity.UnitText;
                    break;

                case ScalarTypes.QuaternionNumberQuantity: // return H{a, b, c, d}
                    rv =  "H{" + QuaternionQuantity.Value.Real.ToString(CultureInfo.InvariantCulture) + ", "
                        + QuaternionQuantity.Value.i.ToString(CultureInfo.InvariantCulture) + ", "
                        + QuaternionQuantity.Value.j.ToString(CultureInfo.InvariantCulture) + ", "
                        + QuaternionQuantity.Value.k.ToString(CultureInfo.InvariantCulture) + "}"
                        + this.QuaternionQuantity.UnitText;
                    break;

                case ScalarTypes.SymbolicQuantity: // return symbolic quantity into parsable format 
                    //add $ before any symbol
                    string sq = SymbolicQuantity.Value.ToString();
                    foreach (string sym in SymbolicQuantity.Value.InvolvedSymbols)
                        sq = sq.Replace(sym, "$" + sym);
                    rv =  sq + SymbolicQuantity.UnitText;
                    break;

                case ScalarTypes.FunctionQuantity:    // return the body of the function
                    rv = FunctionQuantity.Value.SymbolicBodyText;
                    break;
                case ScalarTypes.RationalNumberQuantity:  // return Q{a, b}
                    rv = "Q{" + this.RationalQuantity.Value.num.ToString(CultureInfo.InvariantCulture) + ", "
                        + this.RationalQuantity.Value.den.ToString(CultureInfo.InvariantCulture) + "}"
                        + this.RationalQuantity.UnitText;
                    break;
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " ToExpression String is not implemented yet");
            }

            if (rv.EndsWith("<1>")) return rv.Substring(0, rv.Length - 3);
            else return rv;

        }

        /// <summary>
        /// Gets the value hosted in this scalar as text.
        /// </summary>
        /// <returns></returns>
        public override string ToValueString()
        {
            if (_ScalarType == ScalarTypes.NumericalQuantity) return NumericalQuantity.Value.ToString(CultureInfo.InvariantCulture);

            if (_ScalarType == ScalarTypes.SymbolicQuantity) return SymbolicQuantity.Value.ToString();

            if (_ScalarType == ScalarTypes.ComplexNumberQuantity) return ComplexQuantity.Value.ToString();

            if (_ScalarType == ScalarTypes.QuaternionNumberQuantity) return QuaternionQuantity.Value.ToString();

            if (_ScalarType == ScalarTypes.FunctionQuantity) return FunctionQuantity.Value.ToShortString();

            if (_ScalarType == ScalarTypes.QsOperation) return Operation.ToShortString();

            if (_ScalarType == ScalarTypes.RationalNumberQuantity) return RationalQuantity.Value.ToString();

            throw new NotImplementedException("Unknow scalar type: " + _ScalarType.ToString());
        }

        #region Operations

        #region Scalar Operations
        /// <summary>
        /// Add the passed scalar to the current scalar.
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public QsScalar AddScalar(QsScalar scalar)
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:  // lhs := number
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar { NumericalQuantity = this.NumericalQuantity + scalar.NumericalQuantity };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.ToSymbolicQuantity() + scalar.SymbolicQuantity };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = this.NumericalQuantity.ToComplex() + scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.NumericalQuantity.ToQuaternion() + scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar { NumericalQuantity = this.NumericalQuantity + scalar.RationalQuantity.Unit.GetThisUnitQuantity<double>(scalar.RationalQuantity.Value.Value) };
                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " + " + scalar.ScalarType.ToString());
                    }
                case ScalarTypes.SymbolicQuantity: // lhs := symbolic variable
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity + scalar.ToSymbolicQuantity() };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity + scalar.SymbolicQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity + scalar.ToSymbolicQuantity() };
                        case ScalarTypes.FunctionQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity + scalar.FunctionQuantity.Value.ToSymbolicQuantity() };
                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " + " + scalar.ScalarType.ToString());
                    }
                case ScalarTypes.ComplexNumberQuantity: // lhs := complex
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = this.ComplexQuantity + scalar.NumericalQuantity.ToComplex() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = this.ComplexQuantity + scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.ComplexQuantity.ToQuaternion() + scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            {
                                AnyQuantity<Complex> result = null;
                                result = scalar.RationalQuantity.Unit.GetThisUnitQuantity<Complex>(new Complex((double)scalar.RationalQuantity.Value.Value, 0.0));
                                result = this.ComplexQuantity + result;
                                return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " + " + scalar.ScalarType.ToString());
                    }
                case ScalarTypes.QuaternionNumberQuantity: // lhs := quanternion
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.QuaternionQuantity + scalar.NumericalQuantity.ToQuaternion() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.QuaternionQuantity + scalar.ComplexQuantity.ToQuaternion() };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.QuaternionQuantity + scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            {
                                AnyQuantity<Quaternion> result = null;
                                result = scalar.RationalQuantity.Unit.GetThisUnitQuantity<Quaternion>(new Quaternion((double)scalar.RationalQuantity.Value.Value, 0.0, 0, 0));
                                result = this.QuaternionQuantity + result;
                                return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " + " + scalar.ScalarType.ToString());
                    }
                case ScalarTypes.FunctionQuantity:  // lhs := function
                    {
                        return ((QsFunction)FunctionQuantity.Value.AddOperation(scalar)).ToQuantity().ToScalar();
                    }
                case ScalarTypes.RationalNumberQuantity: // lhs := Rational
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) { RationalQuantity = this.RationalQuantity + scalar.RationalQuantity };
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) { RationalQuantity = this.RationalQuantity + scalar.NumericalQuantity.ToRational() };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity =this.ToSymbolicQuantity() + scalar.SymbolicQuantity };
                       case ScalarTypes.ComplexNumberQuantity:
                            {
                                AnyQuantity<Complex> result = null;
                                result = this.RationalQuantity.Unit.GetThisUnitQuantity<Complex>(new Complex((double)this.RationalQuantity.Value.Value, 0.0));
                                result = result + scalar.ComplexQuantity;
                                return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = result };
                            }
                        case ScalarTypes.QuaternionNumberQuantity:
                            {
                                AnyQuantity<Quaternion> result = null;
                                result = this.RationalQuantity.Unit.GetThisUnitQuantity<Quaternion>(new Quaternion((double)this.RationalQuantity.Value.Value, 0.0, 0, 0));
                                result = result + scalar.QuaternionQuantity;
                                return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " + " + scalar.ScalarType.ToString());
                    }
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " + " + scalar.ScalarType.ToString());
            }
        }

        public QsScalar SubtractScalar(QsScalar scalar)
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar { NumericalQuantity = this.NumericalQuantity - scalar.NumericalQuantity };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.ToSymbolicQuantity() - scalar.SymbolicQuantity };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = this.NumericalQuantity.ToComplex() - scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.NumericalQuantity.ToQuaternion() - scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar { NumericalQuantity = this.NumericalQuantity - scalar.RationalQuantity.Unit.GetThisUnitQuantity<double>(scalar.RationalQuantity.Value.Value) };
                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " + " + scalar.ScalarType.ToString());
                    }
                case ScalarTypes.SymbolicQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity - scalar.ToSymbolicQuantity() };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity - scalar.SymbolicQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity - scalar.ToSymbolicQuantity() };
                        case ScalarTypes.FunctionQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity - scalar.FunctionQuantity.Value.ToSymbolicQuantity() };

                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " + " + scalar.ScalarType.ToString());
                    }
                case ScalarTypes.ComplexNumberQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = this.ComplexQuantity - scalar.NumericalQuantity.ToComplex() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = this.ComplexQuantity - scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.ComplexQuantity.ToQuaternion() - scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            {
                                AnyQuantity<Complex> result = null;
                                result = scalar.RationalQuantity.Unit.GetThisUnitQuantity<Complex>(new Complex((double)scalar.RationalQuantity.Value.Value, 0.0));
                                result = this.ComplexQuantity - result;
                                return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " + " + scalar.ScalarType.ToString());
                    }
                case ScalarTypes.QuaternionNumberQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.QuaternionQuantity - scalar.NumericalQuantity.ToQuaternion() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.QuaternionQuantity - scalar.ComplexQuantity.ToQuaternion() };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.QuaternionQuantity - scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            {
                                AnyQuantity<Quaternion> result = null;
                                result = scalar.RationalQuantity.Unit.GetThisUnitQuantity<Quaternion>(new Quaternion((double)scalar.RationalQuantity.Value.Value, 0.0, 0, 0));
                                result = this.QuaternionQuantity - result;
                                return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " + " + scalar.ScalarType.ToString());
                    }
                case ScalarTypes.FunctionQuantity:
                    {
                        return ((QsFunction)FunctionQuantity.Value.SubtractOperation(scalar)).ToQuantity().ToScalar();
                    }
                case ScalarTypes.RationalNumberQuantity: // lhs := Rational
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) { RationalQuantity = this.RationalQuantity - scalar.RationalQuantity };
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) { RationalQuantity = this.RationalQuantity - scalar.NumericalQuantity.ToRational() };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.ToSymbolicQuantity() - scalar.SymbolicQuantity };
                        case ScalarTypes.ComplexNumberQuantity:
                            {
                                AnyQuantity<Complex> result = null;
                                result = this.RationalQuantity.Unit.GetThisUnitQuantity<Complex>(new Complex((double)this.RationalQuantity.Value.Value, 0.0));
                                result = result - scalar.ComplexQuantity;
                                return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = result };
                            }
                        case ScalarTypes.QuaternionNumberQuantity:
                            {
                                AnyQuantity<Quaternion> result = null;
                                result = this.RationalQuantity.Unit.GetThisUnitQuantity<Quaternion>(new Quaternion((double)this.RationalQuantity.Value.Value, 0.0, 0, 0));
                                result = result - scalar.QuaternionQuantity;
                                return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = result };
                            }

                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " + " + scalar.ScalarType.ToString());
                    }
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " - " + scalar.ScalarType.ToString());
            }

        }

        /// <summary>
        /// Multiply the passed scalar to the current scalar instance.
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public QsScalar MultiplyScalar(QsScalar scalar)
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar { NumericalQuantity = this.NumericalQuantity * scalar.NumericalQuantity };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.ToSymbolicQuantity() * scalar.SymbolicQuantity };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = this.NumericalQuantity.ToComplex() * scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.NumericalQuantity.ToQuaternion() * scalar.QuaternionQuantity };
                        case ScalarTypes.FunctionQuantity:
                            return ((QsFunction)scalar.FunctionQuantity.Value.MultiplyOperation(this)).ToQuantity().ToScalar();
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar { NumericalQuantity = this.NumericalQuantity * scalar.RationalQuantity.Unit.GetThisUnitQuantity<double>(scalar.RationalQuantity.Value.Value) };

                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " * " + scalar.ScalarType.ToString());
                    }
                case ScalarTypes.SymbolicQuantity:
                    {
                        switch (scalar.ScalarType)
                        {
                            case ScalarTypes.NumericalQuantity:
                                return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity * scalar.ToSymbolicQuantity() };
                            case ScalarTypes.SymbolicQuantity:
                                return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity * scalar.SymbolicQuantity };
                            case ScalarTypes.RationalNumberQuantity:
                                return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity * scalar.ToSymbolicQuantity() };
                            case ScalarTypes.FunctionQuantity:
                                return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity * scalar.FunctionQuantity.Value.ToSymbolicQuantity() };
                            default:
                                throw new NotImplementedException(_ScalarType.ToString() + " * " + scalar.ScalarType.ToString());
                        }
                    }
                case ScalarTypes.ComplexNumberQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = this.ComplexQuantity * scalar.NumericalQuantity.ToComplex() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = this.ComplexQuantity * scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.ComplexQuantity.ToQuaternion() * scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            {
                                AnyQuantity<Complex> result = null;
                                result = scalar.RationalQuantity.Unit.GetThisUnitQuantity<Complex>(new Complex((double)scalar.RationalQuantity.Value.Value, 0.0));
                                result = this.ComplexQuantity * result;
                                return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " * " + scalar.ScalarType.ToString());
                    }
                case ScalarTypes.QuaternionNumberQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.QuaternionQuantity * scalar.NumericalQuantity.ToQuaternion() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.QuaternionQuantity * scalar.ComplexQuantity.ToQuaternion() };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.QuaternionQuantity * scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            {
                                AnyQuantity<Quaternion> result = null;
                                result = scalar.RationalQuantity.Unit.GetThisUnitQuantity<Quaternion>(new Quaternion((double)scalar.RationalQuantity.Value.Value, 0.0, 0, 0));
                                result = this.QuaternionQuantity * result;
                                return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " * " + scalar.ScalarType.ToString());
                    }
                case ScalarTypes.FunctionQuantity:
                    {
                        switch (scalar.ScalarType)
                        {
                            case ScalarTypes.QsOperation:
                                return (QsScalar)scalar.Operation.MultiplyOperation(this);
                            default:
                                return ((QsFunction)FunctionQuantity.Value.MultiplyOperation(scalar)).ToQuantity().ToScalar();
                        }
                    }
                case ScalarTypes.RationalNumberQuantity: // lhs := Rational
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) { RationalQuantity = this.RationalQuantity * scalar.RationalQuantity };
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) { RationalQuantity = this.RationalQuantity * scalar.NumericalQuantity.ToRational() };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.ToSymbolicQuantity() * scalar.SymbolicQuantity };
                        case ScalarTypes.ComplexNumberQuantity:
                            {
                                AnyQuantity<Complex> result = null;
                                result = this.RationalQuantity.Unit.GetThisUnitQuantity<Complex>(new Complex((double)this.RationalQuantity.Value.Value, 0.0));
                                result = result * scalar.ComplexQuantity;
                                return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = result };
                            }
                        case ScalarTypes.QuaternionNumberQuantity:
                            {
                                AnyQuantity<Quaternion> result = null;
                                result = this.RationalQuantity.Unit.GetThisUnitQuantity<Quaternion>(new Quaternion((double)this.RationalQuantity.Value.Value, 0.0, 0, 0));
                                result = result * scalar.QuaternionQuantity;
                                return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = result };
                            }

                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " * " + scalar.ScalarType.ToString());
                    }
                case ScalarTypes.QsOperation:
                    return (QsScalar)this.Operation.MultiplyOperation(scalar);
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " * " + scalar.ScalarType.ToString());
            }

        }

        public QsScalar DivideScalar(QsScalar scalar)
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar { NumericalQuantity = this.NumericalQuantity / scalar.NumericalQuantity };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.ToSymbolicQuantity() / scalar.SymbolicQuantity };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = this.NumericalQuantity.ToComplex() / scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.NumericalQuantity.ToQuaternion() / scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar { NumericalQuantity = this.NumericalQuantity / scalar.RationalQuantity.Unit.GetThisUnitQuantity<double>(scalar.RationalQuantity.Value.Value) };
                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " / " + scalar.ScalarType.ToString());
                    }
                case ScalarTypes.SymbolicQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity / scalar.ToSymbolicQuantity() };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity / scalar.SymbolicQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity / scalar.ToSymbolicQuantity() };
                        case ScalarTypes.FunctionQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity / scalar.FunctionQuantity.Value.ToSymbolicQuantity() };
                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " / " + scalar.ScalarType.ToString());
                    }
                case ScalarTypes.ComplexNumberQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = this.ComplexQuantity / scalar.NumericalQuantity.ToComplex() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = this.ComplexQuantity / scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.ComplexQuantity.ToQuaternion() / scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            {
                                AnyQuantity<Complex> result = null;
                                result = scalar.RationalQuantity.Unit.GetThisUnitQuantity<Complex>(new Complex((double)scalar.RationalQuantity.Value.Value, 0.0));
                                result = this.ComplexQuantity / result;
                                return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " / " + scalar.ScalarType.ToString());
                    }
                case ScalarTypes.QuaternionNumberQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.QuaternionQuantity / scalar.NumericalQuantity.ToQuaternion() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.QuaternionQuantity / scalar.ComplexQuantity.ToQuaternion() };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.QuaternionQuantity / scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            {
                                AnyQuantity<Quaternion> result = null;
                                result = scalar.RationalQuantity.Unit.GetThisUnitQuantity<Quaternion>(new Quaternion((double)scalar.RationalQuantity.Value.Value, 0.0, 0, 0));
                                result = this.QuaternionQuantity / result;
                                return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " / " + scalar.ScalarType.ToString());
                    }
                case ScalarTypes.FunctionQuantity:
                    {
                        return ((QsFunction)FunctionQuantity.Value.DivideOperation(scalar)).ToQuantity().ToScalar();
                    }
                case ScalarTypes.RationalNumberQuantity: // lhs := Rational
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) { RationalQuantity = this.RationalQuantity / scalar.RationalQuantity };
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) { 
                                RationalQuantity = this.RationalQuantity / scalar.NumericalQuantity.ToRational()
                            };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.ToSymbolicQuantity() / scalar.SymbolicQuantity };
                        case ScalarTypes.ComplexNumberQuantity:
                            {
                                AnyQuantity<Complex> result = null;
                                result = this.RationalQuantity.Unit.GetThisUnitQuantity<Complex>(new Complex((double)this.RationalQuantity.Value.Value, 0.0));
                                result = result / scalar.ComplexQuantity;
                                return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = result };
                            }
                        case ScalarTypes.QuaternionNumberQuantity:
                            {
                                AnyQuantity<Quaternion> result = null;
                                result = this.RationalQuantity.Unit.GetThisUnitQuantity<Quaternion>(new Quaternion((double)this.RationalQuantity.Value.Value, 0.0, 0, 0));
                                result = result / scalar.QuaternionQuantity;
                                return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = result };
                            }

                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " / " + scalar.ScalarType.ToString());
                    }

                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " / " + scalar.ScalarType.ToString());
            }

        }

        public QsScalar PowerScalar(QsScalar power)
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    {
                        switch (power.ScalarType)
                        {
                            case ScalarTypes.NumericalQuantity:
                                {
                                    if (this.NumericalQuantity.Value < 0.0 && power.NumericalQuantity.Value == 0.5)
                                    {
                                        var av = Math.Sqrt(Math.Abs(this.NumericalQuantity.Value));
                                        return new Complex(0, av).ToQuantity().ToScalar();
                                    }
                                    else
                                    {
                                        return new QsScalar { NumericalQuantity = AnyQuantity<double>.Power(this.NumericalQuantity, power.NumericalQuantity) };
                                    }
                                }
                            case ScalarTypes.SymbolicQuantity:
                                {
                                    if (this.NumericalQuantity.Dimension.IsDimensionless)
                                    {
                                        SymbolicVariable sv = new SymbolicVariable(this.NumericalQuantity.Value.ToString(CultureInfo.InvariantCulture));
                                        return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = AnyQuantity<SymbolicVariable>.Power(sv.ToQuantity(), power.SymbolicQuantity) };
                                    }
                                    else
                                    {
                                        throw new QsException("Raising none dimensionless quantity to symbolic quantity is not supported");
                                    }
                                }
                            case ScalarTypes.RationalNumberQuantity:
                                return new QsScalar { NumericalQuantity = AnyQuantity<double>.Power(this.NumericalQuantity, power.RationalQuantity.Unit.GetThisUnitQuantity<double>(power.RationalQuantity.Value.Value)) }; 
                            default:
                                throw new NotImplementedException(_ScalarType.ToString() + " ^ " + power.ScalarType.ToString());
                        }

                    }
                case ScalarTypes.SymbolicQuantity:
                    {
                        switch (power.ScalarType)
                        {
                            case ScalarTypes.NumericalQuantity:
                            {
                                double dpower = power.NumericalQuantity.Value;
                                QsScalar nsq = new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = AnyQuantity<SymbolicVariable>.Power(this.SymbolicQuantity, power.NumericalQuantity) };

                                return nsq;                                
                            }
                            case ScalarTypes.SymbolicQuantity:
                            {
                                if (power.SymbolicQuantity.Dimension.IsDimensionless && this.SymbolicQuantity.Dimension.IsDimensionless)
                                {
                                    // get the raised symbolic variable
                                    SymbolicVariable RaisedSymbolicVariable = SymbolicVariable.SymbolicPower(this.SymbolicQuantity.Value, power.SymbolicQuantity.Value);

                                    // make it into quantity
                                    AnyQuantity<SymbolicVariable> NewSymbolicQuantity = RaisedSymbolicVariable.ToQuantity();

                                    // assign into SymbolicQuantity property in new QsScalar object.

                                    QsScalar NewSymbolicQuantityScalar = NewSymbolicQuantity.ToScalar();

                                    return NewSymbolicQuantityScalar;

                                }
                                else
                                    throw new QsException("Raising Symbolic Quantity to Symbolic Quantity is only valid when the two quantities are dimensionless");
                            }
                            default:
                            throw new NotImplementedException("Raising Symbolic Quantity to " + power.ScalarType.ToString() + " is not implemented yet");
                        }
                        
                    }

                case ScalarTypes.ComplexNumberQuantity:
                    switch (power.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity)
                            {
                                ComplexQuantity = AnyQuantity<Complex>.Power(this.ComplexQuantity, power.NumericalQuantity)
                            };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity)
                            {
                                ComplexQuantity = AnyQuantity<Complex>.Power(this.ComplexQuantity, power.ComplexQuantity)
                            };
                        default:
                            throw new NotImplementedException("Raising Complex Quantity to " + power.ScalarType.ToString() + " is not implemented yet");

                    }

                case ScalarTypes.QuaternionNumberQuantity:
                    switch (power.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity)
                            {
                                QuaternionQuantity = AnyQuantity<Quaternion>.Power(this.QuaternionQuantity, power.NumericalQuantity)
                            };
                        default:
                            throw new NotImplementedException("Raising Quaternion Quantity to " + power.ScalarType.ToString() + " is not implemented yet");
                    }
                case ScalarTypes.FunctionQuantity:
                    {
                        return ((QsFunction)FunctionQuantity.Value.PowerOperation(power)).ToQuantity().ToScalar();
                    }
                case ScalarTypes.RationalNumberQuantity: // lhs := Rational
                    switch (power.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) 
                            { 
                                RationalQuantity = AnyQuantity<Rational>.Power(this.RationalQuantity,  power.NumericalQuantity)
                            };

                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " ^ " + power.ScalarType.ToString());
                    }

                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " ^ " + power.ScalarType.ToString());
            }
        }

        public QsScalar ModuloScalar(QsScalar scalar)
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar { NumericalQuantity = this.NumericalQuantity % scalar.NumericalQuantity };
                        default:
                            throw new NotImplementedException(_ScalarType.ToString() + " % " + scalar.ScalarType.ToString());
                    }
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " % " + scalar.ScalarType.ToString());
            }

        }

        public QsScalar DifferentiateScalar(QsScalar scalar)
        {
            switch (_ScalarType)
            {
                case ScalarTypes.FunctionQuantity:
                    return ((QsFunction)this.FunctionQuantity.Value.DifferentiateOperation(scalar)).ToQuantity().ToScalar();
                case ScalarTypes.SymbolicQuantity:
                        switch (scalar._ScalarType)
                        {
                            case ScalarTypes.SymbolicQuantity:
                                {
                                    var dsv = scalar.SymbolicQuantity.Value;
                                    SymbolicVariable nsv = this.SymbolicQuantity.Value;
                                    int times = (int)dsv.SymbolPower;
                                    while (times > 0)
                                    {
                                        nsv = nsv.Differentiate(dsv.Symbol);
                                        times--;
                                    }

                                    return nsv.ToQuantity().ToScalar();

                                }
                            default:
                                throw new NotImplementedException();
                        }

                case ScalarTypes.QsOperation:
                        {
                            var o = (QsScalar)this.Clone();
                            return (QsScalar)o.Operation.DifferentiateOperation(scalar);
                        }
                case ScalarTypes.NumericalQuantity:
                        {
                            return Zero;
                        }
                default:
                        throw new NotImplementedException(_ScalarType.ToString() + " | " + scalar.ScalarType.ToString());
            }
        
        }

        #endregion

        #region Vector Operations

        public QsVector AddVector(QsVector vector)
        {
            QsVector v = new QsVector(vector.Count);

            for (int i = 0; i < vector.Count; i++)
            {
                v.AddComponent(this + vector[i]);
            }

            return v;
        }

        public QsVector SubtractVector(QsVector vector)
        {
            QsVector v = new QsVector(vector.Count);

            for (int i = 0; i < vector.Count; i++)
            {
                v.AddComponent(this - vector[i]);
            }

            return v;
        }


        /// <summary>
        /// Multiply Scalar by Vector.
        /// </summary>
        /// <param name="q"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public QsVector MultiplyVector(QsVector vector)
        {
            

            QsVector v = new QsVector(vector.Count);

            for (int i = 0; i < vector.Count; i++)
            {
                v.AddComponent(this.MultiplyScalar(vector[i]));
            }

            return v;
        }



        #endregion

        #region Matrix Operations

        /// <summary>
        /// Scalar + Matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public QsMatrix AddMatrix(QsMatrix matrix)
        {
            QsMatrix Total = new QsMatrix();
            for (int IY = 0; IY < matrix.RowsCount; IY++)
            {
                List<QsScalar> row = new List<QsScalar>(matrix.ColumnsCount);

                for (int IX = 0; IX < matrix.ColumnsCount; IX++)
                {
                    row.Add(this + matrix[IY, IX]);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;
        }

        /// <summary>
        /// Scalar - Matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public QsMatrix SubtractMatrix(QsMatrix matrix)
        {
            QsMatrix Total = new QsMatrix();
            for (int IY = 0; IY < matrix.RowsCount; IY++)
            {
                List<QsScalar> row = new List<QsScalar>(matrix.ColumnsCount);

                for (int IX = 0; IX < matrix.ColumnsCount; IX++)
                {
                    row.Add(this - matrix[IY, IX]);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;
        }

        /// <summary>
        /// Scalar * Matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public QsMatrix MultiplyMatrix(QsMatrix matrix)
        {
            QsMatrix Total = new QsMatrix();
            for (int IY = 0; IY < matrix.RowsCount; IY++)
            {
                List<QsScalar> row = new List<QsScalar>(matrix.ColumnsCount);

                for (int IX = 0; IX < matrix.ColumnsCount; IX++)
                {
                    row.Add(this * matrix[IY, IX]);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;
        }


        #endregion

        #endregion


        #region operators redifintion for scalar explicitly
        public static QsScalar operator +(QsScalar a, QsScalar b)
        {
            return a.AddScalar(b);
        }

        public static QsScalar operator -(QsScalar a, QsScalar b)
        {
            return a.SubtractScalar(b);
        }

        public static QsScalar operator *(QsScalar a, QsScalar b)
        {
            return a.MultiplyScalar(b);
        }

        public static QsScalar operator /(QsScalar a, QsScalar b)
        {
            return a.DivideScalar(b);
        }

        public static QsScalar operator %(QsScalar a, QsScalar b)
        {
            return a.ModuloScalar(b);
        }
        #endregion


        #region Special Values
        private static QsScalar one = "1".ToScalar();
        private static QsScalar zero = "0".ToScalar();
        private static QsScalar minusOne = "-1".ToScalar();

        
        /// <summary>
        /// Returns -1 as dimensionless quantity scalar.
        /// </summary>
        public static QsScalar NegativeOne
        {
            get 
            { 
                return QsScalar.minusOne; 
            }
        }

        /// <summary>
        /// return 1 as dimensionless quantity scalar.
        /// </summary>
        public static QsScalar One
        {
            get
            {
                return one;
            }
        }


        /// <summary>
        /// Returns zero as dimensionless quantity.
        /// </summary>
        public static QsScalar Zero
        {
            get
            {
                return zero;
            }
        }
        #endregion


        #region QsValue Operations


        public override QsValue Identity
        {
            get
            {
                return One;
            }
        }

        public override QsValue AddOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;

            if (value is QsScalar)
            {
                return this.AddScalar((QsScalar)value);
            }
            else if (value is QsVector)
            {
                var b = value as QsVector;

                return this.AddVector(b);

            }
            else if (value is QsText)
            {
                var text = value as QsText;
                var qt = QsEvaluator.CurrentEvaluator.SilentEvaluate(text.Text) as QsValue;
                return AddOperation(qt);
            }
            else if (value is QsFunction)
            {
                if (this.ScalarType == ScalarTypes.SymbolicQuantity)
                {
                    var fn = value as QsFunction;
                    return this.AddScalar(fn.ToSymbolicScalar());
                }
                else
                {
                    throw new NotImplementedException("Adding QsScalar[" + this.ScalarType.ToString() + "] from QsFunction is not implemented yet");
                }
            }

            else
            {
                throw new NotImplementedException("Adding QsScalar to " + value.GetType().Name + " is not implemented yet");
            }
        }

        public override QsValue SubtractOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                return this.SubtractScalar((QsScalar)value);
            }
            else if (value is QsVector)
            {
                var b = value as QsVector;

                return this.SubtractVector(b);

            }
            else if (value is QsMatrix)
            {
                var m = value as QsMatrix;
                return this.SubtractMatrix(m);
            }
            else if (value is QsText)
            {
                var text = value as QsText;
                var qt = QsEvaluator.CurrentEvaluator.SilentEvaluate(text.Text) as QsValue;
                return SubtractOperation(qt);
            }
            else if (value is QsFunction)
            {
                if (this.ScalarType == ScalarTypes.SymbolicQuantity)
                {
                    var fn = value as QsFunction;
                    return this.SubtractScalar(fn.ToSymbolicScalar());
                }
                else
                {
                    throw new NotImplementedException("Subtracting QsScalar[" + this.ScalarType.ToString() + "] from QsFunction is not implemented yet");
                }
            }
            else
            {
                throw new NotImplementedException("Subtracting QsScalar from " + value.GetType().Name + " is not implemented yet");
            }
        }


        public override QsValue MultiplyOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                if (this._ScalarType == ScalarTypes.QsOperation)
                    return Operation.MultiplyOperation(value);
                else
                    return this.MultiplyScalar((QsScalar)value);
            }
            else if (value is QsVector)
            {
                if (this.ScalarType == ScalarTypes.QsOperation)
                {
                    // because the operation may include Del operator which behave like vector.
                    return this.Operation.MultiplyOperation(value);
                }
                else
                {
                    var b = value as QsVector;

                    return this.MultiplyVector(b);
                }
            }
            else if (value is QsMatrix)
            {
                var m = value as QsMatrix;
                return this.MultiplyMatrix(m);
            }
            else if (value is QsText)
            {
                var text = value as QsText;
                var qt = QsEvaluator.CurrentEvaluator.SilentEvaluate(text.Text) as QsValue;
                return MultiplyOperation(qt);
            }
            else if (value is QsFunction)
            {
                if (this.ScalarType == ScalarTypes.SymbolicQuantity)
                {
                    var fn = value as QsFunction;
                    return this.MultiplyScalar(fn.ToSymbolicScalar());
                }
                else
                {
                    throw new NotImplementedException("Multiplying QsScalar[" + this.ScalarType.ToString() + "] from QsFunction is not implemented yet");
                }
            }
            else
            {
                throw new NotImplementedException("Multiplying QsScalar with " + value.GetType().Name + " is not implemented yet");
            }
        }

        public override QsValue DotProductOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (this._ScalarType == ScalarTypes.QsOperation)
            {
                return this.Operation.DotProductOperation(value);
            }
            else
            {
                return this.MultiplyOperation(value);
            }
        }

        public override QsValue CrossProductOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;

            if (this._ScalarType == ScalarTypes.QsOperation)
            {
                return this.Operation.CrossProductOperation(value);
            }
            else
            {
                return this.MultiplyOperation(value);
            }
        }

        public override QsValue DivideOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                return this.DivideScalar((QsScalar)value);
            }
            else if (value is QsText)
            {
                var text = value as QsText;
                var qt = QsEvaluator.CurrentEvaluator.SilentEvaluate(text.Text) as QsValue;
                return DivideOperation(qt);
            }
            else if (value is QsFunction)
            {
                if (this.ScalarType == ScalarTypes.SymbolicQuantity)
                {
                    var fn = value as QsFunction;
                    return this.DivideScalar(fn.ToSymbolicScalar());
                }
                else
                {
                    throw new NotImplementedException("Dividing QsScalar[" + this.ScalarType.ToString() + "] from QsFunction is not implemented yet");
                }
            }
            else
            {
                throw new NotImplementedException("Dividing QsScalar over " + value.GetType().Name + " is not implemented yet");
            }
        }

        public override QsValue PowerOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                if (ScalarType == ScalarTypes.QsOperation) return this.Operation.PowerOperation(vl);
                return this.PowerScalar((QsScalar)value);
            }
            else
            {
                throw new NotImplementedException("Raising QsScalar to power of " + value.GetType().Name + " is not implemented yet");
            }
        }


        /// <summary>
        /// ||Scalar||
        /// </summary>
        /// <returns></returns>
        public override QsValue NormOperation()
        {
            return AbsOperation();
        }

        /// <summary>
        /// |Scalar|
        /// </summary>
        /// <returns></returns>
        public override QsValue AbsOperation()
        {
            var q = this.NumericalQuantity;

            if (q.Value < 0)
            {
                return new QsScalar { NumericalQuantity = q * "-1".ToQuantity() };
            }
            else
            {
                return new QsScalar { NumericalQuantity = q * "1".ToQuantity() };
            }

        }


        /// <summary>
        /// Calculate the modulo of 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue ModuloOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            // this is tricky because if you divide 5<m>/2<s> you got Speed 2.5<m/s>
            //   but the modulus will be 5<m> / 2<s> = 2<m/s> + 1<m>

            if (value is QsScalar)
            {
                return this.ModuloScalar((QsScalar)value);
            }
            else
            {
                throw new NotImplementedException("Modulo of QsScalar over " + value.GetType().Name + " is not implemented yet");
            }
        }

        public override QsValue TensorProductOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            throw new NotImplementedException("Tensor product of QsScalar and " + value.GetType().Name + " is not implemented yet");
        }

        public override QsValue LeftShiftOperation(QsValue times)
        {
            throw new NotImplementedException();
        }

        public override QsValue RightShiftOperation(QsValue times)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Relational Operation

        public override bool LessThan(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                QsScalar scalar = (QsScalar)value;
                return this.NumericalQuantity < scalar.NumericalQuantity;
            }
            else if (value is QsVector)
            {
                var vector = (QsVector)value;

                //compare with the magnitude of the vector
                var mag = vector.Magnitude();

                return ((QsScalar)this.AbsOperation()).NumericalQuantity < mag.NumericalQuantity;
                
            }
            else if (value is QsMatrix)
            {
                var matrix = (QsMatrix)value;
                throw new NotSupportedException();
            }
            else
            {
                throw new NotSupportedException();
            }
            
        }

        public override bool GreaterThan(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                QsScalar scalar = (QsScalar)value;
                return this.NumericalQuantity > scalar.NumericalQuantity;
            }
            else if (value is QsVector)
            {
                var vector = (QsVector)value;

                //compare with the magnitude of the vector
                var mag = vector.Magnitude();

                return ((QsScalar)this.AbsOperation()).NumericalQuantity > mag.NumericalQuantity;
            }
            else if (value is QsMatrix)
            {
                var matrix = (QsMatrix)value;
                throw new NotSupportedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override bool LessThanOrEqual(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                QsScalar scalar = (QsScalar)value;
                return this.NumericalQuantity <= scalar.NumericalQuantity;
            }
            else if (value is QsVector)
            {
                var vector = (QsVector)value;
                //compare with the magnitude of the vector
                var mag = vector.Magnitude();

                return ((QsScalar)this.AbsOperation()).NumericalQuantity <= mag.NumericalQuantity;
            }
            else if (value is QsMatrix)
            {
                var matrix = (QsMatrix)value;
                throw new NotSupportedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override bool GreaterThanOrEqual(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                QsScalar scalar = (QsScalar)value;
                return this.NumericalQuantity >= scalar.NumericalQuantity;
            }
            else if (value is QsVector)
            {
                var vector = (QsVector)value;
                //compare with the magnitude of the vector
                var mag = vector.Magnitude();

                return ((QsScalar)this.AbsOperation()).NumericalQuantity >= mag.NumericalQuantity;
            }
            else if (value is QsMatrix)
            {
                var matrix = (QsMatrix)value;
                throw new NotSupportedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override bool Equality(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if ((object)value == null) return false;

            if (value is QsScalar)
            {
                QsScalar scalar = (QsScalar)value;
                if (this.ScalarType == scalar.ScalarType)
                {
                    switch(this.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return this.NumericalQuantity == scalar.NumericalQuantity;
                        case ScalarTypes.ComplexNumberQuantity:
                            return this.ComplexQuantity == scalar.ComplexQuantity;
                        case  ScalarTypes.QuaternionNumberQuantity:
                            return this.QuaternionQuantity == scalar.QuaternionQuantity;
                        case ScalarTypes.SymbolicQuantity:
                            return this.SymbolicQuantity == scalar.SymbolicQuantity;
                        default:
                            throw new QsException("N/A");
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (value is QsVector)
            {
                var vector = (QsVector)value;
                //compare with the magnitude of the vector
                var mag = vector.Magnitude();

                return ((QsScalar)this.AbsOperation()).NumericalQuantity == mag.NumericalQuantity;
            }
            else if (value is QsMatrix)
            {
                var matrix = (QsMatrix)value;
                throw new NotSupportedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override bool Inequality(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                QsScalar scalar = (QsScalar)value;
                return this.NumericalQuantity != scalar.NumericalQuantity;
            }
            else if (value is QsVector)
            {
                var vector = (QsVector)value;
                //compare with the magnitude of the vector
                var mag = vector.Magnitude();

                return ((QsScalar)this.AbsOperation()).NumericalQuantity != mag.NumericalQuantity;
            }
            else if (value is QsMatrix)
            {
                var matrix = (QsMatrix)value;
                throw new NotSupportedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        public override QsValue GetIndexedItem(QsParameter[] indices)
        {
            // I am adding new feature here that
            //  I can execute the function if it was as a scalar value of function type.
            //  which means if f(x) = x^2
            //  you can call the function   as      f(3)
            //  or better you can call it  as  @f[3]
            // -------------
            //  why I am doing this  ??
            //   because I was  want to be able to call the function directly after differentiating it
            //   @f|$x[3]   
            //  why I didn't use  normal brackets??
            //   because it is reserved to know the function itself by parameters (as I have overloaded functions by parameter names) not types
            //   @f(x,y)  !=  @f(u,v)    etc.


            if (this._ScalarType == ScalarTypes.FunctionQuantity)
            {
                return this.FunctionQuantity.Value.InvokeByQsParameters(indices);
            }
            else
            {
                throw new QsException(string.Format("Indexer is not implemented for Scalar type {0}", _ScalarType.ToString()));
            }
        }

        public override void SetIndexedItem(QsParameter[] indices, QsValue value)
        {
            throw new NotImplementedException();
        }

        #region ICloneable Members

        public object Clone()
        {
            QsScalar n = new QsScalar(this._ScalarType);

            switch (_ScalarType)
            {
                case  ScalarTypes.NumericalQuantity:
                    n.NumericalQuantity = (AnyQuantity<double>)this.NumericalQuantity.Clone();
                    break;

                case ScalarTypes.SymbolicQuantity:
                    {
                        var svalue = (SymbolicVariable)this.SymbolicQuantity.Value.Clone();
                        var sq = this.SymbolicQuantity.Unit.GetThisUnitQuantity<SymbolicVariable>(svalue);
                        n.SymbolicQuantity = sq;
                    }
                    break;

                case ScalarTypes.ComplexNumberQuantity:
                    n.ComplexQuantity = (AnyQuantity<Complex>)this.ComplexQuantity.Clone();
                    break;

                case ScalarTypes.QuaternionNumberQuantity:
                    n.QuaternionQuantity = (AnyQuantity<Quaternion>)this.QuaternionQuantity.Clone();
                    break;

                case ScalarTypes.FunctionQuantity:
                    {
                        var fvalue = (QsFunction)this.FunctionQuantity.Value.Clone();
                        var fq = this.FunctionQuantity.Unit.GetThisUnitQuantity<QsFunction>(fvalue);
                        n.FunctionQuantity = fq;
                    }
                    break;

                case ScalarTypes.QsOperation:
                    n.Operation = (QsOperation)this.Operation.Clone();
                    break;


                case ScalarTypes.RationalNumberQuantity:
                    n.RationalQuantity = (AnyQuantity<Rational>)this.RationalQuantity.Clone();
                    break;

            }

            return n;
        }

        #endregion

        /// <summary>
        /// differentiate the current scalar  by overriding the method
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue DifferentiateOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
                return this.DifferentiateScalar((QsScalar)value);
            else
                return base.DifferentiateOperation(value);
        }

        /// <summary>
        /// make a range from  this scalar to the input parameter scalar.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue RangeOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            QsScalar to = value as QsScalar;
            if (to != null)
            {
                if (this._ScalarType == ScalarTypes.NumericalQuantity && to._ScalarType == ScalarTypes.NumericalQuantity)
                {
                    double start = this.NumericalQuantity.Value;
                    double end = to.NumericalQuantity.Value;
                    
                    QsVector v = new QsVector();
                    if (end >= start)
                        for (double id = start; id <= end; id++) v.AddComponent(id.ToQuantity().ToScalar());
                    else
                        for (double id = start; id >= end; id--) v.AddComponent(id.ToQuantity().ToScalar());

                    return v;
                }
                else
                {
                    throw new NotImplementedException("Range from " + this._ScalarType.ToString() + " to " + to._ScalarType.ToString() + " is not supported");
                }
            }
            else
            {
                throw new NotImplementedException("Range to " + value.GetType().Name + " is not supported");
            }
        }


        #region IConvertible
        public TypeCode GetTypeCode()
        {
            throw new NotImplementedException();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            return (byte)NumericalQuantity.Value;
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return (decimal)NumericalQuantity.Value;
        }

        public double ToDouble(IFormatProvider provider)
        {
            return NumericalQuantity.Value;
        }

        public short ToInt16(IFormatProvider provider)
        {
            return (short)NumericalQuantity.Value;
        }

        public int ToInt32(IFormatProvider provider)
        {
            return (int)NumericalQuantity.Value;
        }

        public long ToInt64(IFormatProvider provider)
        {
            return (long)NumericalQuantity.Value;
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return (sbyte)NumericalQuantity.Value;
        }

        public float ToSingle(IFormatProvider provider)
        {
            return (float)NumericalQuantity.Value;
        }

        public string ToString(IFormatProvider provider)
        {
            return NumericalQuantity.ToShortString();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {

            if (conversionType == typeof(QsFunction)) return FunctionQuantity.Value;
            if (conversionType == typeof(Complex)) return ComplexQuantity.Value;
            if (conversionType == typeof(Quaternion)) return QuaternionQuantity.Value;
            if (conversionType == typeof(Rational)) return RationalQuantity.Value;
            if (conversionType == typeof(SymbolicVariable)) return SymbolicQuantity.Value;

            switch (ScalarType)
            {
                case ScalarTypes.ComplexNumberQuantity:
                    return ComplexQuantity;
                case ScalarTypes.FunctionQuantity:
                    return FunctionQuantity;
                case ScalarTypes.NumericalQuantity:
                    return NumericalQuantity;
                case ScalarTypes.QuaternionNumberQuantity:
                    return QuaternionQuantity;
                case ScalarTypes.RationalNumberQuantity:
                    return RationalQuantity;
                case ScalarTypes.SymbolicQuantity:
                    return SymbolicQuantity;

                default:
                    return null;
            }

            
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return (ushort)NumericalQuantity.Value;
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return (uint)NumericalQuantity.Value;
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return (ulong)NumericalQuantity.Value;
        }
        #endregion

        public override QsValue Execute(ParticleLexer.Token expression)
        {
            if (this._ScalarType== ScalarTypes.QuaternionNumberQuantity && expression.TokenValue.Equals("RotationMatrix", StringComparison.OrdinalIgnoreCase))
            {
                return this.QuaternionQuantity.Value.To_3x3_RotationMatrix();
            }
            else 
            return base.Execute(expression);
        }


        /// <summary>
        /// returns constant value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static QsValue Constant(string name)
        {
            if(name.Equals("i", StringComparison.OrdinalIgnoreCase)) 
            {
                return new QsScalar(ScalarTypes.ComplexNumberQuantity)
                {
                    ComplexQuantity = Unit.Parse("1").GetThisUnitQuantity<Complex>( Complex.ImaginaryOne)
                };
            }

            if (name.Equals("pi", StringComparison.OrdinalIgnoreCase))
            {
                return Math.PI.ToQuantity().ToScalarValue();
            }

            if (name.Equals("phi", StringComparison.OrdinalIgnoreCase))
            {
                return ((1 + Math.Sqrt(5)) / 2).ToQuantity().ToScalarValue();
            }

            if (name.Equals("e", StringComparison.OrdinalIgnoreCase))
            {
                return Math.E.ToQuantity().ToScalarValue();
            }

            return zero;
        }
    }
}
