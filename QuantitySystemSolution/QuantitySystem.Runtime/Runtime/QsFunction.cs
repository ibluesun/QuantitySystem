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
using Qs.RuntimeTypes;
using System.Reflection;
using System.Reflection.Emit;


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

        /// <summary>
        /// if the function has a namespace then the value of it is here.
        /// </summary>
        public string FunctionNamespace { get; set; }


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

        /// <summary>
        /// Level one function calling with direct Quantities.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public AnyQuantity<double> Invoke(params AnyQuantity<double>[] args)
        {
            var parms = (from arg in args
                         select 
                         QsParameter.MakeParameter(arg.ToScalarValue(), arg.ToShortString())).ToArray();

            return ((QsScalar)Invoke(parms)).Quantity;
        }

        /// <summary>
        /// Level two function calling with Qs Values 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public QsValue Invoke(params QsValue[] args)
        {
            var parms = (from arg in args
                         select
                         QsParameter.MakeParameter(arg, arg.ToString())).ToArray();

            return (QsScalar)Invoke(parms);
            
        }

        /// <summary>
        /// Invoke parameterless function
        /// </summary>
        /// <returns></returns>
        public AnyQuantity<double> Invoke()
        {
            return ((QsScalar)FunctionDelegate_0()).Quantity;
        }

        /// <summary>
        /// Invoke function with any number of parameters up to 12 parameter
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public QsValue Invoke(params QsParameter[] args)
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
                    throw new QsInvalidInputException("Function arguments exceed the library limit {12}");
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

                if (this.Parameters[ip].Type == QsParamType.Function) //is this parameter in declaration is pointing to function handle
                {
                    //yes: treat this parameter as function handle
                    // get the argument as a string value to be used after that as a function name.
                    nakedParameter = Expression.Constant(args[ip]);  // and I will postpone the evaluation until we process the function.

                    //expression to make parameter from the corresponding naked parameter
                    parameters.Add(Expression.Call(typeof(QsParameter).GetMethod("MakeParameter"),
                        nakedParameter,
                        rawParameter));
                }
                else
                {
                    //normal variable
                    
                    nakedParameter = vario.ParseArithmatic(args[ip]);

                    // if this was another function name without like  v(c,g,8)  where g is a function name will be passed to c
                    //  then excpetion will occur in GetQuantity that variable was not found in the scope

                    Expression tryBody = Expression.Call(typeof(QsParameter).GetMethod("MakeParameter"),
                        nakedParameter,
                        rawParameter);

                    Expression catchBody = Expression.Call(typeof(QsParameter).GetMethod("MakeParameter"),
                        Expression.Constant(null),
                        rawParameter);

                    // The try catch block when catch exception will execute the call but by passing the parameter as text only
                    var tt = Utils.Try(tryBody);
                    //tt.Catch(typeof(QsVariableNotFoundException), catchBody);
                    tt.Catch(typeof(System.Exception), catchBody);

                    

                    parameters.Add(tt.ToExpression());
                }


            }

            Expression DelegateProperty = Expression.Property(Expression.Constant(this), "FunctionDelegate_" + parameters.Count.ToString(CultureInfo.InvariantCulture));
            return Expression.Invoke(DelegateProperty, parameters);
        }


        /// <summary>
        /// This function is used internally from the expression calls.
        ///  by this I was able to solve passing function handle more than once into another functions.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public QsValue GetInvoke(params QsParameter[] parameters)
        {
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

        internal Func<QsValue> FunctionDelegate_0
        {
            get
            {
                return (Func<QsValue>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsValue> FunctionDelegate_1
        {
            get
            {
                return (Func<QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsValue> FunctionDelegate_2
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_3
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_4
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_5
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_6
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_7
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_8
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_9
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_10
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_11
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        internal Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_12
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
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
            t = t.MergeTokens(new SpaceToken());
            t = t.MergeTokens(new WordToken());
            t = t.MergeTokens(new NumberToken());
            t = t.MergeTokens(new UnitizedNumberToken());

            t = t.MergeTokens(new NameSpaceToken());

            t = t.GroupBrackets();
            t = t.RemoveSpaceTokens();

            t = t.MergeTokens(new AssignmentOperatorToken());

            int tidx = 0; // surve as a base for indexing token if there is namespace it will be 1 otherwise remain 0

            if (t[0].TokenType == typeof(NameSpaceToken)) tidx = 1; //the function begin with namespace.

            if (
                t[tidx].TokenType == typeof(WordToken)
                && (t.Count > (tidx + 1) ? t[tidx + 1].TokenType == typeof(ParenthesisGroupToken) : false)
                && (t.Count > (tidx + 2) ? t[tidx + 2].TokenType == typeof(AssignmentOperatorToken) : false)
                )
            {
                //get function name
                // will be the first token after excluding namespace.
                string functionName = t[tidx].TokenValue;

                string functionNamespace = "";
                if (tidx == 1) functionNamespace = t[0][0].TokenValue;

                //get parameters
                QsParamInfo[] prms = (from c in t[tidx + 1]
                                      where c.TokenType == typeof(WordToken)
                                      select new QsParamInfo { Name = c.TokenValue, Type = QsParamType.Value }).ToArray();


                QsFunction qf = new QsFunction(function)
                {
                    FunctionNamespace = functionNamespace,
                    FunctionName = functionName,
                    Parameters = prms
                };


                LambdaBuilder lb = Utils.Lambda(typeof(QsValue), functionName);


                foreach (QsParamInfo prm in prms)
                {
                    lb.Parameter(typeof(QsParameter), prm.Name);
                }

                List<Expression> statements = new List<Expression>();

                QsVar qv = new QsVar(qse, function.Substring(t[tidx + 2].IndexInText + t[tidx + 2].TokenValueLength), qf, lb);

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

        /// <summary>
        /// Get the function that is stored in the scope.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="realName"></param>
        /// <returns></returns>
        public static QsFunction GetFunction(Scope scope, string realName)
        {
            //get the namespace part
            string ns = "";

            if (realName.IndexOf(':') > -1) ns = realName.Substring(0, realName.IndexOf(':'));

            string funcName = realName.Substring(realName.IndexOf(':') + 1);

            if (string.IsNullOrEmpty(ns))
            {
                // no namespace included then it is from the local scope.

                var function = (QsFunction)QsEvaluator.GetScopeVariable(scope, ns, funcName);

                return function;
                
            }
            else
            {
                //try the external module first 
                //then try the local scope function
                var func = GetFunctionFromExternalModule(realName);
                if (func == null)
                {
                    func = (QsFunction)QsEvaluator.GetScopeVariable(scope, ns, funcName);

                }
                return func;
            }
            
        }



        private static QsFunction GetFunctionFromExternalModule(string realName)
        {
            //get the namespace part
            string ns = "";

            if (realName.IndexOf(':') > -1) ns = realName.Substring(0, realName.IndexOf(':'));

            string funcName = realName.Substring(realName.IndexOf(':') + 1);

            //try another search in the Qs.Modules

            string QsFuncName = funcName.Substring(0, funcName.IndexOf('#'));
            int QsFuncParamCount = int.Parse(funcName.Substring(funcName.IndexOf('#') + 1));


            QsFunction QsModFunc = new QsFunction("[Qs.Modules." + ns + "." + QsFuncName + "]");
            QsModFunc.FunctionNamespace = ns;
            QsModFunc.FunctionName = QsFuncName;


            var module = QsEvaluator.GetQsNameSpace(ns);

            if (module != null)
            {
                List<System.Type> paramTypes = new List<System.Type>(QsFuncParamCount);
                for (int i = 0; i < QsFuncParamCount; i++) paramTypes.Add(typeof(QsParameter));

                MethodInfo mi = module.GetMethod(QsFuncName,
                    BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public,
                    System.Type.DefaultBinder,
                    paramTypes.ToArray(),
                    null);

                if (mi != null)
                {
                    var miParameters = mi.GetParameters();

                    QsParamInfo[] prms = (from c in miParameters
                                          select new QsParamInfo { Name = c.Name, Type = QsParamType.Value }).ToArray();

                    QsModFunc.Parameters = prms;

                    #region Delegate creation section
                    switch (QsFuncParamCount)
                    {
                        case 0:
                            QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                                typeof(Func<QsValue>),
                                mi);
                            break;
                        case 1:
                            QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                                typeof(Func<QsParameter, QsValue>),
                                mi);
                            break;
                        case 2:
                            QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                                typeof(Func<QsParameter, QsParameter, QsValue>),
                                mi);
                            break;
                        case 3:
                            QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                                typeof(Func<QsParameter, QsParameter, QsParameter, QsValue>),
                                mi);
                            break;
                        case 4:
                            QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                                typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                                mi);
                            break;
                        case 5:
                            QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                                typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                                mi);
                            break;
                        case 6:
                            QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                                typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                                mi);
                            break;
                        case 7:
                            QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                                typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                                mi);
                            break;
                        case 8:
                            QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                                typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                                mi);
                            break;
                        case 9:
                            QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                                typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                                mi);
                            break;
                        case 10:
                            QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                                typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                                mi);
                            break;
                        case 11:
                            QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                                typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                                mi);
                            break;
                        case 12:
                            QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                                typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                                mi);
                            break;

                    }
                    #endregion

                    return QsModFunc;
                }
                else
                {
                    throw new QsException("Function " + realName + " not fount in external module.");  // no static function found
                }
            }
            else
            {
                return null;   //no external or built in type found.
            }
        }

        /// <summary>
        /// Get the function from the global heap or from namespace, and throw exception if not found.
        /// This function is used in building expression.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="realName"></param>
        /// <returns></returns>
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
