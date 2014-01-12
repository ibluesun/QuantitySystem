using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using Qs.Runtime;

namespace Qs.Types
{
    public partial class QsVector: QsValue, IEnumerable<QsScalar>
    {

        /// <summary>
        /// for storing the quantities.
        /// </summary>
        private List<QsScalar> ListStorage;


        #region constructors

        public QsVector()
        {
            ListStorage = new List<QsScalar>();
        }

        public QsVector(int count)            
        {
            ListStorage = new List<QsScalar>(count);
            
        }

        public QsVector(params QsScalar[] scalars)
        {
            ListStorage = new List<QsScalar>(scalars.Length);
            ListStorage.AddRange(scalars);
        }

        /// <summary>
        /// Concatenate the double value to the current vector.
        /// </summary>
        /// <param name="value"></param>
        public void AddComponent(double value)
        {
            ListStorage.Add(value.ToQuantity().ToScalar());
        }

        /// <summary>
        /// Concatenate the scalar to the current vector.
        /// </summary>
        /// <param name="scalar"></param>
        public void AddComponent(QsScalar scalar)
        {
            ListStorage.Add(scalar);
        }

        /// <summary>
        /// Concatenate the scalars to the current vector
        /// </summary>
        /// <param name="scalars"></param>
        public void AddComponents(params QsScalar[] scalars)
        {
            ListStorage.AddRange(scalars);
        }

        /// <summary>
        /// Concatenate the elements of vectors to the current vector.
        /// </summary>
        /// <param name="vector"></param>
        public void AddComponents(QsVector vector)
        {
            foreach (var s in vector) ListStorage.Add(s);
        }

        #endregion

        /// <summary>
        /// Magnitude of the vector.
        /// </summary>
        /// <returns></returns>
        public QsScalar Magnitude()
        {
            

            var v_dot_v = this.DotProductOperation(this) as QsScalar;

            var sqrt_v_dot_v = v_dot_v.PowerScalar("0.5".ToScalar());


            return sqrt_v_dot_v;
        }

        /// <summary>
        /// Returns the sum of the vector components
        /// </summary>
        /// <returns></returns>
        public QsScalar Sum()
        {
            QsScalar total = this[0];
            for (int i = 1; i < this.Count; i++) total = total + this[i];
            return total;

        }

        #region Vector behaviour
        public int Count
        {
            get
            {
                return ListStorage.Count;
            }
        }

        public QsScalar this[int i]
        {
            get
            {
                if (i < 0) i = ListStorage.Count + i;
                return ListStorage[i];
            }
            set
            {
                if (i < 0) i = ListStorage.Count + i;
                ListStorage[i] = value;
            }
        }
        #endregion


        /// <summary>
        /// Test if the vector is one element only.
        /// </summary>
        public bool IsScalar
        {
            get
            {
                if (Count == 1) return true;
                else return false;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("QsVector: ");
            for (int ix = 0; ix < this.Count; ix++)
            {
                string cell = this[ix].ToShortString();
                sb.Append(cell);
                sb.Append(" ");
            }

            return sb.ToString();
        }


        /// <summary>
        /// Copy the vector to another instance with the same components.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static QsVector CopyVector(QsVector vector)
        {
            QsVector vec = new QsVector(vector.Count);

            foreach (var q in vector)
            {
                vec.AddComponent((QsScalar)q.Clone());
            }
            return vec;
        }

        public QsScalar[] ToArray()
        {
            return ListStorage.ToArray();
        }



        #region IEnumerable<QsScalar> Members

        public IEnumerator<QsScalar> GetEnumerator()
        {
            return ListStorage.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ListStorage.GetEnumerator();
        }

        #endregion




        #region ICloneable Members

        public object Clone()
        {
            return CopyVector(this);

        }

        #endregion



        public override string ToShortString()
        {
            return "Vector";
        }

        public QsMatrix ToVectorMatrix()
        {
            return new QsMatrix(this);
        }

        public QsMatrix ToCoVectorMatrix()
        {
            return new QsMatrix(this).Transpose();
        }

    }
}
