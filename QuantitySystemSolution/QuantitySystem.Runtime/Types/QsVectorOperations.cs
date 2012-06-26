using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using Qs.Runtime;
using System.Threading.Tasks;

namespace Qs.Types
{
    public partial class QsVector : QsValue, IEnumerable<QsScalar>
    {
        
        #region Scalar Operations
        /// <summary>
        /// Add Scalar to the vector components.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private QsVector AddScalar(QsScalar s)
        {
            QsVector v = new QsVector(this.Count);

            for (int i = 0; i < this.Count; i++)
            {
                v.AddComponent(this[i] + s);
            }

            return v;
        }

        /// <summary>
        /// Subtract scalar from the vector components.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private QsVector SubtractScalar(QsScalar s)
        {
            QsVector v = new QsVector(this.Count);

            for (int i = 0; i < this.Count; i++)
            {
                v.AddComponent(this[i] - s);
            }

            return v;

        }

        private QsVector MultiplyScalar(QsScalar s)
        {
            QsVector v = new QsVector(this.Count);

            for (int i = 0; i < this.Count; i++)
            {
                v.AddComponent(this[i] * s);
            }

            return v;

        }

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


        /// <summary>
        /// Differentiate every component in vectpr with the scalar.
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public QsVector DifferentiateScalar(QsScalar scalar)
        {
            QsVector v = new QsVector(this.Count);

            for (int i = 0; i < this.Count; i++)
            {
                v.AddComponent((QsScalar)this[i].DifferentiateScalar(scalar));
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

            QsVector v = QsVector.CopyVector(this);

            Parallel.For(0, this.Count, (i) =>
                {
                    v[i] = v[i] + vector[i];
                }
            );


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

            QsVector v = QsVector.CopyVector(this);

            Parallel.For(0, this.Count, (i) =>
            {
                v[i] = v[i] - vector[i];
            }
            );



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

            QsVector v = QsVector.CopyVector(this);

            Parallel.For(0, this.Count, (i) =>
            {
                v[i] = v[i].MultiplyScalar(vector[i]);
            }
            );


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

            QsVector v = QsVector.CopyVector(this);

            Parallel.For(0, this.Count, (i) =>
            {
                v[i] = v[i] / vector[i];
            }
            );

            return v;
        }

        private QsValue ModuloVector(QsVector vector)
        {
            if (this.Count != vector.Count) throw new QsException("Vectors are not equal");


            QsVector v = QsVector.CopyVector(this);

            Parallel.For(0, this.Count, (i) =>
            {
                v[i] = v[i] % vector[i];
            }
            );


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
            if (this.Count != 3) throw new QsException("Cross product only happens with 3 component vector");

            // cross product as determinant of matrix.

            QsMatrix mat = new QsMatrix(
                new QsVector(QsScalar.One, QsScalar.One, QsScalar.One)
                , this
                , v2);

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
                return this.AddVector((QsVector)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override QsValue SubtractOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                return SubtractScalar((QsScalar)value);
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
        public override QsValue MultiplyOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                return MultiplyScalar((QsScalar)value);
            }
            else if (value is QsVector)
            {
                return this.MultiplyVector((QsVector)value);
            }
            else if (value is QsMatrix)
            {
                QsMatrix mvec =  this.ToVectorMatrix().MultiplyMatrix((QsMatrix)value);
                // make it vector again.
                return mvec[0];
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
        public override QsValue DotProductOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                return MultiplyScalar((QsScalar)value);
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

        public override QsValue CrossProductOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                return MultiplyScalar((QsScalar)value);
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

        public override QsValue DivideOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


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

        public override QsValue DifferentiateOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {   
                return this.DifferentiateScalar((QsScalar)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override QsValue PowerOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


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

        public override QsValue ModuloOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


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
        public override QsValue TensorProductOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


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

                return (new QsMatrix(vcs.ToArray()));

            }
            else
            {
                throw new NotSupportedException();
            }
            
        }


        public override QsValue LeftShiftOperation(QsValue vl)
        {
            QsValue times;
            if (vl is QsReference) times = ((QsReference)vl).ContentValue;
            else times = vl;


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

        public override QsValue RightShiftOperation(QsValue vl)
        {
            QsValue times;
            if (vl is QsReference) times = ((QsReference)vl).ContentValue;
            else times = vl;


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



        public override bool LessThan(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


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

        public override bool GreaterThan(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


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

        public override bool LessThanOrEqual(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


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

        public override bool GreaterThanOrEqual(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


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

        public override bool Equality(QsValue vl)
        {

            if ((object)vl == null) return false;

            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


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

        public override bool Inequality(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


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

        public override QsValue GetIndexedItem(QsParameter[] allIndices)
        {
            if (allIndices.Count() > 1) throw new QsException("Vector have one index only");
            int[] indices = new int[allIndices.Length];
            for (int ix = 0; ix < indices.Length; ix++) indices[ix] = (int)((QsScalar)allIndices[ix].QsNativeValue).NumericalQuantity.Value;                
            int index = indices[0];
            return ListStorage[index];
        }

        public override void SetIndexedItem(QsParameter[] indices, QsValue value)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Some operations on the vector.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public override QsValue Execute(ParticleLexer.Token expression)
        {
            if (expression.TokenValue.Equals("length", StringComparison.OrdinalIgnoreCase))
                return this.Count.ToScalarValue();

            return base.Execute(expression);

        }
    }
}
