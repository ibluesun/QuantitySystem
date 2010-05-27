using System;
using System.Collections.Generic;
using System.Globalization;
using Qs.Types;

namespace Qs.Runtime
{
    /// <summary>
    /// Single indexed sequence.
    /// </summary>
    public partial class QsSequence : SortedList<int, QsSequenceElement> , IEnumerable<QsValue>
    {

        /// <summary>
        /// if the function has a namespace then the value of it is here.
        /// </summary>
        public string SequenceNamespace { get; set; }

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

            
            if (this.Parameters.Length > 0)
            {
                // this is a call to form symbolic element
                // like g[n](x) ..> x^n
                // and calling g[2] 
                //  the output should be x^2
                //  and be parsed into function  (QsFunction)
                var e = GetElement(index);

                var FunctionBody= e.ElementDeclaration.Replace(this.SequenceIndexName, index.ToString(CultureInfo.InvariantCulture));

                string porma = string.Empty;  // the parameters separated by comma ','
                foreach (var prm in this.Parameters)
                {
                    porma += prm.Name + ", ";
                }
                porma = porma.TrimEnd(',', ' ');

                string FunctionDeclaration = "_(" + porma + ") = " + FunctionBody;

                QsFunction qs = QsFunction.ParseFunction(QsEvaluator.CurrentEvaluator, FunctionDeclaration);
                return qs;
            }
            else
            {
                val = GetElement(index).Execute(index);
            }

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




        #region IEnumerable<QsValue> Members


        /// <summary>
        /// Sequence begin from minimum declared index till the maximum number that can stored in double.
        /// </summary>
        /// <returns></returns>
        public new IEnumerator<QsValue> GetEnumerator()
        {
            int i = MinimumIndex;
            var q = this.GetElementValue(i);
            while (((QsScalar)q).NumericalQuantity.Value < double.MaxValue)
            {

                yield return (QsValue)q;

                i++;
                q = this.GetElementValue(i);

            }
        }

        #endregion
    }
}
