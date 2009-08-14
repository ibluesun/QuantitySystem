using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using System.Globalization;
using QuantitySystem.Units;
using Microsoft.Linq.Expressions;

namespace Qs.Runtime
{
    /// <summary>
    /// Single indexed sequence.
    /// </summary>
    public partial class QsSequence : Dictionary<int, QsSequenceElement> , IEnumerable<AnyQuantity<double>>
    {

        /// <summary>
        /// Minimum decalred index in the sequence.
        /// </summary>
        public int MinimumIndex { get; set; }

        /// <summary>
        /// Maximum declared index in the sequence.
        /// </summary>
        public int MaximumIndex { get; set; }


        /// <summary>
        /// Correspones To: S[i]
        /// </summary>
        /// <param name="index"></param>
        /// <returns><see cref="QsSequenceElement"/></returns>
        public new QsSequenceElement this[int index]
        {
            get
            {
                QsSequenceElement el;
                if (!this.TryGetValue(index, out el))
                {
                    //failed to get the value
                    // here the magic begins

                    if (index > 0)
                    {
                        int IX = index;
                        while (!this.ContainsKey(IX))
                        {
                            IX--;  //decrease the index.
                        }
                        //Bingo we found it
                        this.TryGetValue(IX, out el);
                    }
                    else if (index < 0)
                    {
                        int IX = index;
                        while (!this.ContainsKey(IX))
                        {
                            IX++;  //decrease the index.
                        }
                        //Bingo we found it
                        this.TryGetValue(IX, out el);
                    }
                    else
                    {
                        //zero and not found the in logical error or some one altered the zero element.
                        throw new QsException("Damn it. Where is the zero element");
                    }
                }
                return el;
            }
            set
            {
                if (index < MinimumIndex) MinimumIndex = index;
                if (index > MaximumIndex) MaximumIndex = index;

                value.ParentSequence = this;
                value.IndexInParentSequence = index;
                base[index] = value;

                //clear the cache
                CachedValues.Clear();
            }   
        }

        /// <summary>
        /// The same function as indexer.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public QsSequenceElement GetElement(int index)
        {
            return this[index];
        }



        private bool _CachingEnabled;
        /// <summary>
        /// If true the sequence ability to to be cached is available.
        /// </summary>
        public bool CachingEnabled
        {
            internal set
            {
                _CachingEnabled = value;

                if (value == false)
                    CachedValues.Clear(); //clear the previous cached values.
            }
            get
            {
                return _CachingEnabled;
            }
        }

        /// <summary>
        /// Keeps the the values that were calculated before.
        /// When modifieng an item the index of this item and all after items should be deleted.
        /// </summary>
        private Dictionary<int, AnyQuantity<double>> CachedValues = new Dictionary<int, AnyQuantity<double>>();


        /// <summary>
        /// Gets the element quantity and employ cach
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public AnyQuantity<double> GetElementQuantity(int index)
        {
            AnyQuantity<double> val;
            if (CachingEnabled)
            {
                if (CachedValues.TryGetValue(index, out val))
                {
                    return val;
                }
            }

            val = (AnyQuantity<double>)GetElement(index).Execute(index);

            if (CachingEnabled) CachedValues[index] = val;

            return val;

        }

        #region Get Element Quantity Functions
        public AnyQuantity<double> GetElementQuantity(int index, AnyQuantity<double> arg0)
        {
            AnyQuantity<double> val = (AnyQuantity<double>)GetElement(index).Execute(index, arg0);
            return val;
        }

        public AnyQuantity<double> GetElementQuantity(int index, AnyQuantity<double> arg0, AnyQuantity<double> arg1)
        {
            AnyQuantity<double> val = (AnyQuantity<double>)GetElement(index).Execute(index, arg0, arg1);
            return val;
        }

        public AnyQuantity<double> GetElementQuantity(int index, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2)
        {
            AnyQuantity<double> val = (AnyQuantity<double>)GetElement(index).Execute(index, arg0, arg1, arg2);
            return val;
        }

