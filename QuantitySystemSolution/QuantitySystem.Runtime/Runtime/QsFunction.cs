using System.Collections.Generic;
using System.Linq;
using Microsoft;
using Microsoft.Linq.Expressions;
using Microsoft.Scripting;
using Microsoft.Scripting.Ast;
using Microsoft.Scripting.Runtime;
using ParticleLexer;
using ParticleLexer.TokenTypes;
using QuantitySystem.Quantities.BaseQuantities;
using System.Globalization;


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

        /// <summary>
        /// Tells if the function has an evaluated body that can be invoked.
        /// </summary>
        public bool HasCode 
        {
            get
            {
                if (_FunctionDelegate == null) return false;
                else return true;
            }
        }



        public AnyQuantity<double> Invoke()
        {
            return FunctionDelegate_0();
        }
        public AnyQuantity<double> Invoke(params QsParameter[] args)
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


        /// <summary>
        /// Evaluate every argument and call the suitable function
        /// </summary>
        /// <param name="vario"></param>
        /// <returns></returns>
        public Expression GetInvokeExpression(QsVar vario, List<string> args)
        {
            List<Expression> parameters = new List<Expression>();

            for (int ip = 0; ip < args.Count(); ip++)
            {
                Expression nakedParameter;
                Expression rawParameter = Expression.Constant(args[ip]);

                if (this.Parameters[ip].Type == QsParamType.Function)
                {
                    //Handle to function.
                    nakedParameter = Expression.Constant(args[ip]);  // and I will postpone the evaluation untill we process the function.
                }
                else
                {
                    //normal variable
                    nakedParameter = vario.ParseArithmatic(args[ip]);
                }

                parameters.Add(Expression.Call(typeof(QsParameter).GetMethod("MakeParameter"), nakedParameter, rawParameter));

            }

            Expression DelegateProperty = Expression.Property(Expression.Constant(this), "FunctionDelegate_" + parameters.Count.ToString(CultureInfo.InvariantCulture));
            return Expression.Invoke(DelegateProperty, parameters);
        }


        public AnyQuantity<double> GetInvoke(params QsParameter[] parameters)
        {
            // args hold the hard coded value of the arguments that the function take.
            // parameters hold  the enclosed function parameters that will be sent this function
            // for example  V(x,c) = c(x/2)
            //   x as a parameter from parameters  which will be QsParameter.
            //   "x/2" is the hard coded text that passed to the 
            // we have to get the value of x into the "x/2" and evaluate
            // 


            List<QsParameter> ProcessedParameters = new List<QsParameter>();

            for (int ip = 0; ip < parameters.Count(); ip++)
            {
                QsParameter nakedParameter;

                if (this.Parameters[ip].Type == QsParamType.Function)
                {
                    //Handle to function.
                    if (parameters[ip].Value != null)
                    {
                        nakedParameter = QsParameter.MakeParameter(parameters[ip].Unknown, parameters[ip].RawValue);  // and I will postpone the evaluation untill we process the function.
                    }
                    else
                    {
                        //look for the raw value , this is the trick to keep the passed function name in the parameters if it wasn't evaluated

                        nakedParameter = QsParameter.MakeParameter(parameters[ip].RawValue, parameters[ip].RawValue);
                    }
                }
                else
                {

                    //normal variable
                    nakedParameter = QsParameter.MakeParameter(parameters[ip].Quantity, parameters[ip].RawValue);

                }

                ProcessedParameters.Add(nakedParameter);

            }

            return Invoke(ProcessedParameters.ToArray());
        }

        #region public delegate properties for the function with its number of parameters

        private object _FunctionDelegate;

        internal Func<AnyQuantity<double>> FunctionDelegate_0
        {
            get
            {
                return (Func<AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, AnyQuantity<double>> FunctionDelegate_1
        {
            get
            {
                return (Func<QsParameter, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, AnyQuantity<double>> FunctionDelegate_2
        {
            get
            {
                return (Func<QsParameter, QsParameter, AnyQuantity<double>>)_FunctionDelegate;
            }
        }


        internal Func<QsParameter, QsParameter, QsParameter, AnyQuantity<double>> FunctionDelegate_3
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>> FunctionDelegate_4
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>> FunctionDelegate_5
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>> FunctionDelegate_6
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>> FunctionDelegate_7
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>> FunctionDelegate_8
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>> FunctionDelegate_9
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>> FunctionDelegate_10
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>> FunctionDelegate_11
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>> FunctionDelegate_12
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, AnyQuantity<double>>)_FunctionDelegate;
            }
        }

        #endregion


        /// <summary>
        /// The function body text
        /// </summary>
        public string FunctionBody { get; private set; }



        /// <summary>
        /// Parameters of the functions.
        /// </summary>
        public QsParamInfo[] Parameters { get; set; }


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
            t = t.GroupBrackets();

            t = t.MergeTokens(new AssignmentOperatorToken());
            if (t[0].TokenType == typeof(WordToken)
                && (t.Count > 1 ? t[1].TokenType == typeof(ParenthesisGroupToken) : false)
                && (t.Count > 2 ? t[2].TokenType == typeof(AssignmentOperatorToken) : false))
            {
                //get function name
                // will be the first token
                string functionName = t[0].TokenValue;

                //get parameters
                QsParamInfo[] prms = (from c in t[1]
                                      where c.TokenType == typeof(WordToken)
                                      select new QsParamInfo { Name = c.TokenValue, Type = QsParamType.AnyQuantity }).ToArray();


                QsFunction qf = new QsFunction(function)
                {
                    FunctionName = functionName,
                    Parameters = prms
                };


                LambdaBuilder lb = Utils.Lambda(typeof(AnyQuantity<double>), functionName);


                foreach (QsParamInfo prm in prms)
                {
                    lb.Parameter(typeof(QsParameter), prm.Name);
                }

                List<Expression> statements = new List<Expression>();

                QsVar qv = new QsVar(qse, function.Substring(t[2].IndexInText + t[2].TokenValueLength), qf, lb);

                statements.Add(qv.ResultExpression);   //making the variable expression itself make it the return value of the function.

                lb.Body = Expression.Block(statements);

                LambdaExpression lbe = lb.MakeLambda();
                qf.FunctionExpression = lbe;


                return qf;

            }
            else
            {
                return null;
            }
        }


        public static QsFunction GetFunction(Scope scope, string realName)
        {
            object q;
            scope.TryGetName(SymbolTable.StringToId(realName), out q);
            return (QsFunction)q;
            
        }

        public static QsFunction GetFunctionAndThrowIfNotFound(Scope scope, string realName)
        {
            var f = GetFunction(scope, realName);
            if (f == null) throw new QsException("Function: '" + realName + "' Couldn't be found in global heap.");
            else return f;
        }


        public static string FormFunctionScopeName(string functionName, int paramCount)
        {
            return functionName + "#" + paramCount.ToString(CultureInfo.InvariantCulture);
        }


        #endregion



    }
}
