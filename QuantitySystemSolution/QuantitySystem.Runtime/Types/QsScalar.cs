using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using Qs.Runtime;
using SymbolicAlgebra;
using System.Globalization;
using QuantitySystem.Units;

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
                    default:
                        throw new NotImplementedException();
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
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    return NumericalQuantity.ToString();
                case ScalarTypes.SymbolicQuantity:
                    return SymbolicQuantity.ToString();
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " Operation not implemented yet");
            }
        }

        public string ToShortString()
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    return NumericalQuantity.ToShortString();
                case ScalarTypes.SymbolicQuantity:
                    return SymbolicQuantity.ToShortString();
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " Operation not implemented yet");
            }
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
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " Operation not implemented yet");
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
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " Operation not implemented yet");
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
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " Operation not implemented yet");
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
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " Operation not implemented yet");
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
                                int ipower = (int)power.NumericalQuantity.Value;

                                if ((power.NumericalQuantity.Value - ipower) > 0.00000000000000001) //because IEEE floating number is crazy about comparisons
                                    throw new QsException("Raising Symbolic Quantity to Real Number is not supported");
                                
                                QsScalar nsq = new QsScalar(ScalarTypes.SymbolicQuantity);
                                nsq.SymbolicQuantity = this.SymbolicQuantity.Value.Power(ipower).ToQuantity();
                                nsq.SymbolicQuantity.Unit = this.SymbolicQuantity.Unit.RaiseUnitPower(ipower);
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
                default:
                    throw new NotImplementedException(_ScalarType.ToString() + " Operation not implemented yet");
            }
        }

        public QsScalar ModuloScalar(QsScalar scalar)
        {
            return new QsScalar { NumericalQuantity = this.NumericalQuantity % scalar.NumericalQuantity };
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
            else
            {
                throw new NotSupportedException();
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
            else
            {
                throw new NotSupportedException();
            }
        }


        public override QsValue MultiplyOperation(QsValue value)
        {
            if (value is QsScalar)
            {
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
            else
            {
                throw new NotSupportedException();
            }
        }

        public override QsValue DotProductOperation(QsValue value)
        {
            return this.MultiplyOperation(value);
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
            else
            {
                throw new NotSupportedException();
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
                throw new NotSupportedException();
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
                throw new NotSupportedException();
            }
        }

        public override QsValue TensorProductOperation(QsValue value)
        {
            throw new NotImplementedException();
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
                return this.NumericalQuantity == scalar.NumericalQuantity;
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


        #region ICloneable Members

        public object Clone()
        {
            QsScalar n = new QsScalar(this._ScalarType);

            n.NumericalQuantity = this.NumericalQuantity;
            n.SymbolicQuantity = this.SymbolicQuantity;

            return n;
        }

        #endregion
    }
}
