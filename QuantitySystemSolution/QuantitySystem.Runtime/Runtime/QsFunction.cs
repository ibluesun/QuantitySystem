using System.Collections.Generic;
using System.Linq;
using Microsoft;
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
using System.Text;

using Microsoft.Scripting.Utils;
using System.Text.RegularExpressions;


namespace Qs.Runtime
{
    /// <summary>
    /// Function that declared in Qs
    /// </summary>
    public partial class QsFunction
    {
        private string functionName;

        private readonly bool _IsDefault = false;

        /// <summary>
        /// if this is the first function declared then this flag will be set to true;
        /// </summary>
        public bool IsDefault
        {
            get { return _IsDefault; }
        }


        /// <summary>
        /// The function name as stored in the program scope.
        /// </summary>
        public string FunctionSymbolicName 
        { 
            get
            {
                //the function name should be unique for the sake of multiple variables and not to hide 
                //   other variables.

                if (_IsDefault)
                    return functionName + "#" + Parameters.Length.ToString(CultureInfo.InvariantCulture);
                else
                {
                    //make the function naming include also parameter names.
                    // i.e.   f#2_x_y_z

                    StringBuilder sb = new StringBuilder(functionName + "#" + Parameters.Length.ToString(CultureInfo.InvariantCulture));

                    foreach (var p in this.Parameters)
                    {
                        sb.Append("_");
                        sb.Append(p.Name);
                    }

                    return sb.ToString();
                }
            }

        }

        internal string FunctionName
        {
            get
            {
                return functionName;
            }
            set
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

            return ((QsScalar)InvokeByQsParameters(parms)).Quantity;
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

            return (QsScalar)InvokeByQsParameters(parms);
            
        }

        ///// <summary>
        ///// Invoke parameterless function
        ///// </summary>
        ///// <returns></returns>
        //public AnyQuantity<double> Invoke()
        //{
        //    return ((QsScalar)FunctionDelegate_0()).Quantity;
        //}

        /// <summary>
        /// Invoke function with any number of parameters up to 12 parameter
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public QsValue InvokeByQsParameters(params QsParameter[] args)
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
        public Expression GetInvokeExpression(QsVar vario, string[] args)
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

                    
                    // -> Catch(QsVariableNotFoundException e) {QsParameter.MakeParameter(e.ExtraData, args[ip])}

                    var e = Expression.Parameter(typeof(QsVariableNotFoundException), "e");
                    Expression catchBody = Expression.Call(typeof(QsParameter).GetMethod("MakeParameter"),
                        Expression.Property(e, "ExtraData"),
                        rawParameter);

                    // The try catch block when catch exception will execute the call but by passing the parameter as text only
                    var tt = Utils.Try(tryBody);

                    tt.Catch(e, catchBody);

                    

