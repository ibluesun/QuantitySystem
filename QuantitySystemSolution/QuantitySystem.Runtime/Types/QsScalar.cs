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
    public sealed class QsScalar : QsValue, ICloneable
    {

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
                    scalar = FunctionQuantity.ToString();
                    break;
                case ScalarTypes.QsOperation:
                    scalar = Operation.ToString();
                    break;
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " Operation not implemented yet");
            }

            return scalar;
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
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " Operation not implemented yet");
            }
        }

        /// <summary>
        /// Text that is able to be parsed.
        /// </summary>
        /// <returns></returns>
        public string ToExpressionString()
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    return NumericalQuantity.ToShortString();
                case ScalarTypes.ComplexNumberQuantity:
                    return "C{" + ComplexQuantity.Value.Real.ToString(CultureInfo.InvariantCulture) + ", " + ComplexQuantity.Value.Imaginary.ToString(CultureInfo.InvariantCulture) + "}" + this.ComplexQuantity.UnitText;
                case ScalarTypes.QuaternionNumberQuantity:
                    return "H{" + QuaternionQuantity.Value.Real.ToString(CultureInfo.InvariantCulture) + ", "
                        + QuaternionQuantity.Value.i.ToString(CultureInfo.InvariantCulture) + ", "
                        + QuaternionQuantity.Value.j.ToString(CultureInfo.InvariantCulture) + ", "
                        + QuaternionQuantity.Value.k.ToString(CultureInfo.InvariantCulture) + "}"
                        + this.QuaternionQuantity.UnitText;
                case ScalarTypes.SymbolicQuantity:
                    {
                        //add $ before any symbol
                        string sq = SymbolicQuantity.Value.ToString();
                        foreach (string sym in SymbolicQuantity.Value.InvolvedSymbols)
                            sq = sq.Replace(sym, "$" + sym);
                        return sq + SymbolicQuantity.UnitText;
                        
                    }
                case ScalarTypes.FunctionQuantity:
                    {
                        return FunctionQuantity.Value.SymbolicBodyText;
                    }
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " ToExpression String is not implemented yet");
            }

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
                case ScalarTypes.NumericalQuantity:
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
                        default:
                            throw new NotImplementedException();
                    }
                case ScalarTypes.SymbolicQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity + scalar.ToSymbolicQuantity() };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity + scalar.SymbolicQuantity };
                        default:
                            throw new NotImplementedException();
                    }
                case ScalarTypes.ComplexNumberQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = this.ComplexQuantity + scalar.NumericalQuantity.ToComplex() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = this.ComplexQuantity + scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.ComplexQuantity.ToQuaternion() + scalar.QuaternionQuantity };
                        default:
                            throw new NotImplementedException();
                    }
                case ScalarTypes.QuaternionNumberQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.QuaternionQuantity + scalar.NumericalQuantity.ToQuaternion() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.QuaternionQuantity + scalar.ComplexQuantity.ToQuaternion() };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = this.QuaternionQuantity + scalar.QuaternionQuantity };
                        default:
                            throw new NotImplementedException();
                    }
                case ScalarTypes.FunctionQuantity:
                    {
                        return ((QsFunction)FunctionQuantity.Value.AddOperation(scalar)).ToQuantity().ToScalar();
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
                        default:
                            throw new NotImplementedException();
                    }
                case ScalarTypes.SymbolicQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity - scalar.ToSymbolicQuantity() };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity - scalar.SymbolicQuantity };
                        default:
                            throw new NotImplementedException();
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
                        default:
                            throw new NotImplementedException();
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
                        default:
                            throw new NotImplementedException();
                    }
                case ScalarTypes.FunctionQuantity:
                    {
                        return ((QsFunction)FunctionQuantity.Value.SubtractOperation(scalar)).ToQuantity().ToScalar();
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
                        default:
                            throw new NotImplementedException();
                    }
                case ScalarTypes.SymbolicQuantity:
                    {
                        switch (scalar.ScalarType)
                        {
                            case ScalarTypes.NumericalQuantity:
                                return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity * scalar.ToSymbolicQuantity() };
                            case ScalarTypes.SymbolicQuantity:
                                return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity * scalar.SymbolicQuantity };
                            default:
                                throw new NotImplementedException();
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
                        default:
                            throw new NotImplementedException();
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
                        default:
                            throw new NotImplementedException();
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
                        default:
                            throw new NotImplementedException();
                    }
                case ScalarTypes.SymbolicQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity / scalar.ToSymbolicQuantity() };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this.SymbolicQuantity / scalar.SymbolicQuantity };
                        default:
                            throw new NotImplementedException();
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
                        default:
                            throw new NotImplementedException();
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
                        default:
                            throw new NotImplementedException();
                    }
                case ScalarTypes.FunctionQuantity:
                    {
                        return ((QsFunction)FunctionQuantity.Value.DivideOperation(scalar)).ToQuantity().ToScalar();
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
                                return new QsScalar { NumericalQuantity = AnyQuantity<double>.Power(this.NumericalQuantity, power.NumericalQuantity) };
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
                            default:
                                throw new NotImplementedException();
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
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " ^ " + power.ScalarType.ToString());
            }
        }

        public QsScalar ModuloScalar(QsScalar scalar)
        {
            return new QsScalar { NumericalQuantity = this.NumericalQuantity % scalar.NumericalQuantity };
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
                        return (QsScalar)this.Operation.DifferentiateOperation(scalar);
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
                v.AddComponent(this * vector[i]);
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
        public static QsScalar MinusOne
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

        public override QsValue AddOperation(QsValue value)
        {
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
                    return this.AddScalar(fn.SymbolicBodyScalar);
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

        public override QsValue SubtractOperation(QsValue value)
        {
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
                    return this.SubtractScalar(fn.SymbolicBodyScalar);
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


        public override QsValue MultiplyOperation(QsValue value)
        {
            if (value is QsScalar)
            {
                if (this._ScalarType == ScalarTypes.QsOperation)
                    return Operation.MultiplyOperation(value);
                else
                    return this.MultiplyScalar((QsScalar)value);
            }
            else if (value is QsVector)
            {
                var b = value as QsVector;

                return this.MultiplyVector(b);
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
                    return this.MultiplyScalar(fn.SymbolicBodyScalar);
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

        public override QsValue DotProductOperation(QsValue value)
        {
            if (this._ScalarType == ScalarTypes.QsOperation)
            {
                return this.Operation.DotProductOperation(value);
            }
            else
            {
                return this.MultiplyOperation(value);
            }
        }

        public override QsValue CrossProductOperation(QsValue value)
        {
            return this.MultiplyOperation(value);
        }

        public override QsValue DivideOperation(QsValue value)
        {
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
                    return this.DivideScalar(fn.SymbolicBodyScalar);
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

        public override QsValue PowerOperation(QsValue value)
        {
            if (value is QsScalar)
            {
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
        public override QsValue ModuloOperation(QsValue value)
        {
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

        public override QsValue TensorProductOperation(QsValue value)
        {
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

        public override bool LessThan(QsValue value)
        {
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

        public override bool GreaterThan(QsValue value)
        {
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

        public override bool LessThanOrEqual(QsValue value)
        {
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

        public override bool GreaterThanOrEqual(QsValue value)
        {
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

        public override bool Equality(QsValue value)
        {
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

        public override bool Inequality(QsValue value)
        {
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

        public override QsValue GetIndexedItem(int[] indices)
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

            }

            return n;
        }

        #endregion


        public override QsValue DifferentiateOperation(QsValue value)
        {
            if (value is QsScalar)
                return this.DifferentiateScalar((QsScalar)value);
            else
                return base.DifferentiateOperation(value);
        }
        
    }
}
