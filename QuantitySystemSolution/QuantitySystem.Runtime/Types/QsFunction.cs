using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Scripting.Ast;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting.Utils;
using ParticleLexer;
using ParticleLexer.StandardTokens;
using Qs.Types;
using QuantitySystem.Quantities.BaseQuantities;
using Qs.Runtime;
using ParticleLexer.QsTokens;
using System.Linq.Expressions;
using System;


namespace Qs.Types
{
    /// <summary>
    /// Function that declared in Qs
    /// </summary>
    public partial class QsFunction : QsValue, ICloneable
    {
        private string functionName;


        public string FunctionName
        {
            get
            {
                return functionName;
            }
            internal set
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


            }
        }

        /// <summary>
        /// Tells if the function has an evaluated body that can be invoked.
        /// </summary>
        public bool HasCode 
        {
            get
            {
                if (InternalFunctionDelegate == null) return false;
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

            return ((QsScalar)InvokeByQsParameters(parms)).NumericalQuantity;
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

        /// <summary>
        /// Invoke parameterless function
        /// </summary>
        /// <returns></returns>
        public AnyQuantity<double> Invoke()
        {
            return ((QsScalar)FunctionDelegate_0()).NumericalQuantity;
        }

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
                Expression rawParameter = Expression.Constant(args[ip].Trim());

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
                else if (this.Parameters[ip].Type == QsParamType.Raw)
                {
                    //don't evaluate the parameter 
                    // just take the text and pass it to the function as it is.
                    nakedParameter = Expression.Constant(QsParameter.MakeParameter(args[ip], args[ip]));

                    parameters.Add(nakedParameter);
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


            var qsParamArray = Expression.NewArrayInit(typeof(QsParameter), parameters);
            return Expression.Call(Expression.Constant(this), this.GetType().GetMethod("InvokeByQsParameters"), qsParamArray);

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
                    if (parameters[ip].ParameterValue != null)
                    {
                        nakedParameter = QsParameter.MakeParameter(parameters[ip].ParameterValue, parameters[ip].ParameterRawText);  // and I will postpone the evaluation untill we process the function.
                    }
                    else
                    {
                        //look for the raw value , this is the trick to keep the passed function name in the parameters if it wasn't evaluated

                        nakedParameter = QsParameter.MakeParameter(parameters[ip].ParameterRawText, parameters[ip].ParameterRawText);
                    }
                }
                else if (this.Parameters[ip].Type == QsParamType.Raw)
                {
                    nakedParameter = QsParameter.MakeParameter(parameters[ip].ParameterRawText, parameters[ip].ParameterRawText);
                }
                else
                {

                    //normal variable
                    nakedParameter = QsParameter.MakeParameter(parameters[ip].QsNativeValue, parameters[ip].ParameterRawText);

                }

                ProcessedParameters.Add(nakedParameter);

            }

            return InvokeByQsParameters(ProcessedParameters.ToArray());
        }

        #region private delegate properties for the function with its number of parameters

        internal System.Delegate _InternalFunctionDelegate;
        internal System.Delegate InternalFunctionDelegate
        {
            get
            {
                if (_InternalFunctionDelegate == null)
                    _InternalFunctionDelegate = FunctionExpression.Compile();
                return _InternalFunctionDelegate;
            }
            set
            {
                _InternalFunctionDelegate = value;
            }
        }

        private Func<QsValue> FunctionDelegate_0
        {
            get
            {
                return (Func<QsValue>)InternalFunctionDelegate;
            }
        }