        public AnyQuantity<double> GetElementQuantity(int index, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2, AnyQuantity<double> arg3)
        {
            AnyQuantity<double> val = (AnyQuantity<double>)GetElement(index).Execute(index, arg0, arg1, arg2, arg3);
            return val;
        }

        public AnyQuantity<double> GetElementQuantity(int index, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2, AnyQuantity<double> arg3, AnyQuantity<double> arg4)
        {
            AnyQuantity<double> val = (AnyQuantity<double>)GetElement(index).Execute(index, arg0, arg1, arg2, arg3, arg4);
            return val;
        }

        public AnyQuantity<double> GetElementQuantity(int index, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2, AnyQuantity<double> arg3, AnyQuantity<double> arg4, AnyQuantity<double> arg5)
        {
            AnyQuantity<double> val = (AnyQuantity<double>)GetElement(index).Execute(index, arg0, arg1, arg2, arg3, arg4, arg5);
            return val;
        }

        public AnyQuantity<double> GetElementQuantity(int index, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2, AnyQuantity<double> arg3, AnyQuantity<double> arg4, AnyQuantity<double> arg5, AnyQuantity<double> arg6)
        {
            AnyQuantity<double> val = (AnyQuantity<double>)GetElement(index).Execute(index, arg1, arg1, arg2, arg3, arg4, arg5, arg6);
            return val;
        }

        #endregion
        
