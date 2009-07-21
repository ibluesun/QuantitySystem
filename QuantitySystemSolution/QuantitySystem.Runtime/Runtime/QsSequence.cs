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
    /// Specs will be released soon.
    /// </summary>
    public class QsSequence : Dictionary<int, QsSequenceElement>
    {

        public string SequenceBody { get; private set; }


        /// <summary>
        /// The sequence must have element in zero index.
        /// </summary>
        /// <param name="zeroElement"></param>
        public QsSequence(QsSequenceElement zeroElement)
        {
            base[0] = zeroElement;
        }

        /// <summary>
        /// Correspones To: S[i]
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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
                value.ParentSequence = this;
                value.IndexInParentSequence = index;
                base[index] = value;
            }   
        }

        public QsSequenceElement GetElement(int index)
        {
            return this[index];
            
        }

        public AnyQuantity<double> GetElementQuantity(int index)
        {
            

            return (AnyQuantity<double>)GetElement(index).Execute(index);

        }

        /// <summary>
        /// The method sum all elements in the sequence between the supplied indexes.
        /// Correspondes To: S[i..k]
        /// </summary>
        public AnyQuantity<double> Sum(int fromIndex, int toIndex)
        {
            AnyQuantity<double> Total = GetElementQuantity(fromIndex);

            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementQuantity(i);
            }

            return Total;
        }

        public AnyQuantity<double> Mean(int fromIndex, int toIndex)
        {
            var tot = Sum(fromIndex, toIndex);
            var n = toIndex - fromIndex + 1;
            var count = Unit.ParseQuantity(n.ToString(CultureInfo.InvariantCulture));

            return tot / count;


        }

    }
}
