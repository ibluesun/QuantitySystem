﻿using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using System.Globalization;
using QuantitySystem.Units;
using Microsoft.Linq.Expressions;
using Qs.RuntimeTypes;

namespace Qs.Runtime
{
    /// <summary>
    /// Single indexed sequence.
    /// </summary>
    public partial class QsSequence : Dictionary<int, QsSequenceElement> , IEnumerable<QsValue>
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
        private Dictionary<int, QsValue> CachedValues = new Dictionary<int, QsValue>();


        /// <summary>
        /// Gets the element quantity and employ cach
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public QsValue GetElementValue(int index)
        {
            QsValue val;
            if (CachingEnabled)
            {
                if (CachedValues.TryGetValue(index, out val))
                {
                    return val;
                }
            }

            val = (QsValue)GetElement(index).Execute(index);

            if (CachingEnabled) CachedValues[index] = val;

            return val;

        }

        #region Get Element Quantity Functions
        public QsValue GetElementValue(int index, QsValue arg0)
        {
            QsValue val = (QsValue)GetElement(index).Execute(index, arg0);
            return val;
        }

        public QsValue GetElementValue(int index, QsValue arg0, QsValue arg1)
        {
            QsValue val = (QsValue)GetElement(index).Execute(index, arg0, arg1);
            return val;
        }

        public QsValue GetElementValue(int index, QsValue arg0, QsValue arg1, QsValue arg2)
        {
            QsValue val = (QsValue)GetElement(index).Execute(index, arg0, arg1, arg2);
            return val;
        }

        public QsValue GetElementValue(int index, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3)
        {
            QsValue val = (QsValue)GetElement(index).Execute(index, arg0, arg1, arg2, arg3);
            return val;
        }

        public QsValue GetElementValue(int index, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4)
        {
            QsValue val = (QsValue)GetElement(index).Execute(index, arg0, arg1, arg2, arg3, arg4);
            return val;
        }

        public QsValue GetElementValue(int index, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5)
        {
            QsValue val = (QsValue)GetElement(index).Execute(index, arg0, arg1, arg2, arg3, arg4, arg5);
            return val;
        }

        public QsValue GetElementValue(int index, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5, QsValue arg6)
        {
            QsValue val = (QsValue)GetElement(index).Execute(index, arg1, arg1, arg2, arg3, arg4, arg5, arg6);
            return val;
        }

        #endregion


        #region Higher Sequence Manipulation functions (Summation, Multiplication, and Average).

