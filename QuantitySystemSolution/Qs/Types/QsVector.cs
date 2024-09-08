using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using Qs.Runtime;
using System.Xml.Schema;

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

            ClearCache();
        }

        /// <summary>
        /// Concatenate the scalars to the current vector
        /// </summary>
        /// <param name="scalars"></param>
        public void AddComponents(params QsScalar[] scalars)
        {
            ListStorage.AddRange(scalars);

            ClearCache();
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
            for (int i = 1; i < ListStorage.Count; i++) total = total + this[i];
            return total;
        }

        public QsScalar Mean()
        {   
            var total  = this.Sum().DivideScalar(Count.ToQuantity().ToScalar());
            return total;
        }

        QsVector _AscendedVector = null;

        public QsVector AscendedVecor
        {
            get
            {
                if (_AscendedVector == null)
                {
                    var ordered = ListStorage.OrderBy(v => v, new QsValueComparer());

                    _AscendedVector = new QsVector(ordered.ToArray());

                }
                return _AscendedVector;
            }

        }

        QsVector _DescendedVector = null;
        public QsVector DescendedVector
        {
            get
            {
                if (_DescendedVector == null)
                {
                    var ordered = ListStorage.OrderByDescending(v => v, new QsValueComparer());

                    _DescendedVector = new QsVector(ordered.ToArray());

                }
                return _DescendedVector;
            }
        }

        /// <summary>
        /// arranged data in ascending order .. it gives the middle value of odd elements count or the average of the two middle values of even elements count
        /// </summary>
        /// <returns></returns>
        public QsScalar Median()
        {
            var ao = AscendedVecor;
            Math.DivRem(ao.Count, 2, out var rm);
            if (rm == 0)
            {
                // even number  so we take the average of the inner ones  
                var aoc = ao.Count / 2;

                var twos = ao[aoc - 1].AddScalar(ao[aoc]);

                var result = twos.DivideScalar(((double)2).ToQuantity().ToScalar());
                return result;
            }
            else
            {
                var aoc = (int)Math.Floor(ao.Count / 2.0);

                var result = ao[aoc];

                return result;

            }
        }


        /// <summary>
        /// Minimum and maximum values of the vector
        /// </summary>
        /// <returns></returns>
        public QsFlowingTuple Range()
        {
            QsFlowingTuple q = new QsFlowingTuple();
            q.AddTupleValue(AscendedVecor[0]);
            q.AddTupleValue(AscendedVecor[AscendedVecor.Count - 1]);
            return q;
        }


        QsValue _Mode = null;

        public QsFlowingTuple Frequency()
        {

            List<(QsValue, int)> values = new List<(QsValue, int)>();

            
            for (int ix = 0; ix < AscendedVecor.Count; ix++)
            {
                int ix_freq = 1;

                bool last = true;
                for (int iy = ix + 1; iy < AscendedVecor.Count; iy++)
                {
                    if (AscendedVecor[ix].Equality(AscendedVecor[iy]))
                        ix_freq++;
                    else
                    {
                        
                        values.Add((AscendedVecor[ix], ix_freq));
                        

                        ix = iy - 1;
                        
                        last = false;
                        break;
                    }
                }

                if (last)
                {
                    values.Add((AscendedVecor[ix], ix_freq));
                    break;
                }
            }

            var vvdodesc = values.OrderByDescending(v => v.Item2).ToArray();

            if (vvdodesc.Length == 0) _Mode = new QsText("No Mode");
            if (vvdodesc.Length == 1) _Mode = vvdodesc[0].Item1;

            if (vvdodesc.Length == 2 && vvdodesc[0].Item2 > vvdodesc[1].Item2)
                _Mode = vvdodesc[0].Item1;
            else
                _Mode = new QsText("No Mode");

            if (vvdodesc.Length > 2)
            {
                if (vvdodesc[0].Item2 == vvdodesc[1].Item2 &&  vvdodesc[1].Item2 == vvdodesc[2].Item2) _Mode = new QsText("No Mode");
                else if (vvdodesc[0].Item2 == vvdodesc[1].Item2) _Mode = new QsText($"Bi-Modal ({vvdodesc[0].Item1.ToShortString()}, {vvdodesc[1].Item1.ToShortString()})");
                else if (vvdodesc[0].Item2 > vvdodesc[1].Item2) _Mode = vvdodesc[0].Item1;
                else
                    _Mode = new QsText("No Mode");

                
            }
            

            


            var vvd = vvdodesc.Select(x => new QsTupleValue(x.Item1.ToValueString(), x.Item2.ToScalarValue())).ToArray();
            return new QsFlowingTuple(vvd);
        }


        public QsValue Mode()
        {
            if (_Mode == null)
                Frequency();

            return _Mode;
            
        }

        #region Vector behaviour
        public int Count => ListStorage.Count;
        

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


        protected override void OnClearCache()
        {
            _AscendedVector = null;
            _DescendedVector = null;
            _Mode = null;
        }
    }
}
