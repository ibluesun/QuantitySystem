using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using Microsoft.Linq.Expressions;
using Microsoft.Scripting.Ast;
using QuantitySystem.Units;

namespace Qs.Runtime
{

    /// <summary>
    /// Like Variant in vb in old days :)
    /// AnyQuantity
    /// Function
    /// Sequence
    /// </summary>
    public class QsSequenceElement
    {

        public object ElementValue { get; set; }


        /// <summary>
        /// The parent sequence that hold this element.
        /// </summary>
        public QsSequence ParentSequence { get; set; }

        /// <summary>
        /// This element index in the parent sequence.
        /// This way every element know its position in the parent sequence.
        /// </summary>
        public int IndexInParentSequence { get; set; }


        

        /// <summary>
        /// Execute the element and passing it the index on which execution should work on.
        /// </summary>
        /// <param name="executeIndex"></param>
        /// <returns></returns>
        public AnyQuantity<double> Execute(int executeIndex)
        {
            
            if (ElementValue.GetType() == typeof(Microsoft.Func<int, AnyQuantity<double>>))
            {

                return ((Microsoft.Func<int, AnyQuantity<double>>)ElementValue)(executeIndex);
                
            }
            else if (ElementValue.GetType() == typeof(QsSequence))
            {
                QsSequence seq = (QsSequence)ElementValue;

                return seq.GetElementQuantity(executeIndex);
            }
            else
                return (AnyQuantity<double>)ElementValue;
        }


        public static QsSequenceElement FromQuantity(AnyQuantity<double> quantity)
        {
            QsSequenceElement el = new QsSequenceElement();
            el.ElementValue = quantity;

            return el;

        }

        public static QsSequenceElement FromSequenceAccess(QsSequence sequence)
        {

            QsSequenceElement el = new QsSequenceElement();
            el.ElementValue = sequence;

            return el;

        }


        public static QsSequenceElement FromDelegate(Microsoft.Func<int, AnyQuantity<double>> function)
        {

            QsSequenceElement el = new QsSequenceElement();
            el.ElementValue = function;

            return el;

            
        }
    }
}