        /// <summary>
        /// The method sum all elements in the sequence between the supplied indexes.
        /// Correspondes To: S[i..k]
        /// </summary>
        public AnyQuantity<double> SumElements(int fromIndex, int toIndex)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex);

            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementQuantity(i);
            }

            return Total;
        }

        #region SumElements Functions
        public AnyQuantity<double> SumElements(int fromIndex, int toIndex, AnyQuantity<double> arg0)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex, arg0);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementQuantity(i, arg0);
            }
            return Total;
        }
        public AnyQuantity<double> SumElements(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex, arg0, arg1);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementQuantity(i, arg0, arg1);
            }
            return Total;
        }
        public AnyQuantity<double> SumElements(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex, arg0, arg1, arg2);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementQuantity(i, arg0, arg1, arg2);
            }
            return Total;
        }
        public AnyQuantity<double> SumElements(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2, AnyQuantity<double> arg3)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex, arg0, arg1, arg2, arg3);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementQuantity(i, arg0, arg1, arg2, arg3);
            }
            return Total;
        }
        public AnyQuantity<double> SumElements(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2, AnyQuantity<double> arg3, AnyQuantity<double> arg4)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex, arg0, arg1, arg2, arg3, arg4);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementQuantity(i, arg0, arg1, arg2, arg3, arg4);
            }
            return Total;
        }
        public AnyQuantity<double> SumElements(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2, AnyQuantity<double> arg3, AnyQuantity<double> arg4, AnyQuantity<double> arg5)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementQuantity(i, arg0, arg1, arg2, arg3, arg4, arg5);
            }
            return Total;
        }
        public AnyQuantity<double> SumElements(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2, AnyQuantity<double> arg3, AnyQuantity<double> arg4, AnyQuantity<double> arg5, AnyQuantity<double> arg6)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementQuantity(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            }
            return Total;
        }
        #endregion


        public AnyQuantity<double> Average(int fromIndex, int toIndex)
        {
            var tot = SumElements(fromIndex, toIndex);
            var n = toIndex - fromIndex + 1;
            var count = Qs.ToQuantity((double)n);
            return tot / count;
        }



        #region Average Functions
        public AnyQuantity<double> Average(int fromIndex, int toIndex, AnyQuantity<double> arg0)
        {
            var tot = SumElements(fromIndex, toIndex, arg0);
            var n = toIndex - fromIndex + 1;
            var count = Qs.ToQuantity((double)n);
            return tot / count;
        }
        public AnyQuantity<double> Average(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1)
        {
            var tot = SumElements(fromIndex, toIndex, arg0, arg1);
            var n = toIndex - fromIndex + 1;
            var count = Qs.ToQuantity((double)n);
            return tot / count;
        }
        public AnyQuantity<double> Average(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2)
        {
            var tot = SumElements(fromIndex, toIndex, arg0, arg1, arg2);
            var n = toIndex - fromIndex + 1;
            var count = Qs.ToQuantity((double)n);
            return tot / count;
        }
        public AnyQuantity<double> Average(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2, AnyQuantity<double> arg3)
        {
            var tot = SumElements(fromIndex, toIndex, arg0, arg1, arg2, arg3);
            var n = toIndex - fromIndex + 1;
            var count = Qs.ToQuantity((double)n);
            return tot / count;
        }
        public AnyQuantity<double> Average(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2, AnyQuantity<double> arg3, AnyQuantity<double> arg4)
        {
            var tot = SumElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4);
            var n = toIndex - fromIndex + 1;
            var count = Qs.ToQuantity((double)n);
            return tot / count;
        }
        public AnyQuantity<double> Average(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2, AnyQuantity<double> arg3, AnyQuantity<double> arg4, AnyQuantity<double> arg5)
        {
            var tot = SumElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            var n = toIndex - fromIndex + 1;
            var count = Qs.ToQuantity((double)n);
            return tot / count;
        }
        public AnyQuantity<double> Average(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2, AnyQuantity<double> arg3, AnyQuantity<double> arg4, AnyQuantity<double> arg5, AnyQuantity<double> arg6)
        {
            var tot = SumElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            var n = toIndex - fromIndex + 1;
            var count = Qs.ToQuantity((double)n);
            return tot / count;
        }
        #endregion



        /// <summary>
        /// The method multiply all elements in the sequence between the supplied indexes.
        /// Correspondes To: S[i..k]
        /// </summary>
        public AnyQuantity<double> MulElements(int fromIndex, int toIndex)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex);

            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementQuantity(i);
            }

            return Total;
        }

        #region MulElements Functions
        public AnyQuantity<double> MulElements(int fromIndex, int toIndex, AnyQuantity<double> arg0)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex, arg0);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementQuantity(i, arg0);
            }
            return Total;
        }
        public AnyQuantity<double> MulElements(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex, arg0, arg1);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementQuantity(i, arg0, arg1);
            }
            return Total;
        }
        public AnyQuantity<double> MulElements(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex, arg0, arg1, arg2);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementQuantity(i, arg0, arg1, arg2);
            }
            return Total;
        }
        public AnyQuantity<double> MulElements(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2, AnyQuantity<double> arg3)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex, arg0, arg1, arg2, arg3);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementQuantity(i, arg0, arg1, arg2, arg3);
            }
            return Total;
        }
        public AnyQuantity<double> MulElements(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2, AnyQuantity<double> arg3, AnyQuantity<double> arg4)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex, arg0, arg1, arg2, arg3, arg4);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementQuantity(i, arg0, arg1, arg2, arg3, arg4);
            }
            return Total;
        }
        public AnyQuantity<double> MulElements(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2, AnyQuantity<double> arg3, AnyQuantity<double> arg4, AnyQuantity<double> arg5)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementQuantity(i, arg0, arg1, arg2, arg3, arg4, arg5);
            }
            return Total;
        }
        public AnyQuantity<double> MulElements(int fromIndex, int toIndex, AnyQuantity<double> arg0, AnyQuantity<double> arg1, AnyQuantity<double> arg2, AnyQuantity<double> arg3, AnyQuantity<double> arg4, AnyQuantity<double> arg5, AnyQuantity<double> arg6)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementQuantity(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            }
            return Total;
        }
        #endregion



        #region IEnumerable<AnyQuantity<double>> Members


        /// <summary>
        /// Sequence begin from minimum declared index till the maximum number that can stored in double.
        /// </summary>
        /// <returns></returns>
        public new IEnumerator<AnyQuantity<double>> GetEnumerator()
        {
            int i = MinimumIndex;
            var q = this.GetElementQuantity(i);
            while (q.Value < double.MaxValue)
            {

                yield return q;

                i++;
                q = this.GetElementQuantity(i);

            }
        }

        #endregion
    }
}
