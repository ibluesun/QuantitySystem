using System.Collections.Generic;
using System.Linq;
using Microsoft.Linq.Expressions;
using Microsoft.Scripting.Ast;
using ParticleLexer;
using ParticleLexer.TokenTypes;
using QuantitySystem.Quantities.BaseQuantities;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;
using Microsoft;

namespace Qs.Runtime
{
    /// <summary>
    /// Function that declared in Qs
    /// </summary>
    public class QsFunction
    {
        private string functionName;

        public string FunctionName { 
            get
            {
                //the function name should be unique for the sake of multiple variables and not to hide 
                //   other variables.

                return functionName + "#" + Parameters.Length.ToString();
            }
            private set
            {
                functionName = value;
            }
        }


        private LambdaExpression _FunctionExpression;

        public LambdaExpression FunctionExpression
        {
            get
            {
                return _FunctionExpression;
            }
            private set
            {
                _FunctionExpression = value;
                _FunctionDelegate = value.Compile();

            }
        }

        #region public delegate properties for the function with its number of parameters

        public AnyQuantity<double> Invoke()
        {
            return FunctionDelegate_0();
        }
        public AnyQuantity<double> Invoke(params AnyQuantity<double>[] args)
        {
            switch (args.Length)
            {
                case 0:
                    return FunctionDelegate_0();
                case 1:
                    return FunctionDelegate_1(args[0]);
                case 2:
                    return FunctionDelegate_2(args[0], args[1]);
                case 3:
                    return FunctionDelegate_3(args[0], args[1], args[2]);
                case 4:
                    return FunctionDelegate_4(args[0], args[1], args[2], args[3]);
                case 5:
                    return FunctionDelegate_5(args[0], args[1], args[2], args[3], args[4]);
                case 6:
                    return FunctionDelegate_6(args[0], args[1], args[2], args[3], args[4], args[5]);
                case 7:
                    return FunctionDelegate_7(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                case 8:
                    return FunctionDelegate_8(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                case 9:
                    return FunctionDelegate_9(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                case 10:
                    return FunctionDelegate_10(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                case 11:
                    return FunctionDelegate_11(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]);
                case 12:
                    return FunctionDelegate_12(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]);

                default:
                    throw new QsInvalidInputException("Function with required parameters");
            }
        }



        private object _FunctionDelegate;

        internal Func<AnyQuantity<double>> FunctionDelegate_0
        {
            get
            {
                return (Func<AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<AnyQuantity<double>, AnyQuantity<double>> FunctionDelegate_1
        {
            get
            {
                return (Func<AnyQuantity<double>, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>> FunctionDelegate_2
        {
            get
            {
                return (Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)_FunctionDelegate;
            }
        }


        internal Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>> FunctionDelegate_3
        {
            get
            {
                return (Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>> FunctionDelegate_4
        {
            get
            {
                return (Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>> FunctionDelegate_5
        {
            get
            {
                return (Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>> FunctionDelegate_6
        {
            get
            {
                return (Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>> FunctionDelegate_7
        {
            get
            {
                return (Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>> FunctionDelegate_8
        {
            get
            {
                return (Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>> FunctionDelegate_9
        {
            get
            {
                return (Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>> FunctionDelegate_10
        {
            get
            {
                return (Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>> FunctionDelegate_11
        {
            get
            {
                return (Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>> FunctionDelegate_12
        {
            get
            {
                return (Func<AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>, AnyQuantity<double>>)_FunctionDelegate;
            }
        }


        #endregion



        public string FunctionBody { get; private set; }

        public string[] Parameters { get; set; }

        public QsFunction(string function)
        {
            FunctionBody = function;            
        }

        public override string ToString()
        {

            return FunctionBody;
        }

        #region Helper Functions
        public static QsFunction ParseFunction(QsEvaluator qse, string function)
        {
            Token t = Token.ParseText(function);
            t = t.RemoveSpaceTokens();
            t = t.MergeTokens(new WordToken());
            t = t.MergeTokens(new NumberToken());
            t = t.MergeTokens(new UnitizedNumberToken());
            t = t.GroupParenthesis();

            t = t.MergeTokens(new AssignmentOperator());
            if (t[0].TokenType == typeof(WordToken)
                && (t.Count > 1 ? t[1].TokenType == typeof(GroupToken) : false)
                && (t.Count > 2 ? t[2].TokenType == typeof(AssignmentOperator) : false))
            {
                //get function name
                // will be the first token
                string functionName = t[0].TokenValue;

                //get parameters
                string[] prms = (from c in t[1]
                                 where c.TokenType == typeof(WordToken)
                                 select c.TokenValue).ToArray();

                //get expression of the body.


                //the line contains => lambda expression

                // we should separate between left and right(body)



                LambdaBuilder lb = Utils.Lambda(typeof(AnyQuantity<double>), functionName);


                foreach (string prm in prms)
                {
                    lb.Parameter(typeof(AnyQuantity<double>), prm);
                }

                List<Expression> statements = new List<Expression>();
                Expression var = lb.Variable(typeof(AnyQuantity<double>), "tempo"); //create variable in lambda

                QsVar qv = new QsVar(qse, function.Substring(t[2].IndexInText + t[2].TokenValueLength), lb);

                statements.Add(

                    Expression.Assign(
                        var,
                        qv.ResultExpression
                        )

                    );

                statements.Add(var);   //making the variable expression itself make it the return value of the function.


                lb.Body = Expression.Block(statements);

                LambdaExpression lbe = lb.MakeLambda();
                

                QsFunction qf = new QsFunction(function)
                {
                    FunctionName = functionName,
                    FunctionExpression = lbe,
                    Parameters = prms

                };

                return qf;

            }
            else
            {
                return null;
            }
        }


        public static QsFunction GetFunction(Scope scope, string fnName)
        {
            object q;


            scope.TryGetName(SymbolTable.StringToId(fnName), out q);


            return (QsFunction)q;

        }

        #endregion



    }
}
