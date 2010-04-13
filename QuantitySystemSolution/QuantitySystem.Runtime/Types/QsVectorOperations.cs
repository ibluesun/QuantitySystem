using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using Qs.Runtime;

namespace Qs.Types
{
    public partial class QsVector : QsValue, IEnumerable<QsScalar>
    {
        
        #region Scalar Operations
        public QsVector DivideScalar(QsScalar scalar)
        {
            QsVector v = new QsVector(this.Count);

            for (int i = 0; i < this.Count; i++)
            {
                v.AddComponent(this[i] / scalar);
            }

            return v;

        }

        private QsValue ModuloScalar(QsScalar scalar)
        {
            QsVector v = new QsVector(this.Count);

            for (int i = 0; i < this.Count; i++)
            {
                v.AddComponent(this[i] % scalar);
            }

            return v;
        }


        public QsVector PowerScalar(QsScalar scalar)
        {

            QsVector v = new QsVector(this.Count);

            for (int i = 0; i < this.Count; i++)
            {
                v.AddComponent(this[i].PowerScalar(scalar));
            }

            return v;
        }

        #endregion
        #region Vector Operations

        /// <summary>
        /// Adds two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public QsVector AddVector(QsVector vector)
        {
            if (this.Count != vector.Count) throw new QsException("Vectors are not equal");

            QsVector v = new QsVector(this.Count);

            for (int i = 0; i < this.Count; i++)
            {
                v.AddComponent(this[i] + vector[i]);
            }

            return v;
        }

        /// <summary>
        /// Subtract two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public QsVector SubtractVector(QsVector vector)
        {
            if (this.Count != vector.Count) throw new QsException("Vectors are not equal");

            QsVector v = new QsVector(this.Count);

            for (int i = 0; i < this.Count; i++)
            {
                v.AddComponent(this[i] - vector[i]);
            }

            return v;
        }



        /// <summary>
        /// Multiply vector component by component.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public QsVector MultiplyVector(QsVector vector)
        {
            if (this.Count != vector.Count) throw new QsException("Vectors are not equal");

            QsVector v = new QsVector(this.Count);

            for (int i = 0; i < this.Count; i++)
            {
                v.AddComponent(this[i] * vector[i]);
            }

            return v;
        }
        
        
        /// <summary>
        /// Divide vector component by component.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public QsVector DivideVector(QsVector vector)
        {
            if (this.Count != vector.Count) throw new QsException("Vectors are not equal");

            QsVector v = new QsVector(this.Count);

            for (int i = 0; i < this.Count; i++)
            {
                v.AddComponent(this[i] / vector[i]);
            }

            return v;
        }

        private QsValue ModuloVector(QsVector vector)
        {
            if (this.Count != vector.Count) throw new QsException("Vectors are not equal");

            QsVector v = new QsVector(this.Count);

            for (int i = 0; i < this.Count; i++)
            {
                v.AddComponent(this[i] % vector[i]);
            }

            return v;
        }



        /*
         * Scalar prodcut is commutative that is a{}.b{} = b{}.a{}
         * a{}.b{} = a1b1+a2b2+anbn
         */


        /// <summary>
        /// Dot product of two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public QsScalar ScalarProduct(QsVector vector)
        {
            if (this.Count != vector.Count) throw new QsException("Vectors are not equal");

            AnyQuantity<double> Total = this[0].Quantity * vector[0].Quantity;

            for (int i = 1; i < this.Count; i++)
            {
                Total = Total + (this[i].Quantity * vector[i].Quantity);
            }

            return new QsScalar { Quantity = Total };
        }


        /// <summary>
        /// Cross product for 3 components vector.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public QsVector VectorProduct(QsVector v2)
        {
            if (this.Count != v2.Count) throw new QsException("Vectors are not equal");


            QsVector units = new QsVector(this.Count);
            QsVector a = new QsVector(this.Count);
            QsVector b = new QsVector(v2.Count);

            for (int i = 0; i < this.Count; i++)
            {
                // first row is the units.
                var utou = this[i].Quantity.Unit.PathToUnit(v2[i].Quantity.Unit).ConversionFactor; //the units may be different but the quantities are the same.
                Unit u = (Unit)this[i].Quantity.Unit.Clone();

                // unit with the conversion factor
                units.AddComponent(new QsScalar { Quantity = u.GetThisUnitQuantity<double>(utou) });


                // second row is first vector
                //take the value of the quantity and convert it to dimensionless value.
                a.AddComponent(new QsScalar { Quantity = this[i].Quantity.Value.ToQuantity() });

                // third row is the second vector
                b.AddComponent(new QsScalar { Quantity = v2[i].Quantity.Value.ToQuantity() });

            }

            QsMatrix mat = new QsMatrix(units, a, b);

            return mat.Determinant();

            // problem now: what if we have more than 3 elements in the vector.
            // there is no cross product for more than 3 elements for vectors.

        }




        #endregion



        #region QsValue operations

        public override QsValue Identity

        {
            get
            {
                QsVector v = new QsVector(this.Count);
                for (int i = 0; i < this.Count; i++)
                {
                    v.AddComponent(QsScalar.One);
                }
                return v;
            }
        }

