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

            QsScalar Total = this[0] * vector[0];

            for (int i = 1; i < this.Count; i++)
            {
                Total = Total + (this[i] * vector[i]);
            }

            return Total ;
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
                var utou = this[i].Unit.PathToUnit(v2[i].Unit).ConversionFactor; //the units may be different but the quantities are the same.
                Unit u = (Unit)this[i].Unit.Clone();

                // unit with the conversion factor
                units.AddComponent(new QsScalar { NumericalQuantity = u.GetThisUnitQuantity<double>(utou) });


                // second row is first vector

                //take the value of the quantity and convert it to dimensionless value.
                if (this[i].ScalarType == ScalarTypes.NumericalQuantity)
                    a.AddComponent(new QsScalar { NumericalQuantity = this[i].NumericalQuantity.Value.ToQuantity() });
                else if(this[i].ScalarType==ScalarTypes.SymbolicQuantity)
                    a.AddComponent(new QsScalar( ScalarTypes.SymbolicQuantity) { SymbolicQuantity = this[i].SymbolicQuantity.Value.ToQuantity()});
                else 
                    throw new NotImplementedException();


                // third row is the second vector
                if (v2[i].ScalarType == ScalarTypes.NumericalQuantity)
                    b.AddComponent(new QsScalar { NumericalQuantity = v2[i].NumericalQuantity.Value.ToQuantity() });
                else if (v2[i].ScalarType == ScalarTypes.SymbolicQuantity)
                    b.AddComponent(new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = v2[i].SymbolicQuantity.Value.ToQuantity() });
                else
                    throw new NotImplementedException();

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

                return this.Magnitude().NumericalQuantity < s.NumericalQuantity;
            }
            else if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (this.Magnitude().NumericalQuantity < v.Magnitude().NumericalQuantity);
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
                return this.Magnitude().NumericalQuantity > s.NumericalQuantity;

            }
            else if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (this.Magnitude().NumericalQuantity > v.Magnitude().NumericalQuantity);

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
                return this.Magnitude().NumericalQuantity <= s.NumericalQuantity;

            }
            else if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (this.Magnitude().NumericalQuantity <= v.Magnitude().NumericalQuantity);

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
                return this.Magnitude().NumericalQuantity >= s.NumericalQuantity;

            }
            else if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (this.Magnitude().NumericalQuantity >= v.Magnitude().NumericalQuantity);

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
                return this.Magnitude().NumericalQuantity == s.NumericalQuantity;

            }
            else if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (this.Magnitude().NumericalQuantity == v.Magnitude().NumericalQuantity);

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
                return this.Magnitude().NumericalQuantity != s.NumericalQuantity;

            }
            else if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (this.Magnitude().NumericalQuantity != v.Magnitude().NumericalQuantity);

            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override QsValue GetIndexedItem(int[] indices)
        {
            if (indices.Count() > 1) throw new QsException("Vector have one index only");
            int index = indices[0];
            return ListStorage[index];
        }
    }
}
