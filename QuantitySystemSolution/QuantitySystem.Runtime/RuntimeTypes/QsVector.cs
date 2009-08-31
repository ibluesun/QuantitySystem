using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;

namespace Qs.RuntimeTypes
{
    public partial class QsVector: QsValue, IEnumerable<QsScalar>, ICloneable
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

        public void AddComponent(QsScalar scalar)
        {
            ListStorage.Add(scalar);
        }

        public void AddComponents(params QsScalar[] scalars)
        {
            ListStorage.AddRange(scalars);
        }

        #endregion

        /// <summary>
        /// Magnitude of the vector.
        /// </summary>
        /// <returns></returns>
        public QsScalar Magnitude()
        {
            var exponent = "2".ToQuantity();

            AnyQuantity<double> Total = AnyQuantity<double>.Power(this[0].Quantity, exponent);

            for (int i = 1; i < ListStorage.Count; i++)
            {
                Total = Total + AnyQuantity<double>.Power(this[i].Quantity, exponent);
            }

            exponent = "0.5".ToQuantity();

            Total = AnyQuantity<double>.Power(Total, exponent);

            return new QsScalar { Quantity = Total };
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
                return ListStorage[i];
            }
            set
            {
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
                string cell = this[ix].Quantity.ToShortString();
                sb.Append(cell);
                sb.Append(" ");
            }

            return sb.ToString();
        }



        public static QsVector CopyVector(QsVector vector)
        {
            QsVector vec = new QsVector(vector.Count);

            foreach (var q in vector)
            {
                vec.AddComponent(new QsScalar { Quantity = (AnyQuantity<double>)q.Quantity.Clone() });
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
    }
}
