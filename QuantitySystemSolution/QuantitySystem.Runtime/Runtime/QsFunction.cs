using System.Collections.Generic;
using System.Linq;
using Microsoft.Linq.Expressions;
using Microsoft.Scripting.Ast;
using ParticleLexer;
using ParticleLexer.TokenTypes;
using QuantitySystem.Quantities.BaseQuantities;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;

namespace Qs.Runtime
{
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

        public LambdaExpression FunctionExpression { get; private set; }

        public QsFunctionCallSiteBinder FunctionCallSiteBinder
        {
            get
            {
                return new QsFunctionCallSiteBinder(FunctionExpression);
            }
        }


        public string Function { get; private set; }

        public string[] Parameters { get; set; }

        public QsFunction(string function)
        {
            Function = function;
        }

        public override string ToString()
        {

            return Function;
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

        public static LambdaExpression GetFunctionExpression(Scope scope, string fnName)
        {
            return GetFunction(scope, fnName).FunctionExpression;
        }

        #endregion



    }
}