        private Func<QsParameter, QsValue> FunctionDelegate_1
        {
            get
            {
                return (Func<QsParameter, QsValue>)InternalFunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsValue> FunctionDelegate_2
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsValue>)InternalFunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_3
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsValue>)InternalFunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_4
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)InternalFunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_5
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)InternalFunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_6
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)InternalFunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_7
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)InternalFunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_8
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)InternalFunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_9
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)InternalFunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_10
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)InternalFunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_11
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)InternalFunctionDelegate;
            }
        }

        private Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue> FunctionDelegate_12
        {
            get
            {
                return (Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>)InternalFunctionDelegate;
            }
        }

        #endregion



        /// <summary>
        /// Function text after '=' equal sign.
        /// </summary>
        public string FunctionBody
        {
            get;
            private set;
        }

        /// <summary>
        /// Hold the tokens of function after '=' equal sign
        /// </summary>
        public Token FunctionBodyToken
        {
            get;
            private set;
        }

        /// <summary>
        /// The function declaration text
        /// </summary>
        public string FunctionDeclaration { get; set; }


        /// <summary>
        /// Parameters of the functions.
        /// </summary>
        public QsParamInfo[] Parameters { get; set; }

        /// <summary>
        /// Returns the parameters names in the function
        /// </summary>
        public string[] ParametersNames
        {
            get
            {
                return (from p in Parameters select p.Name).ToArray();
            }
        }


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


        public QsFunction(string functionBody)
        {
            FunctionDeclaration = functionBody;
            
        }

        private readonly bool _IsReadOnly;

        /// <summary>
        /// means you shouldn't modify the value of this function.
        /// </summary>
        public bool IsReadOnly
        {
            get { return _IsReadOnly; }
        }

        public QsFunction(string functionBody, bool isReadOnly)
        {
            FunctionDeclaration = functionBody;
            
            _IsReadOnly = isReadOnly;
        }

        public override string ToShortString()
        {
            return FunctionDeclaration.Substring(0, FunctionDeclaration.IndexOf(')') + 1);
        }

        public override string ToString()
        {
            return FunctionDeclaration;
        }

        #region Helper Functions


        public static Token TokenizeFunction(string functionCode)
        {
            Token functionToken = Token.ParseText(functionCode);
            functionToken = functionToken.MergeTokens<MultipleSpaceToken>();
            functionToken = functionToken.MergeTokens<WordToken>();
            functionToken = functionToken.MergeTokens<NumberToken>();
            functionToken = functionToken.MergeTokens<UnitizedNumberToken>();

            //merge namespace  word+':'  but in multiple namespaces it makes them separate
            functionToken = functionToken.MergeTokens<NamespaceToken>();

            functionToken = functionToken.MergeTokensInGroups(new ParenthesisGroupToken(), new SquareBracketsGroupToken());
            functionToken = functionToken.RemoveSpaceTokens();

            return functionToken;
        }

        public static QsFunction ParseFunction(QsEvaluator qse, string functionDeclaration)
        {
            if (functionDeclaration.StartsWith("@")) return null; 

            // fast check for function as = and ) before it.
            int eqIdx = functionDeclaration.IndexOf('=');
            if (eqIdx > 0)
            {
                // check if ')' exist before the equal '=' sign.
                while (eqIdx > 0)
                {
                    eqIdx--;
                    if (functionDeclaration[eqIdx] == ' ') continue;   // ignore spaces.
                    if (functionDeclaration[eqIdx] == '\t') continue;  // ignore tabs
                    if (functionDeclaration[eqIdx] == ')') goto GoParseFunction;
                    else return null;
                }
            }
            else
            {
                // equal sign exist.
                return null;
            }

        GoParseFunction:


            var functionToken = TokenizeFunction(functionDeclaration);
            return ParseFunction(qse, functionDeclaration, functionToken);
            
        }

        public static QsFunction ParseFunction(QsEvaluator qse, string functionDeclaration, Token functionToken)
        {

            int nsidx = 0; // surve as a base for indexing token if there is namespace it will be 1 otherwise remain 0

            // specify the namespaces end token.
            string functionNamespace = "";
            foreach (var tok in functionToken)
            {
                if (tok.TokenClassType == typeof(NamespaceToken))
                {
                    nsidx++;
                    functionNamespace += tok.TokenValue;
                }
                else
                    break;
            }

            if (
                functionToken[nsidx].TokenClassType == typeof(WordToken) &&
                (functionToken.Count > (nsidx + 1) ? functionToken[nsidx + 1].TokenClassType == typeof(ParenthesisGroupToken) : false) &&
                (functionToken.Count > (nsidx + 2) ? functionToken[nsidx + 2].TokenClassType == typeof(EqualToken) : false)
                )
            {
                //get function name
                // will be the first token after excluding namespace.

                string functionName  = string.Empty;
                

                functionName = functionToken[nsidx].TokenValue;

                //remove the last : from namespace
                functionNamespace = functionNamespace.TrimEnd(':');

                

                List<string> textParams = new List<string>();
                List<QsParamInfo> prms = new List<QsParamInfo>();
                foreach (var c in functionToken[nsidx + 1])
                {
                    if (
                            c.TokenValue.StartsWith("(") || 
                            c.TokenValue.StartsWith(")") || 
                            c.TokenValue.StartsWith(",") ||
                            c.TokenClassType==typeof(MultipleSpaceToken)
                        )
                    {
                        //ignore these things.
                    }
                    else
                    {
                        if (char.IsLetter(c.TokenValue[0]))
                        {
                            textParams.Add(c.TokenValue);
                            prms.Add(new QsParamInfo { Name = c.TokenValue, Type = QsParamType.Value });
                        }
                        else
                            throw new QsSyntaxErrorException("Parameter name must statrt with a letter");
                    }
                }

                //declared for first time a default function.
                QsFunction qf = new QsFunction(functionDeclaration)
                {
                    FunctionNamespace = functionNamespace,
                    FunctionName = functionName,
                    Parameters = prms.ToArray()
                };

               
                LambdaBuilder lb = Utils.Lambda(typeof(QsValue), functionName);

                foreach (QsParamInfo prm in prms)
                {
                    lb.Parameter(typeof(QsParameter), prm.Name);
                }

                List<Expression> statements = new List<Expression>();

                qf.FunctionBody = functionDeclaration.Substring(functionToken[nsidx + 2].IndexInText + functionToken[nsidx + 2].TokenValueLength).Trim();

                QsVar qv = new QsVar(qse, qf.FunctionBody, qf, lb);

                statements.Add(qv.ResultExpression);   //making the variable expression itself make it the return value of the function.

                lb.Body = Expression.Block(statements);

                LambdaExpression lbe = lb.MakeLambda();

                qf.FunctionExpression = lbe;

                qf.FunctionBodyToken = functionToken.TrimTokens(3, 0);

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

                var function = (QsFunction)QsEvaluator.GetScopeValueOrNull(scope, qsNamespace, functionName);

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
        
        /// <summary>
        /// Test if the name express a symbolic name for a function.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsItFunctionSymbolicName(string name)
        {
            if (Regex.Match(name, @".+#.+").Success)
                return true;
            else 
                return false;
        }


        /// <summary>
        /// Test the last charachter in the text f#2 if it is digit then the symbolic name is not expressing overloaded function.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsItDefaultFunctionSymbolicName(string name)
        {
            if (char.IsDigit(name.ToCharArray().Last())) return true;
            else return false;
        }

        #endregion





        #region ICloneable Members

        public object Clone()
        {
            QsFunction f = (QsFunction)this.MemberwiseClone();
            var fdname = f.FunctionDeclaration.Substring(0, f.FunctionDeclaration.IndexOf('('));
            f.FunctionDeclaration = f.FunctionDeclaration.Replace(fdname, "_");
            return f;

        }

        #endregion
    }
}