        public override QsValue AddOperation(QsValue value)
        {
            if (value is QsScalar)
            {
                var s = value as QsScalar;
                return s.AddVector(this);
            }
            else if (value is QsVector)
            {
                return this.AddVector((QsVector)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }


        private QsVector SubtractScalar(QsScalar s)
        {
            QsVector v = new QsVector(this.Count);

            for (int i = 0; i < this.Count; i++)
            {
                v.AddComponent(this[i] - s);
            }

            return v;

        }

        public override QsValue SubtractOperation(QsValue value)
        {
            if (value is QsScalar)
            {
                var s = value as QsScalar;
                
                return SubtractScalar(s);
            }
            else if (value is QsVector)
            {
                return this.SubtractVector((QsVector)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }


        /// <summary>
        /// Normal multiplication.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue MultiplyOperation(QsValue value)
        {
            if (value is QsScalar)
            {
                var s = value as QsScalar;
                return s.MultiplyVector(this);
            }
            else if (value is QsVector)
            {
                return this.MultiplyVector((QsVector)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }


        /// <summary>
        /// Dot product
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue DotProductOperation(QsValue value)
        {
            if (value is QsScalar)
            {
                var s = value as QsScalar;
                return s.MultiplyVector(this);
            }
            else if (value is QsVector)
            {
                return this.ScalarProduct((QsVector)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override QsValue CrossProductOperation(QsValue value)
        {
            if (value is QsScalar)
            {
                var s = value as QsScalar;
                return s.MultiplyVector(this);
            }
            else if (value is QsVector)
            {
                return this.VectorProduct((QsVector)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override QsValue DivideOperation(QsValue value)
        {
            if (value is QsScalar)
            {
                var s = value as QsScalar;
                return this.DivideScalar(s);
            }
            else if (value is QsVector)
            {
                return this.DivideVector((QsVector)value);
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
                var s = value as QsScalar;
                return this.PowerScalar(s);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// ||vector||
        /// </summary>
        /// <returns></returns>
        public override QsValue NormOperation()
        {
            return this.Magnitude();
        }

        /// <summary>
        /// |vector|
        /// </summary>
        /// <returns></returns>
        public override QsValue AbsOperation()
        {
            return this.Magnitude();  
            
            //this is according to wikipedia that |x| if x is vector is valid but discouraged
            //  the norm of vector should be ||x|| notation.


        }

        public override QsValue ModuloOperation(QsValue value)
        {
            if (value is QsScalar)
            {
                var s = value as QsScalar;
                return this.ModuloScalar(s);
            }
            else if (value is QsVector)
            {
                return this.ModuloVector((QsVector)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Form a matrix from two vectors
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue TensorProductOperation(QsValue value)
        {
            if (value is QsScalar)
            {
                throw new NotSupportedException();
            }
            else if (value is QsVector)
            {
                
                var vec = (QsVector)value;

                if (vec.Count != this.Count) throw new QsException("Not equal vector components");

                List<QsVector> vcs = new List<QsVector>();
                foreach (var c in this)
                {
                    QsVector v = (QsVector)(c * vec);
                    vcs.Add(v);

                }

                return new QsMatrix(vcs.ToArray());

            }
            else
            {
                throw new NotSupportedException();
            }
            
        }


        public override QsValue LeftShiftOperation(QsValue times)
        {
            int itimes = Qs.IntegerFromQsValue((QsScalar)times);

            if (itimes > this.Count) itimes = itimes % this.Count;

            
            QsVector vec = new QsVector(this.Count);
            
            for (int i = itimes; i < this.Count; i++)
            {
                vec.AddComponent(this[i]);
            }

            for (int i = 0; i < itimes; i++)
            {
                vec.AddComponent(this[i]);
            }


            return vec;
        }

        public override QsValue RightShiftOperation(QsValue times)
        {
            int itimes = Qs.IntegerFromQsValue((QsScalar)times);

            if (itimes > this.Count) itimes = itimes % this.Count;

            // 1 2 3 4 5 >> 2  == 4 5 1 2 3

            QsVector vec = new QsVector(this.Count);

            for (int i = this.Count - itimes; i < this.Count; i++)
            {
                vec.AddComponent(this[i]);
            }

            for (int i = 0; i < (this.Count - itimes); i++)
            {
                vec.AddComponent(this[i]);
            }


            return vec;
        }


        #endregion



        public override bool LessThan(QsValue value)
        {
            if (value is QsScalar)
            {
                var s = (QsScalar)value.AbsOperation();

                return this.Magnitude().Quantity < s.Quantity;
            }
            else if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (this.Magnitude().Quantity < v.Magnitude().Quantity);
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
                var s = (QsScalar)value.AbsOperation();
                return this.Magnitude().Quantity > s.Quantity;

            }
            else if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (this.Magnitude().Quantity > v.Magnitude().Quantity);

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
                var s = (QsScalar)value.AbsOperation();
                return this.Magnitude().Quantity <= s.Quantity;

            }
            else if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (this.Magnitude().Quantity <= v.Magnitude().Quantity);

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
                var s = (QsScalar)value.AbsOperation();
                return this.Magnitude().Quantity >= s.Quantity;

            }
            else if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (this.Magnitude().Quantity >= v.Magnitude().Quantity);

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
                var s = (QsScalar)value.AbsOperation();
                return this.Magnitude().Quantity == s.Quantity;

            }
            else if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (this.Magnitude().Quantity == v.Magnitude().Quantity);

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
                var s = (QsScalar)value.AbsOperation();
                return this.Magnitude().Quantity != s.Quantity;

            }
            else if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (this.Magnitude().Quantity != v.Magnitude().Quantity);

            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