        /// <summary>
        /// The method sum all elements in the sequence between the supplied indexes.
        /// Correspondes To: S[i++k]
        /// </summary>
        public QsValue SumElements(int fromIndex, int toIndex)
        {
            QsValue Total = GetElementValue(fromIndex);

            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i);
            }

            return Total;
        }

        #region SumElements Functions
        public QsValue SumElements(int fromIndex, int toIndex, QsValue arg0)
        {
            QsValue Total = GetElementValue(fromIndex, arg0);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0);
            }
            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1)
        {
            QsValue Total = GetElementValue(fromIndex, arg0, arg1);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1);
            }
            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2)
        {
            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1, arg2);
            }
            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3)
        {
            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1, arg2, arg3);
            }
            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4)
        {
            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1, arg2, arg3, arg4);
            }
            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5)
        {
            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5);
            }
            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5, QsValue arg6)
        {
            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            }
            return Total;
        }
        #endregion

        /// <summary>
        /// Take the average of the sequence.
        /// Corresponds To: S[i!!k]
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        /// <returns></returns>
        public QsValue Average(int fromIndex, int toIndex)
        {
            var tot = SumElements(fromIndex, toIndex);
            var n = toIndex - fromIndex + 1;
            var count = new QsScalar { Quantity = Qs.ToQuantity((double)n) };
            return tot / count;
        }



        #region Average Functions
        public QsValue Average(int fromIndex, int toIndex, QsValue arg0)
        {
            var tot = SumElements(fromIndex, toIndex, arg0);
            var n = toIndex - fromIndex + 1;
            var count = new QsScalar { Quantity = Qs.ToQuantity((double)n) };
            return tot / count;
        }
        public QsValue Average(int fromIndex, int toIndex, QsValue arg0, QsValue arg1)
        {
            var tot = SumElements(fromIndex, toIndex, arg0, arg1);
            var n = toIndex - fromIndex + 1;
            var count = new QsScalar { Quantity = Qs.ToQuantity((double)n) };
            return tot / count;
        }
        public QsValue Average(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2)
        {
            var tot = SumElements(fromIndex, toIndex, arg0, arg1, arg2);
            var n = toIndex - fromIndex + 1;
            var count = new QsScalar { Quantity = Qs.ToQuantity((double)n) };
            return tot / count;
        }
        public QsValue Average(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3)
        {
            var tot = SumElements(fromIndex, toIndex, arg0, arg1, arg2, arg3);
            var n = toIndex - fromIndex + 1;
            var count = new QsScalar { Quantity = Qs.ToQuantity((double)n) };
            return tot / count;
        }
        public QsValue Average(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4)
        {
            var tot = SumElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4);
            var n = toIndex - fromIndex + 1;
            var count = new QsScalar { Quantity = Qs.ToQuantity((double)n) };
            return tot / count;
        }
        public QsValue Average(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5)
        {
            var tot = SumElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            var n = toIndex - fromIndex + 1;
            var count = new QsScalar { Quantity = Qs.ToQuantity((double)n) };
            return tot / count;
        }
        public QsValue Average(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5, QsValue arg6)
        {
            var tot = SumElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            var n = toIndex - fromIndex + 1;
            var count = new QsScalar { Quantity = Qs.ToQuantity((double)n) };
            return tot / count;
        }
        #endregion



        /// <summary>
        /// The method multiply all elements in the sequence between the supplied indexes.
        /// Correspondes To: S[i**k]
        /// </summary>
        public QsValue MulElements(int fromIndex, int toIndex)
        {
            QsValue Total = GetElementValue(fromIndex);

            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i);
            }

            return Total;
        }

        #region MulElements Functions
        public QsValue MulElements(int fromIndex, int toIndex, QsValue arg0)
        {
            QsValue Total = GetElementValue(fromIndex, arg0);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0);
            }
            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1)
        {
            QsValue Total = GetElementValue(fromIndex, arg0, arg1);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1);
            }
            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2)
        {
            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1, arg2);
            }
            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3)
        {
            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1, arg2, arg3);
            }
            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4)
        {
            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1, arg2, arg3, arg4);
            }
            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5)
        {
            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5);
            }
            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5, QsValue arg6)
        {
            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            }
            return Total;
        }
        #endregion



        /// <summary>
        /// This is a tricky functions
        /// it returns Vector if components are Scalars.
        /// Matrix if components are Vectors
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        /// <returns></returns>
        public QsValue QsValueElements(int fromIndex, int toIndex)
        {
            QsValue firstElement = GetElementValue(fromIndex);
            if (firstElement is QsScalar)
            {
                //return vector
                QsVector Total = new QsVector(toIndex - fromIndex + 1);

                Total.AddComponent((QsScalar)firstElement);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total.AddComponent((QsScalar)GetElementValue(i));
                }

                return Total;
            }
            else if(firstElement is QsVector)
            {
                //return vector
                QsMatrix Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total.AddVector((QsVector)GetElementValue(i));
                }

                return Total;
            }
            else if (firstElement is QsMatrix)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        #region QsValueElements Functions
        public QsValue QsValueElements(int fromIndex, int toIndex, QsValue arg0)
        {
            QsValue firstElement = GetElementValue(fromIndex, arg0);
            if (firstElement is QsScalar)
            {
                //return vector
                QsVector Total = new QsVector(toIndex - fromIndex + 1);

                Total.AddComponent((QsScalar)firstElement);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total.AddComponent((QsScalar)GetElementValue(i, arg0));
                }

                return Total;
            }
            else if (firstElement is QsVector)
            {
                //return vector
                QsMatrix Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total.AddVector((QsVector)GetElementValue(i, arg0));
                }

                return Total;
            }
            else if (firstElement is QsMatrix)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        public QsValue QsValueElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1)
        {
            QsValue firstElement = GetElementValue(fromIndex, arg0, arg1);
            if (firstElement is QsScalar)
            {
                //return vector
                QsVector Total = new QsVector(toIndex - fromIndex + 1);

                Total.AddComponent((QsScalar)firstElement);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1));
                }

                return Total;
            }
            else if (firstElement is QsVector)
            {
                //return vector
                QsMatrix Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total.AddVector((QsVector)GetElementValue(i, arg0, arg1));
                }

                return Total;
            }
            else if (firstElement is QsMatrix)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        public QsValue QsValueElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2)
        {
            QsValue firstElement = GetElementValue(fromIndex, arg0, arg1, arg2);
            if (firstElement is QsScalar)
            {
                //return vector
                QsVector Total = new QsVector(toIndex - fromIndex + 1);

                Total.AddComponent((QsScalar)firstElement);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2));
                }

                return Total;
            }
            else if (firstElement is QsVector)
            {
                //return vector
                QsMatrix Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2));
                }

                return Total;
            }
            else if (firstElement is QsMatrix)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        public QsValue QsValueElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3)
        {
            QsValue firstElement = GetElementValue(fromIndex, arg0, arg1, arg2, arg3);
            if (firstElement is QsScalar)
            {
                //return vector
                QsVector Total = new QsVector(toIndex - fromIndex + 1);

                Total.AddComponent((QsScalar)firstElement);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3));
                }

                return Total;
            }
            else if (firstElement is QsVector)
            {
                //return vector
                QsMatrix Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3));
                }

                return Total;
            }
            else if (firstElement is QsMatrix)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        public QsValue QsValueElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4)
        {
            QsValue firstElement = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4);
            if (firstElement is QsScalar)
            {
                //return vector
                QsVector Total = new QsVector(toIndex - fromIndex + 1);

                Total.AddComponent((QsScalar)firstElement);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3, arg4));
                }

                return Total;
            }
            else if (firstElement is QsVector)
            {
                //return vector
                QsMatrix Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3, arg4));
                }

                return Total;
            }
            else if (firstElement is QsMatrix)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        public QsValue QsValueElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5)
        {
            QsValue firstElement = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            if (firstElement is QsScalar)
            {
                //return vector
                QsVector Total = new QsVector(toIndex - fromIndex + 1);

                Total.AddComponent((QsScalar)firstElement);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5));
                }

                return Total;
            }
            else if (firstElement is QsVector)
            {
                //return vector
                QsMatrix Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5));
                }

                return Total;
            }
            else if (firstElement is QsMatrix)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        public QsValue QsValueElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5, QsValue arg6)
        {
            QsValue firstElement = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            if (firstElement is QsScalar)
            {
                //return vector
                QsVector Total = new QsVector(toIndex - fromIndex + 1);

                Total.AddComponent((QsScalar)firstElement);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6));
                }

                return Total;
            }
            else if (firstElement is QsVector)
            {
                //return vector
                QsMatrix Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6));
                }

                return Total;
            }
            else if (firstElement is QsMatrix)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        
        
        #endregion

        #endregion


        #region IEnumerable<QsValue> Members


        /// <summary>
        /// Sequence begin from minimum declared index till the maximum number that can stored in double.
        /// </summary>
        /// <returns></returns>
        public new IEnumerator<QsValue> GetEnumerator()
        {
            int i = MinimumIndex;
            var q = this.GetElementValue(i);
            while (((QsScalar)q).Quantity.Value < double.MaxValue)
            {

                yield return q;

                i++;
                q = this.GetElementValue(i);

            }
        }

        #endregion
    }
}