                    parameters.Add(tt.ToExpression());
                }


            }

            //Expression DelegateProperty = Expression.Property(Expression.Constant(this), "FunctionDelegate_" + parameters.Count.ToString(CultureInfo.InvariantCulture));
            //return Expression.Invoke(DelegateProperty, parameters);

            var qsParamArray = Expression.NewArrayInit(typeof(QsParameter), parameters);
            return Expression.Call(Expression.Constant(this), this.GetType().GetMethod("InvokeByQsParameters"), qsParamArray);
            //Expression DelegateProperty = Expression.Field(Expression.Constant(this), "_FunctionDelegate");

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
                        nakedParameter = QsParameter.MakeParameter(parameters[ip].Value, parameters[ip].ParameterRawText);  // and I will postpone the evaluation untill we process the function.
                    }
                    else
                    {
                        //look for the raw value , this is the trick to keep the passed function name in the parameters if it wasn't evaluated

                        nakedParameter = QsParameter.MakeParameter(parameters[ip].ParameterRawText, parameters[ip].ParameterRawText);
                    }
                }
                else
                {

                    //normal variable
                    nakedParameter = QsParameter.MakeParameter(parameters[ip].Quantity, parameters[ip].ParameterRawText);

                }

                ProcessedParameters.Add(nakedParameter);

            }

            return InvokeByQsParameters(ProcessedParameters.ToArray());
        }

        #region private delegate properties for the function with its number of parameters

        internal System.Delegate _FunctionDelegate;

        private Func<QsValue> FunctionDelegate_0
        {
            get
            {
                return (Func<QsValue>)_FunctionDelegate;
            }
        }

        private Func<QsParameter, QsValue> FunctionDelegate_1
        {
            get
            {
                return (Func<QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsValue> FunctionDelegate_2
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_3
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_4
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_5
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_6
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_7
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_8
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_9
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_10
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_11
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)_FunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_12
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
        public string FunctionBody { get; set; }


        /// <summary>
        /// Parameters of the functions.
        /// </summary>
        public QsParamInfo[] Parameters { get; set; }


        /// <summary>
        /// returns 0 indexed parameter name location
        /// -1 if not found
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public int GetParameterOrdinal(string parameterName)
        {
            int idx = Parameters.FindIndex((vv) => vv.Name.Equals(parameterName, System.StringComparison.InvariantCultureIgnoreCase));
            return idx;
        }

        /// <summary>
        /// Test if parameter exist in the function.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public bool ContainsParameter(string parameterName)
        {
            if (GetParameterOrdinal(parameterName) > -1) return true;
            else return false;
        }

        /// <summary>
        /// Test if all of given parameters exist in this function.
        /// </summary>
        /// <param name="parametersNames"></param>
        /// <returns></returns>
        public bool ContainsParameters(params string[] parametersNames)
        {
            foreach (string paramName in parametersNames)
            {
                if (!ContainsParameter(paramName))
                    return false; //parameter does not exist break and return false;
            }

            return true;
        }



        public QsFunction(string functionBody, bool isDefault)
        {
            FunctionBody = functionBody;
            _IsDefault = isDefault;
        }

        public QsFunction(string functionBody)
        {
            FunctionBody = functionBody;
            _IsDefault = false;
        }

        private readonly bool _IsReadOnly;

        /// <summary>
        /// means you shouldn't modify the value of this function.
        /// </summary>
        public bool IsReadOnly
        {
            get { return _IsReadOnly; }
        }

        public QsFunction(string functionBody, bool isDefault, bool isReadOnly)
        {
            FunctionBody = functionBody;
            _IsDefault = isDefault;
            _IsReadOnly = isReadOnly;
        }

        public override string ToString()
        {
            return FunctionBody;
        }

        #region Helper Functions

        public static QsFunction ParseFunction(QsEvaluator qse, string function)
        {
            // fast check for function as = and ) before it.
            int eqIdx = function.IndexOf('=');
            if (eqIdx > 0)
            {
                // check if ')' exist before the equal '=' sign.
                while (eqIdx > 0)
                {
                    eqIdx--;
                    if (function[eqIdx] == ' ') continue;   // ignore spaces.
                    if (function[eqIdx] == '\t') continue;  // ignore tabs
                    if (function[eqIdx] == ')') goto GoParseFunction;
                    else return null;
                }
            }
            else
            {
                // equal sign exist.
                return null;
            }


            GoParseFunction:

            Token t = Token.ParseText(function);
            t = t.MergeTokens(new SpaceToken());
            t = t.MergeTokens(new WordToken());
            t = t.MergeTokens(new NumberToken());
            t = t.MergeTokens(new UnitizedNumberToken());

            t = t.MergeTokens(new NameSpaceToken());

            t = t.GroupBrackets();
            t = t.RemoveSpaceTokens();

            t = t.MergeTokens(new AssignmentOperatorToken());

            int nsidx = 0; // surve as a base for indexing token if there is namespace it will be 1 otherwise remain 0

            if (t[0].TokenType == typeof(NameSpaceToken)) nsidx = 1; //the function begin with namespace.

            if (
                t[nsidx].TokenType == typeof(WordToken)
                && (t.Count > (nsidx + 1) ? t[nsidx + 1].TokenType == typeof(ParenthesisGroupToken) : false)
                && (t.Count > (nsidx + 2) ? t[nsidx + 2].TokenType == typeof(AssignmentOperatorToken) : false)
                )
            {
                //get function name
                // will be the first token after excluding namespace.
                string functionName = t[nsidx].TokenValue;

                string functionNamespace = "";
                if (nsidx == 1) functionNamespace = t[0][0].TokenValue;

                //get parameters
                QsParamInfo[] prms = (from c in t[nsidx + 1]
                                      where c.TokenType == typeof(WordToken)
                                      select new QsParamInfo { Name = c.TokenValue, Type = QsParamType.Value }).ToArray();

                var textParams = from prm in prms select prm.Name;

                QsFunction qf;

                // 1- find the default function of this name.
                // 2- if default function exist declare non default function
                // 3- if default function does'nt exist declare default function.
                // Default function: is function declared without specifying its parameters in its name  f#2 f#4  are default functions.

                qf = QsFunction.GetDefaultFunction(qse.Scope, functionNamespace, functionName, textParams.Count());

                if (qf == null)
                {
                    //declared for first time a default function.
                    qf = new QsFunction(function, true)
                     {
                         FunctionNamespace = functionNamespace,
                         FunctionName = functionName,
                         Parameters = prms
                     };
                }
                else
                {
                    // 1- find if function with the same parameters and name exist 
                    // 2- overwrite this function otherwise create new one.

                    qf = GetExactFunctionWithParameters(qse.Scope,
                        functionNamespace,
                        functionName,
                        textParams.ToArray());
                    if (qf == null)
                    {
                        qf = new QsFunction(function, false)
                        {
                            FunctionNamespace = functionNamespace,
                            FunctionName = functionName,
                            Parameters = prms
                        };
                    }
                    else
                    {
                        // do nothing we will only change the implementation.
                        if (qf.IsReadOnly == false) qf.FunctionBody = function;
                        else throw new QsException("Attempt to write into readonly function");
                    }
                }


                LambdaBuilder lb = Utils.Lambda(typeof(QsValue), functionName);


                foreach (QsParamInfo prm in prms)
                {
                    lb.Parameter(typeof(QsParameter), prm.Name);
                }

                List<Expression> statements = new List<Expression>();

                QsVar qv = new QsVar(qse, function.Substring(t[nsidx + 2].IndexInText + t[nsidx + 2].TokenValueLength), qf, lb);

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
        public static QsFunction GetFunction(Scope scope, string qsNamespace, string functionName)
        {

            if (string.IsNullOrEmpty(qsNamespace))
            {
                // no namespace included then it is from the local scope.

                var function = (QsFunction)QsEvaluator.GetScopeVariable(scope, qsNamespace, functionName);

                return function;
                
            }
            else
            {

                try
                {

                    QsNamespace ns = QsNamespace.GetNamespace(scope, qsNamespace);


                    return (QsFunction)ns.GetValue(functionName);

                }
                catch(QsVariableNotFoundException)
                {
                    return null;
                }
                
                

            }
        }




        /// <summary>
        /// Called from Expressions.
        /// Get the function from the global heap or from namespace, and throw exception if not found.
        /// This function is used in building expression.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="realName"></param>
        /// <returns></returns>
        public static QsFunction GetFunctionAndThrowIfNotFound(Scope scope, string functionFullName)
        {
            string nameSpace = string.Empty;
            string functionName = functionFullName;

            if (functionFullName.Contains(':'))
            {
                nameSpace = functionFullName.Substring(0, functionFullName.IndexOf(':'));
                functionName = functionFullName.Substring(functionFullName.IndexOf(':') + 1);
            }


            var f = GetFunction(scope, nameSpace, functionName);

            if (f == null) throw new QsFunctionNotFoundException("Function: '" + functionName + "' Couldn't be found in " + nameSpace + ".");
            else return f;
        }


        /// <summary>
        /// Form a function symbolic name that can be stored into the program heap.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="paramCount"></param>
        /// <returns></returns>
        public static string FormFunctionSymbolicName(string functionName, int paramCount)
        {
            return functionName + "#" + paramCount.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Form a function symbolic name that can be stored into the program heap, but also include parameter names in the symbol.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="paramNames"></param>
        /// <returns></returns>
        public static string FormFunctionSymbolicName(string functionName, params string[] paramNames)
        {
            //make the function naming include also parameter names.
            // i.e.   f#2_x_y_z

            string symbol = functionName + "#" + paramNames.Length.ToString(CultureInfo.InvariantCulture);
            foreach (var p in paramNames) symbol += "_" + p.ToLowerInvariant();
            return symbol;
        }

        public static bool IsItFunctionSymbolicName(string name)
        {
            if (Regex.Match(name, @".+#.+").Success)
                return true;
            else 
                return false;
        }

        #endregion



    }
}
