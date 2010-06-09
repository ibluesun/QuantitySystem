using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using Microsoft.Scripting.Ast;
using QuantitySystem.Units;
using Qs.Types;

using Microsoft.Scripting.Utils;


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

        private string elementDeclaration;

        /// <summary>
        /// The original text that the element took when created.
        /// </summary>
        public string ElementDeclaration 
        {
            get
            {
                return elementDeclaration.Trim();
            }
            set
            {
                elementDeclaration = value;
            }
        }


        // I want to know if specific element will have to evaluate index or parameter
        // this information will be usefull later when planning the caching of the sequence.

        /// <summary>
        /// If true the element has an index that evaluated at runtime.
        /// </summary>
        public bool IndexEvaluation { get; set; }

        /// <summary>
        /// If true the element has one or more parameters that evaluated at runtime.
        /// </summary>
        public bool ParameterEvaluation { get; set; }


        /// <summary>
        /// Contains the DLR expression of the evaluation of this element.
        /// </summary>
        public Expression ElementExpression { get; set; }


        /// <summary>
        /// The value of the sequence element.
        /// - AnyQuantity &lt;double>
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
        /// Execute the element by accepting the index of execution.
        /// index of execution may differ on IndexInParentSequence.
        /// </summary>
        /// <param name="executionIndex">The real calling index.</param>
        /// <returns></returns>
        public QsValue Execute(int executionIndex)
        {

            if (ElementValue.GetType() == typeof(Func<int, QsValue>))
            {

                return ((Func<int, QsValue>)ElementValue)(executionIndex);
                
            }
            else if (ElementValue.GetType() == typeof(QsSequence))
            {
                QsSequence seq = (QsSequence)ElementValue;

                return (QsValue)seq.GetElementValue(executionIndex);
            }
            else
                return (QsValue)ElementValue;
        }

        public QsValue Execute(int executionIndex, params QsValue[] args)
        {
            if (ElementValue is QsValue)
                return (QsValue)ElementValue;
            switch (args.Length)
            {

                case 0:
                    return Execute(executionIndex);
                case 1:
                    return ((Func<int, QsValue, QsValue>)ElementValue)(executionIndex, args[0]);
                case 2:
                    return ((Func<int, QsValue, QsValue, QsValue>)ElementValue)(executionIndex, args[0], args[1]);
                case 3:
                    return ((Func<int, QsValue, QsValue, QsValue, QsValue>)ElementValue)(executionIndex, args[0], args[1], args[2]);
                case 4:
                    return ((Func<int, QsValue, QsValue, QsValue, QsValue, QsValue>)ElementValue)(executionIndex, args[0], args[1], args[2], args[3]);
                case 5:
                    return ((Func<int, QsValue, QsValue, QsValue, QsValue, QsValue, QsValue>)ElementValue)(executionIndex, args[0], args[1], args[2], args[3], args[4]);
                case 6:
                    return ((Func<int, QsValue, QsValue, QsValue, QsValue, QsValue, QsValue, QsValue>)ElementValue)(executionIndex, args[0], args[1], args[2], args[3], args[4], args[5]);
                case 7:
                    return ((Func<int, QsValue, QsValue, QsValue, QsValue, QsValue, QsValue, QsValue, QsValue>)ElementValue)(executionIndex, args[0], args[1], args[2], args[3], args[4], args[5], args[6]);

            }
            throw new QsException("Parameters Exeeded the limits");
        }


        public override string ToString()
        {
            return ElementDeclaration;
        }

        #region Helper Functions

        /// <summary>
        /// Creates element from quantity.
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static QsSequenceElement FromQuantity(QsValue quantity)
        {
            QsSequenceElement el = new QsSequenceElement();
            el.ElementValue = quantity;
            el.ElementDeclaration = quantity.ToString();

            el.IndexEvaluation = false;
            el.ParameterEvaluation = false;

            return el;

        }

        /// <summary>
        /// Creates element from sequence.
        /// The element will return the target sequence with the same index.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static QsSequenceElement FromSequenceAccess(QsSequence sequence)
        {

            QsSequenceElement el = new QsSequenceElement();
            el.ElementValue = sequence;

            el.IndexEvaluation = false;  
            el.ParameterEvaluation = false;

            return el;
        }

        /// <summary>
        /// Create element that point to delegate of one index and no parameters.
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public static QsSequenceElement FromDelegate(Func<int, QsValue> function)
        {
            QsSequenceElement el = new QsSequenceElement();
            el.ElementValue = function;

            return el;
        }

        /// <summary>
        /// Create an element that point to delegate of one index and one Parameter.
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public static QsSequenceElement FromDelegate(Func<int, QsValue, QsValue> function)
        {
            QsSequenceElement el = new QsSequenceElement();
            el.ElementValue = function;
            return el;
        }

        /// <summary>
        /// Two parameters.
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public static QsSequenceElement FromDelegate(Func<int, QsValue, QsValue, QsValue> function)
        {
            QsSequenceElement el = new QsSequenceElement();
            el.ElementValue = function;
            return el;
        }

        /// <summary>
        /// Three parameters.
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public static QsSequenceElement FromDelegate(Func<int, QsValue, QsValue, QsValue, QsValue> function)
        {
            QsSequenceElement el = new QsSequenceElement();
            el.ElementValue = function;

            return el;
        }


        /// <summary>
        /// Parse the element text and make the element point to delegate which evaluate the text if necessary.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static QsSequenceElement Parse(string element, QsEvaluator qse, QsSequence sequence)
        {
            if (string.IsNullOrEmpty(element)) throw new QsException("Can't create element from empty string.");
            
            //try direct quantity
            AnyQuantity<double> v;
            if (Unit.TryParseQuantity(element, out v))
            {
                var el = QsSequenceElement.FromQuantity(new QsScalar { NumericalQuantity = v });
                el.ElementDeclaration = element;

                el.IndexEvaluation = false;
                el.ParameterEvaluation = false;
                return el;
            }
            else
            {
                QsSequenceElement se = new QsSequenceElement();
                
                //try one index delegate without parameters
                //Create the lambda function that will pass the index and parameters to the expression.
                LambdaBuilder lb = Utils.Lambda(typeof(QsValue), "ElementValue");

                //add the index parameter
                lb.Parameter(typeof(int), sequence.SequenceIndexName);
                
                
                //find the index parameter in line to know if it will be evaluated or not
                if (element.IndexOf(sequence.SequenceIndexName) > -1)
                    se.IndexEvaluation = true;


                //make the sequence parameters.
                foreach (var seqParam in sequence.Parameters)
                {
                    lb.Parameter(typeof(QsValue), seqParam.Name);
                    if (element.IndexOf(seqParam.Name) > -1)
                        se.ParameterEvaluation = true;
                }

                QsVar pvar = new QsVar(qse, element, sequence, lb);

                lb.Body = pvar.ResultExpression;

                LambdaExpression le = lb.MakeLambda();

                se.ElementDeclaration = element;
                se.ElementExpression = pvar.ResultExpression;
                se.ElementValue = le.Compile();

                return se;

            }

            throw new QsException("Check me in sequence element :( :( :( ");
            
        }

        #endregion


    }
}
