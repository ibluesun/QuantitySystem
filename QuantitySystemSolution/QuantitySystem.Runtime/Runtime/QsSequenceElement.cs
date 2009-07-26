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

        public string ElementDeclaration { get; set; }

        /// <summary>
        /// The value of the sequence element.
        /// - AnyQuantity%lt;double>
        /// - Another Sequence
        /// - Delegate to code passing index.
        /// - Delegate to code passing index and parameters.
        /// </summary>
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

        public AnyQuantity<double> Execute(int executeIndex, params AnyQuantity<double>[] args)
        {
            switch (args.Length)
            {

                case 0:
                    return Execute(executeIndex);
                case 1:
                    return ((Microsoft.Func<int, AnyQuantity<double>, AnyQuantity<double>>)ElementValue)(executeIndex, args[0]);
                case 2:
                    return ((Microsoft.Func<int, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)ElementValue)(executeIndex, args[0], args[1]);
                case 3:
                    return ((Microsoft.Func<int, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)ElementValue)(executeIndex, args[0], args[1], args[2]);
                case 4:
                    return ((Microsoft.Func<int, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)ElementValue)(executeIndex, args[0], args[1], args[2], args[3]);
                case 5:
                    return ((Microsoft.Func<int, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)ElementValue)(executeIndex, args[0], args[1], args[2], args[3], args[4]);
                case 6:
                    return ((Microsoft.Func<int, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)ElementValue)(executeIndex, args[0], args[1], args[2], args[3], args[4], args[5]);
                case 7:
                    return ((Microsoft.Func<int, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)ElementValue)(executeIndex, args[0], args[1], args[2], args[3], args[4], args[5], args[6]);

            }
            throw new QsException("Parameters Exeeded the limits");
        }




        #region Helper Functions
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

        /// <summary>
        /// With index to 
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public static QsSequenceElement FromDelegate(Microsoft.Func<int, AnyQuantity<double>> function)
        {
            QsSequenceElement el = new QsSequenceElement();
            el.ElementValue = function;

            return el;
        }

        public static QsSequenceElement FromDelegate(Microsoft.Func<int, AnyQuantity<double>, AnyQuantity<double>> function)
        {
            QsSequenceElement el = new QsSequenceElement();
            el.ElementValue = function;

            return el;
        }

        public static QsSequenceElement FromDelegate(Microsoft.Func<int, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>> function)
        {
            QsSequenceElement el = new QsSequenceElement();
            el.ElementValue = function;

            return el;
        }

        public static QsSequenceElement FromDelegate(Microsoft.Func<int, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>> function)
        {
            QsSequenceElement el = new QsSequenceElement();
            el.ElementValue = function;

            return el;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static QsSequenceElement Parse(string element, QsEvaluator qse, QsSequence sequence)
        {
            
            //try direct quantity
            AnyQuantity<double> v;
            if (Unit.TryParseQuantity(element, out v))
            {
                var el = QsSequenceElement.FromQuantity(v);
                el.ElementDeclaration = element;
                return el;
            }
            else
            {
                //try one index delegate without parameters
                

                LambdaBuilder lb = Utils.Lambda(typeof(AnyQuantity<double>), "ElementValue");

                //make the index parameter
                lb.Parameter(typeof(int), sequence.SequenceIndexName);

                //make the sequence parameters.
                foreach (string seqParam in sequence.SequenceParameters)
                    lb.Parameter(typeof(AnyQuantity<double>), seqParam);

                QsVar pvar = new QsVar(qse, element, sequence, lb);

                lb.Body = pvar.ResultExpression;

                LambdaExpression le = lb.MakeLambda();

                QsSequenceElement se = new QsSequenceElement();
                se.ElementDeclaration = element;
                se.ElementValue = le.Compile();

                return se;

            }

            throw new Exception("Check me :( :( :( ");
            
        }

        #endregion

    }
}
