using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Scripting;

using Microsoft.Scripting.Ast;

using ParticleLexer;
using ParticleLexer.TokenTypes;
using QuantitySystem;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities.DimensionlessQuantities;
using QuantitySystem.Units;
using System.Globalization;
using Qs.Modules;
using System.Reflection;
using Qs.RuntimeTypes;
using Qs.Runtime.Operators;


namespace Qs.Runtime
{
    public enum QsVarParseModes
    {
        None,
        Function,
        Sequence
    }

    public class QsVar
    {

        private readonly QsEvaluator evaluator;

        public QsEvaluator Evaluator
        {
            get
            {
                return evaluator;
            }
        }


        #region regular expressions

        public const string VariableNameExpression = @"^\s*(\w+)\s*$";

        const string DoubleNumber = @"[-+]?\d+(\.\d+)*([eE][-+]?\d+)*";

        const string UnitizedNumber = "(?<num>" + DoubleNumber + ")\\s*<(?<unit>.+?)>";

        const string SimpleArithmatic = "\\s*(?<q>(" + UnitizedNumber + ")|(" + DoubleNumber + ")|\\w+)\\s*(?<op>[\\+\\-\\/\\*])?";

        #endregion


        public class ExprOp
        {
            public Expression QuantityExpression { get; set; }
            public string Operation { get; set; }
            public ExprOp Next { get; set; }
        }



        public QsVarParseModes ParseMode { get; set; }

        LambdaBuilder lambdaBuilder = null;


        private QsFunction Function;


        /// <summary>
        /// Evaluate the function body taking into considerations
        /// the parameters of the lambda function.
        /// </summary>
        /// <param name="evaluator"></param>
        /// <param name="line"></param>
        /// <param name="lb"></param>
        public QsVar(QsEvaluator evaluator, string line, QsFunction function, LambdaBuilder lb)
        {
            this.evaluator = evaluator;
            if (function != null)
            {
                lambdaBuilder = lb;

                Function = function;

                ParseMode = QsVarParseModes.Function;
            }

            ResultExpression = ParseArithmatic(line);

        }


        public static Expression ParseToExpression(QsEvaluator evaluator, string line)
        {
            QsVar v = new QsVar(evaluator, line);
            return v.ResultExpression;
        }


        /// <summary>
        /// Evaluates Normal Calculations.
        /// </summary>
        /// <param name="evaluator"></param>
        /// <param name="line"></param>
        public QsVar(QsEvaluator evaluator, string line)
        {
            this.evaluator = evaluator;

            ResultExpression = ParseArithmatic(line);

        }

        private QsSequence Sequence= null;

        /// <summary>
        /// Evaulate the line for use in sequence body.
        /// </summary>
        /// <param name="evaluator"></param>
        /// <param name="line"></param>
        /// <param name="sequenceName"></param>
        public QsVar(QsEvaluator evaluator, string line, QsSequence sequence, LambdaBuilder lb)
        {
            this.evaluator = evaluator;

            if (sequence!=null)
            {
                lambdaBuilder = lb;
                Sequence = sequence;
                ParseMode = QsVarParseModes.Sequence;
            }

            ResultExpression = ParseArithmatic(line);
        }


        /// <summary>
        /// Merge operators that have more than one letter.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        internal Token MergeOperators(Token token)
        {
            Token tok = token.MergeTokens(new PowerDotToken());
            tok = tok.MergeTokens(new PowerCrossToken());
            

            return tok;
        }

        internal Token ConditionsTokenize(Token token)
        {
            var tokens = token.MergeTokens(new WhenStatementToken());

            tokens = tokens.MergeTokens(new OtherwiseStatementToken());

            tokens = tokens.MergeTokens(new AndStatementToken());
            tokens = tokens.MergeTokens(new OrStatementToken());
            tokens = tokens.MergeTokens(new EqualToken());
            tokens = tokens.MergeTokens(new NotEqualToken());
            tokens = tokens.MergeTokens(new LessThanToken());
            tokens = tokens.MergeTokens(new LessThanOrEqualToken());
            tokens = tokens.MergeTokens(new GreaterThanToken());
            tokens = tokens.MergeTokens(new GreaterThanOrEqualToken());


            return tokens;



        }

        /// <summary>
        /// Main parsing method.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        internal Expression ParseArithmatic(string line)
        {
            var tokens = Token.ParseText(line);            

            tokens = tokens.MergeTokens(new SpaceToken());

            tokens = ConditionsTokenize(tokens);   // make tokens of conditional statements

            tokens = tokens.MergeTokens(new WordToken());                 //discover words
            tokens = tokens.MergeTokens(new NumberToken());               //discover the numbers
            tokens = tokens.MergeTokens(new UnitizedNumberToken());   //discover the unitized numbers

            tokens = MergeOperators(tokens);

            tokens = tokens.MergeTokens(new NameSpaceToken());
            tokens = tokens.MergeTokens(new NameSpaceAndValueToken());

            tokens = tokens.GroupBrackets();                             // group (--()-) parenthesis


            tokens = tokens.MergeTokens(new MagnitudeToken());
            tokens = tokens.MergeTokens(new AbsoluteToken());



            tokens = tokens.RemoveSpaceTokens();                           //remove all spaces

            tokens = tokens.DiscoverCalls(StringComparer.OrdinalIgnoreCase, "When", "Otherwise", "and", "or"); //ignore reserved words 

            Expression quantityExpression = null;
            ExprOp eop = null;

            ExprOp FirstEop = null;

            int ix = 0;                 //this is the index in the discovered tokens
            
            while (ix < tokens.Count)
            {
                string q = tokens[ix].TokenValue;
                if (q == "+" || q == "-")
                {
                    // unary prefix operator.

                    //consume another token for number
                    ix++;
                    q = q + tokens[ix].TokenValue;
                }

                string op = ix + 1 < tokens.Count ? tokens[ix + 1].TokenValue : string.Empty;

                bool FactorialPostfix = false;
                if (!string.IsNullOrEmpty(op))
                {
                    if (op == "!")
                    {
                        FactorialPostfix = true;
                    }

                }
                if(tokens[ix].TokenType == typeof(SequenceCallToken))
                {
                    quantityExpression = SequenceCallExpression(
                        tokens[ix][0].TokenValue,
                        tokens[ix][1],
                        tokens[ix].Count > 2 ? tokens[ix][2] : null                   //if arguments exist we must include them   form  S[n](x,y,z)
                        );

                }
                else if (tokens[ix].TokenType == typeof(FunctionCallToken))
                {

                    quantityExpression = FunctionCallExpression(
                        tokens[ix][0].TokenValue,
                        tokens[ix][1]
                        );
                }
                else if (tokens[ix].TokenType == typeof(ParenthesisGroupToken))
                {
                    quantityExpression = ParseArithmatic(q.Substring(1, q.Length - 2));
                }
                else if (tokens[ix].TokenType == typeof(UnitizedNumberToken))
                {
                    //unitized number                    
                    quantityExpression = Expression.Constant(QsValue.ParseScalar(q), typeof(QsValue)); //you have to explicitly tell expression the type because it searches for the operators and can't find them
                }
                else if (tokens[ix].TokenType == typeof(NumberToken))
                {
                    //ordinary number

                    quantityExpression = Expression.Constant(QsValue.ParseScalar(q), typeof(QsValue));

                }
                else if (tokens[ix].TokenType == typeof(CurlyBracketGroupToken))
                {
                    // Vector declaration.
                    quantityExpression = ParseVector(tokens[ix]);
                }
                else if (tokens[ix].TokenType == typeof(SquareBracketGroupToken))
                {
                    // Matrix declaration.
                    quantityExpression = ParseMatrix(tokens[ix]);
                }
                else if (tokens[ix].TokenType == typeof(MagnitudeToken))
                {
                    quantityExpression = ValueNorm(tokens[ix]);
                }
                else if (tokens[ix].TokenType == typeof(AbsoluteToken))
                {
                    quantityExpression = ValueAbsolute(tokens[ix]);
                }
                else
                {
                    // Word token:  means variable 
                    if (ParseMode == QsVarParseModes.Function)
                    {
                        #region Variabl in Function Parsing
                        //get it from the parameters of the lambda
                        //  :) if it is found here then it will not be obtained from the global heap :)
                        //      now I understand how variable scopes occur :D

                        try
                        {
                            Expression eu = lambdaBuilder.Parameters.Single(c => c.Name == q);


                            // parameter is having quantity in normal cases
                            Expression directQuantity = Expression.Property(eu, "Quantity");

                            // another expression for getting the quantity from the variable passed 
                            //   this case only occure if I called a function declared like this   f(a) = a(5,3)+a
                            //      because when calling f(u)  where u=50 and u(x,y)=x+y  then I want the evaluation to get the calculations right
                            //      because 'a' alone is expressing single global variable.



                            Expression indirectQuantity = Expression.Call(
                                                    eu,
                                                    typeof(QsParameter).GetMethod("GetIndirectQuantity"),
                                                    Expression.Constant(Scope)
                                                    );

                            //if the quantity in parameter == null then get it as indirect quantity from the scope of the variables

                            quantityExpression = Expression.Condition(
                                Expression.Equal(directQuantity, Expression.Constant(null)),
                                indirectQuantity,
                                directQuantity
                            );

                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.Print(e.ToString());

                            //quantity variable  //get it from evaluator  global heap
                            quantityExpression = GetVariable(q);
                        }
                        #endregion
                    }
                    else if (ParseMode == QsVarParseModes.Sequence)
                    {
                        #region Variable in Sequence Parsing
                        //try to get the variable from the sequence index
                        try
                        {
                            quantityExpression = lambdaBuilder.Parameters.Single(c => c.Name == q);

                            //we should check if q is index or parameter of sequence.

                            if (Sequence.SequenceIndexName == q)
                            {
                                //it is really an index
                                // convert it into Quantity.
                                //   because it is only integer

                                quantityExpression = Expression.Call(typeof(Qs).GetMethod("ToScalarValue", new Type[] { typeof(int) }), quantityExpression);

                            }

                        }
                        catch
                        {
                            //quantity variable  //get it from evaluator  global heap
                            quantityExpression = GetVariable(q);
                            Sequence.CachingEnabled = false;  //because when evaluating external variable the external variable may change without knowing
                        }
                        #endregion

                    }
                    else
                    {
                        //quantity variable  //get it from evaluator  global heap
                        quantityExpression = GetVariable(q);
                    }

                }

                //apply the postfix here

                if (FactorialPostfix)
                {
                    quantityExpression = Expression.Call(typeof(QsGamma).GetMethod("Factorial"), quantityExpression);

                    //get the next operator.
                    ix++;
                    op = ix + 1 < tokens.Count ? tokens[ix + 1].TokenValue : string.Empty;

                    FactorialPostfix = false;
                }

                if (eop == null)
                {
                    //firs time creation
                    FirstEop = new ExprOp();

                    eop = FirstEop;
                }
                else
                {
                    //use the next object to be eop.
                    eop.Next = new ExprOp();
                    eop = eop.Next;
                }

                eop.Operation = op;
                eop.QuantityExpression = quantityExpression;

                ix += 2;

            }

            //then form the calculation expression
            return  ConstructExpression(FirstEop);

        }


        /// <summary>
        /// Get Absolute of number or determinant of matrix.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private Expression ValueAbsolute(Token token)
        {
            Expression rr = ParseArithmatic(token.TokenValue.Trim('|'));

            rr = Expression.Call(rr, typeof(QsValue).GetMethod("AbsOperation"));

            return rr;
        }

        /// <summary>
        /// Get the magnitude of the inner text between ||text||
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private Expression ValueNorm(Token token)
        {
            //the token is holding the vector or the expression that will lead to the vector.

            Expression rr = ParseArithmatic(token.TokenValue.Trim('|'));

            rr = Expression.Call(rr, typeof(QsValue).GetMethod("NormOperation"));
            
            return rr;

        }


        /// <summary>
        /// Vector of the format { 40, 20, f(2), S[10], 23}
        /// </summary>
        /// <param name="vectorText"></param>
        /// <returns></returns>
        private Expression ParseVector(Token tok)
        {
            List<Expression> qtyExpressions = new List<Expression>();
            Token token = tok.TrimStart(typeof(LeftCurlyBracketToken));
            token = token.TrimEnd(typeof(RightCurlyBracketToken));

            token = token.MergeAllBut(typeof(WordToken), new CommaToken(), new SpaceToken());
            token = token.RemoveTokens(typeof(CommaToken), typeof(SpaceToken));
            

            for (int i = 0; i < token.Count ;i++ )
            {
                if (token[i].TokenType==typeof(WordToken))
                {
                    qtyExpressions.Add(ParseArithmatic(token[i].TokenValue));
                }
            }

            var valuesArray = Expression.NewArrayInit(typeof(QsValue), qtyExpressions);

            var vectorExpression = Expression.Call(
                typeof(QsValue).GetMethod("VectorFromValues"),
                valuesArray);

            return vectorExpression;
        }


        /// <summary>
        /// Parsing is expecting values separated by colon or space 
        /// also components should be vectors on the syntax {4 3 2 1}.
        /// </summary>
        /// <param name="tok"></param>
        /// <returns></returns>
        private Expression ParseMatrix(Token tok)
        {
            List<Expression> vctExpressions = new List<Expression>();

            Token token = tok.TrimStart(typeof(LeftSquareBracketToken));

            token = token.TrimEnd(typeof(RightSquareBracketToken));

            // first split between semi colon tokens.
            // then use every splitted token to 
            
            token = token.MergeAllBut(typeof(WordToken), new SemiColonToken());

            // now the tokens are scalars or vectors followed by semi colon;
            // put them beside each other

            for (int i = 0; i < token.Count; i++)
            {
                if (token[i].TokenType == typeof(WordToken))
                {

                    vctExpressions.Add(ParseArithmatic("{" + token[i].TokenValue + "}"));
                    
                }
            }

            var valuesArray = Expression.NewArrayInit(typeof(QsValue), vctExpressions);

            var matrixExpression = Expression.Call(
                typeof(QsValue).GetMethod("MatrixFromValues"),
                valuesArray);

            return matrixExpression;
        }



        /// <summary>
        /// Obtain variable value by expression in the current scope
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Expression GetVariable(string name)
        {
            Type ScopeType = this.Evaluator.Scope.GetType();

            //store the scope
            var ScopeExp = Expression.Constant(Evaluator.Scope, ScopeType);

            //check if the name contain namespace
            string[] var = name.Split(':');
            if (var.Length == 2)
            {
                var fe = Expression.Call(
                    typeof(QsEvaluator).GetMethod("GetScopeQsValue"),
                    ScopeExp,
                    Expression.Constant(var[0]),
                    Expression.Constant(var[1])
                    );
                return fe;
            }
            else
            {

                var fe = Expression.Call(
                    typeof(QsEvaluator).GetMethod("GetScopeQsValue"),
                    ScopeExp,
                    Expression.Constant(string.Empty),
                    Expression.Constant(var[0])
                    );


                return fe;
            }
        }

        public QsValue Execute(string line)
        {
            ParseArithmatic(line);
            return Execute() as QsValue;
        }

        public object Execute()
        {
            Expression<Func<object>> cq = Expression.Lambda<Func<object>>
                (this.ResultExpression);

            Func<object> aqf = cq.Compile();

            object result = aqf();
            return result;
        }


        public Microsoft.Scripting.Runtime.Scope Scope
        {
            get
            {
                return this.Evaluator.Scope;
            }
        }

        public Type ScopeType { get { return Scope.GetType(); } }

        #region Expressions Generators

        public Expression SequenceCallExpression(string sequenceName, Token indexes, Token args)
        {
           
            //discover the index
            if (indexes.Count > 3) throw new QsException("Calling sequence with more than one index is not supported yet.");

            string indexText = indexes[1].TokenValue; //this is parameter between [.]  to be evaluated later
            if (indexText == "]") // empty square brackets
                indexText = QsSequence.DefaultIndexName;

            // discover sequence parameters.
            List<Expression> parameters = new List<Expression>();
            if (args != null)
            {
                //the sequence called with parameters
                parameters = new List<Expression>();

                //now parameters separated
                for (int ai = 1; ai < args.Count - 1; ai++)
                {
                    if (args[ai].TokenValue != ",")
                        parameters.Add(ParseArithmatic(args[ai].TokenValue));
                }
            }

            // discover namespace

            string sequenceNamespace = string.Empty;
            if (sequenceName.IndexOf(':') > -1) sequenceNamespace = sequenceName.Substring(0, sequenceName.IndexOf(':'));

            string seqCallName = sequenceName.Substring(sequenceName.IndexOf(':') + 1);

            string seqo = QsSequence.FormSequenceSymbolicName(seqCallName, 1, parameters.Count);

            //get the sequence dynamically because sequence can be recursive :)

            Expression QsSequExpression = Expression.Call(typeof(QsSequence).GetMethod("GetSequence"),
                Expression.Constant(this.Evaluator.Scope),
                Expression.Constant(sequenceNamespace),
                Expression.Constant(seqo)
                );

            #region Index manipulation
            Expression IndexExpression = null;
            Expression FromExpression = null;
            Expression ToExpression = null;

            // now the index could be
            // integer number
            int n;

            string methodName = "GetElementValue";

            Token itok = Token.ParseText(indexText);
            itok = itok.RemoveSpaceTokens();
            itok = itok.MergeTokens(new WordToken());
            
            itok = itok.MergeAllBut(typeof(WordToken), new SequenceRangeToken());

            if (itok.Count == 3 && itok[1].TokenType == typeof(SequenceRangeToken))
            {
                FromExpression = Expression.Call(typeof(Qs).GetMethod("IntegerFromQsValue"), ParseArithmatic(itok[0].TokenValue));
                ToExpression = Expression.Call(typeof(Qs).GetMethod("IntegerFromQsValue"), ParseArithmatic(itok[2].TokenValue));

                if (itok[1].TokenValue == "++")
                    methodName = "SumElements";
                else if (itok[1].TokenValue == "!!")
                    methodName = "Average";
                else if (itok[1].TokenValue == "**")
                    methodName = "MulElements";
                else if (itok[1].TokenValue == "..")
                    methodName = "QsValueElements";
            }
            else if (int.TryParse(indexText, out n))
            {
                IndexExpression = Expression.Constant(n, typeof(int));
            }
            else
            {
                // expression to be calculated 
                //     variable that should be manipulated from the sequence body if we are declaring sequence.
                //     variable from the sequence parameters

                IndexExpression = ParseArithmatic(indexText);

                //however we need to convert it to integer. because it is an index
                IndexExpression = Expression.Call(typeof(Qs).GetMethod("IntegerFromQsValue"), IndexExpression);

            }
            #endregion

            #region GetElementValue determination
            MethodInfo mi;
            if (methodName == "GetElementValue")
            {
                switch (parameters.Count)
                {
                    case 0:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int) });
                        break;
                    case 1:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(QsValue) });
                        break;
                    case 2:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(QsValue), typeof(QsValue) });
                        break;
                    case 3:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(QsValue), typeof(QsValue), typeof(QsValue) });
                        break;
                    case 4:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue) });
                        break;
                    case 5:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue) });
                        break;
                    case 6:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue) });
                        break;
                    case 7:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue) });
                        break;
                    default:
                        throw new QsException("Number of sequence parameters exceed the built in functionality");
                }
                // insert the index parameter.
                parameters.Insert(0, IndexExpression);

            }
            else
            {
                switch (parameters.Count)
                {
                    case 0:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(int) });
                        break;
                    case 1:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(int), typeof(QsValue) });
                        break;
                    case 2:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(int), typeof(QsValue), typeof(QsValue) });
                        break;
                    case 3:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(int), typeof(QsValue), typeof(QsValue), typeof(QsValue) });
                        break;
                    case 4:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(int), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue) });
                        break;
                    case 5:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(int), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue) });
                        break;
                    case 6:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(int), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue) });
                        break;
                    case 7:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(int), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue), typeof(QsValue) });
                        break;
                    default:
                        throw new QsException("Number of sequence parameters exceed the built in functionality");
                }
                // insert the index parameter.
                parameters.Insert(0, ToExpression);
                parameters.Insert(0, FromExpression);

            }

            #endregion

            var finale = Expression.Call(QsSequExpression
                , mi
                , parameters.ToArray()
                );

            return finale;

        }

        /// <summary>
        /// Used to build the function call expression 
        /// either if called directly or called from another function 
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Expression FunctionCallExpression(string functionName, Token args)
        {
            //fn(x, y, ff(y/x, e + fr(d)))     <== sample form :D

            List<string> paramsText = new List<string>();  //contains the parameters sent as text 

            //discover parameters
            for (int ai = 1; ai < args.Count - 1; ai++ )
            {
                if (args[ai].TokenValue != ",") paramsText.Add(args[ai].TokenValue.Trim());
            }

            // discover namespace
            string functionNameSpace = "";

            if (functionName.IndexOf(':') > -1) functionNameSpace = functionName.Substring(0, functionName.IndexOf(':'));

            // function name part after namespace
            string funcCallName = functionName.Substring(functionName.IndexOf(':') + 1);

            //now parameters separated

            // search for suitable function to be called.
            // 1- if the parameters are raw parameters without named argument then get the default function
            // 2- if one of the parameters contain named argument then search for this function.

            bool NamedArgumentOccured = false;
            

            List<string> argNames = new List<string>();

            for (int i = 0; i < paramsText.Count; i++)
            {
                string namedParam = paramsText[i];

                int NamedAssignOperatorIndex = namedParam.IndexOf(":=");

                if (NamedAssignOperatorIndex >= 1)
                {
                    argNames.Add(namedParam.Substring(0, NamedAssignOperatorIndex));
                    NamedArgumentOccured = true;
                }
                else
                {
                    if (NamedArgumentOccured) throw new QsException("Normal argument after named argument is not permitted");
                }

                
            }

            int FirstNamedArgumentIndex = paramsText.Count - argNames.Count; //point to the index of the first named argument in the caller.

            QsFunction TargetFunction;

            if (argNames.Count > 0)
            {
                #region Named Arguments discovery
                // get all the functions that contain these named argument parameters.

                var DiscoveredFunctions = QsFunction.FindFunctionByParameters(Scope, functionNameSpace, funcCallName, paramsText.Count, argNames.ToArray());

                if (DiscoveredFunctions.Length == 0)
                {
                    throw new QsFunctionNotFoundException("No functions found at all, please review your parameters");
                }
                if (DiscoveredFunctions.Length > 1)
                {

                    //we need to make another filtering criteria to decrease the discovered numbers.

                    List<QsFunction> Pass2Functions = new List<QsFunction>();

                    foreach (var func in DiscoveredFunctions)
                    {
                        bool include = true;
                        for (int ix = 0; ix < FirstNamedArgumentIndex; ix++)
                        {
                            string calleeParam = func.Parameters[ix].Name;

                            //does this parameter lie in the named arguments one.

                            // for example in case of f(4, U:=3, V:=1)
                            //   and current called function is f(u, r, v)
                            //    then u exist in the list of {U:=3, V:=1}
                            //     which exclude this function from the Pass2Functions.

                            if (argNames.Contains(calleeParam, StringComparer.OrdinalIgnoreCase))
                            {
                                include = false;
                                break;
                            }
                        }

                        if (include) Pass2Functions.Add(func);   
                    }

                    if (Pass2Functions.Count == 0)
                        throw new QsFunctionNotFoundException("No function from the (" + DiscoveredFunctions.Length.ToString(CultureInfo.InvariantCulture) + ") discovered is having suitable parameters order");
                    if (Pass2Functions.Count == 1)
                        TargetFunction = Pass2Functions[0];
                    else
                        throw new QsFunctionNotFoundException("(" + Pass2Functions.Count.ToString(CultureInfo.InvariantCulture) + ") functions, please specify more named arguments. ");
                #endregion

                }
                else
                {
                    TargetFunction = DiscoveredFunctions[0];
                }

            }
            else
            {
                // specify the function real name 
                string TargetFunctionRealName =
                    QsFunction.FormFunctionSymbolicName(funcCallName, paramsText.Count); //to call the right function 

                TargetFunction = QsFunction.GetFunction(Scope, functionNameSpace, TargetFunctionRealName);
            }

            List<Expression> parameters = new List<Expression>();

            #region Helper delegates
            //delegate to check if the function name in the Function object of the current function expression formation or if parsemode = function
            Func<string, Expression> ParameterFunction = delegate(string funcName)
            {
                //this anonymous function is calling the function dynamically from the passed function argument.
                
                //but may be the function name should be retrieved from a parameter.
                int parametersCount;
                if (ParseMode == QsVarParseModes.Function)
                    parametersCount = Function.Parameters.Count(c => c.Name == funcName);
                else if (ParseMode == QsVarParseModes.Sequence)
                    parametersCount = Sequence.Parameters.Count(c => c.Name == funcName);
                else
                    throw new QsException("Parse mode is not known to evaluate this function");

                if (parametersCount > 0)
                {
                    QsParamInfo pinfo; //the parameter in the parent function that hold the function name.
                    if (ParseMode == QsVarParseModes.Function)
                        pinfo = Function.Parameters.Single(c => c.Name == funcName);
                    else if (ParseMode == QsVarParseModes.Sequence)
                        pinfo = Sequence.Parameters.Single(c => c.Name == funcName);
                    else
                        throw new QsException("Parse mode is not known to evaluate this function");



                    //Yes the function should be retrieved dynamically from the passed parameter name.

                    // THIS IS THE RIGHT SIDE OF EVALUATING FUNCTION    i.e. f(a) = 5+a(4,2)+a  
                    // ------------------------------------------------------------------------

                    // Prepare the expression that will execute this function dynamically

                    // Get the function name from the parameter
                    Expression FunctionParameter = lambdaBuilder.Parameters.Single(c => c.Name == functionName);

                    Expression FunctionParameterName = Expression.Call(FunctionParameter,
                        typeof(QsParameter).GetMethod("GetTrueFunctionName"),
                        Expression.Constant(paramsText.Count)
                        );

                    // Get the Function object.
                    Expression Functor = Expression.Call(typeof(QsFunction).GetMethod("GetFunctionAndThrowIfNotFound"),
                        Expression.Constant(Scope),
                        FunctionParameterName
                        );

                    //prepare arguments.
                    //  evaluate the inner arguments of this function  v(x, c , b) = c(b(x), x-2,x/4,40) 
                    //   b(x)  //will be evaluate to another late discover expression.
                    //   x-2
                    //   x/4
                    //   40
                    List<Expression> FunctorParams = new List<Expression>();
                    foreach (string prm in paramsText)
                    {
                        Expression q = ParseArithmatic(prm); // evaluate the parameter
                        Expression rw;
                        try
                        {
                            rw = lambdaBuilder.Parameters.Single(c => c.Name == prm);
                            rw = Expression.Property(rw, "ParameterRawText");  //raw value that was send with this parameter.
                        }
                        catch
                        {
                            rw = Expression.Constant(prm);   // the hard coded parameter because finding the parameter value failed
                            //occurs if the passing parameter contains expression not standalone variable.
                        }

                        
                        var tc = Utils.Try(
                         Expression.Call(
                            typeof(QsParameter).GetMethod("MakeParameter"),
                            q,                                //Evaluated value.
                            rw                                //Raw Value
                            ));

                        tc.Catch(typeof(QsVariableNotFoundException),
                            Expression.Call(
                            typeof(QsParameter).GetMethod("MakeParameter"),
                            Expression.Constant(null),                                //no value 
                            rw                                //Raw Value
                            ));

                        Expression qp = tc.ToExpression();

                        FunctorParams.Add(qp);

                    }

                    // expression to call the function QsFunction.GetInvoke  is used here 
                    Expression CallExpression = Expression.Call(Functor,
                        typeof(QsFunction).GetMethod("GetInvoke"),
                        Expression.NewArrayInit(typeof(QsParameter), FunctorParams)
                        );

                    //set that the parameter is function.
                    pinfo.Type = QsParamType.Function;


                    return CallExpression;
                }

                return null;

            };

            #endregion

            if (TargetFunction != null)
            {
                // Means a function is already available in the global heap to be called.
                // so we can call it directly if we wish 
                

                // check if the function name is originally come from the parameters.
                if (this.ParseMode == QsVarParseModes.Function)
                {
                    //but if the function name is from parameter name 
                    // then we can't call it from global heap 
                    // we have to get the parameter name that was passed to the function
                    //  and call it dynamically from there.

                    Expression FunP = ParameterFunction(functionName); //caling the helper delegate.
                    if (FunP != null) return FunP;
                }


                #region Adjusting (ReArranging) the named arguments order to the real function parameter order

                //Rearrange paramsText so it includes named arguments
                //  named argument is on the format  x:=20, j:=30

                Dictionary<string, string> fParameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                Dictionary<string, bool> setParameters = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

                // first add all values of parameters in dictionary of ParameterName: Value
                for (int i = 0; i < paramsText.Count;i++ )
                {
                    fParameters.Add(TargetFunction.Parameters[i].Name, paramsText[i]);
                    setParameters.Add(TargetFunction.Parameters[i].Name, false);
                }


                NamedArgumentOccured = false;

                // second search for named arguments and replace the old values.
                for (int i = 0; i < paramsText.Count; i++)
                {
                    string namedParam = paramsText[i];

                    int NamedAssignOperatorIndex = namedParam.IndexOf(":=");

                    if (NamedAssignOperatorIndex >= 1)
                    {
                        string paramName = namedParam.Substring(0, NamedAssignOperatorIndex).Trim();
                        string paramVal = namedParam.Substring(NamedAssignOperatorIndex + 2).Trim();

                        if (fParameters.ContainsKey(paramName))
                        {
                            if (!setParameters[paramName]) //check if the parameter was set before.
                                fParameters[paramName] = paramVal;  //put the value in its corresponding parameter.
                            else
                                throw new QsException("Parameter '" + paramName + "' was set before");

                            setParameters[paramName] = true;
                        }
                        else 
                        {
                            throw new QsException("Named parameter '" + paramName + "' not found in the function: " + TargetFunction.FunctionBody);
                        }

                        NamedArgumentOccured = true; //to prevent normal arguments after named arguments.
                    }
                    else
                    {
                        if (NamedArgumentOccured) throw new QsException("Normal argument after named argument");
                        setParameters[TargetFunction.Parameters[i].Name] = true;
                    }

                }

                #endregion

                // Function exist in global heap of the scope 
                //  send values of corrected ordinal (positions parameters)
                return TargetFunction.GetInvokeExpression(this, fParameters.Values.ToArray<string>());
            }
            else
            {
                //Why we can't find the function in the global heap.

                if (this.ParseMode == QsVarParseModes.Function) // are we in function parsing mode?
                {
                    Expression FunP = ParameterFunction(functionName); //try to get the function from the parameter

                    if (FunP != null) return FunP;
                }

                throw new QsFunctionNotFoundException(functionName + " Can't be found.");

            }

        }


        /// <summary>
        /// Just take the left and right expression with the operator and make arithmatic expression.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="op"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private Expression ArithExpression(ExprOp eop, out short skip)
        {
            Expression left = eop.QuantityExpression;
            string op = eop.Operation;
            Expression right = eop.Next.QuantityExpression;

            skip = 1;

            Type aqType = typeof(QsValue);

            if (op == "^") return Expression.Power(left, right, aqType.GetMethod("Pow"));
            if (op == "^.") return Expression.Power(left, right, aqType.GetMethod("PowDot"));
            if (op == "^x") return Expression.Power(left, right, aqType.GetMethod("PowCross"));
            if (op == "*") return Expression.Multiply(left, right);
            if (op == ".") return Expression.Multiply(left, right, aqType.GetMethod("DotProduct"));
            if (op == "x") return Expression.Multiply(left, right, aqType.GetMethod("CrossProduct"));
            if (op == "/") return Expression.Divide(left, right);
            if (op == "%") return Expression.Modulo(left, right);
            if (op == "+") return Expression.Add(left, right);
            if (op == "-") return Expression.Subtract(left, right);

            if (op == "<") return Expression.LessThan(left, right);
            if (op == ">") return Expression.GreaterThan(left, right);
            if (op == "<=") return Expression.LessThanOrEqual(left, right);
            if (op == ">=") return Expression.GreaterThanOrEqual(left, right);

            if (op == "==") return Expression.Equal(left, right);
            if (op == "!=") return Expression.NotEqual(left, right);

            if (op.Equals("and", StringComparison.OrdinalIgnoreCase)) 
                return Expression.And(left, right);

            if (op.Equals("or", StringComparison.OrdinalIgnoreCase)) 
                return Expression.Or(left, right);

            if (op.Equals("when", StringComparison.OrdinalIgnoreCase))
            {
                // when -> condition -> otherwise -> result
                // here we must consume another expression for the false evaluation.
                //   if there are more expression on the linked list


                if (eop.Next.Next == null)
                {
                    throw new QsException("syntax error: When without Otherwise");
                }
                else
                {
                    skip = 2;

                    //eop:=when .Next=Expression .Next=otherwise .Net=expression

                    //There is a node test if it is a when node.
                    if (eop.Next.Next.Operation.Equals("when", StringComparison.OrdinalIgnoreCase))
                    {
                        // yes there is a node when that should its body processed 
                        short innerSkip;

                        var CompoundWhenExpression = ArithExpression(eop.Next.Next, out innerSkip);  //send the next when node

                        skip += innerSkip;  // to know how many nodes we skipped.

                        var WhenExpression = Expression.Condition(right, left, CompoundWhenExpression, typeof(QsValue));
                        return WhenExpression;
                    }
                    else
                    {
                        var WhenExpression = Expression.Condition(right, left, eop.Next.Next.QuantityExpression, typeof(QsValue));
                        return WhenExpression;
                    }
                
                }
            }

            throw new NotSupportedException("Not Supported Operator '" + op + "'");
        }

        /// <summary>
        /// Takes the linked list of formed expressions and construct the arithmatic expressions based
        /// on the priority of calculation operators.
        /// Passes:
        /// 1- { "^" }
        /// 2- { "*", "/", "%" /*modulus*/ }
        /// 3- { "+", "-" }
        /// </summary>
        /// <param name="FirstEop"></param>
        /// <returns></returns>
        private Expression ConstructExpression(ExprOp FirstEop)
        {
            //Treat operators as groups
            //  means * and /  are in the same pass
            //  + and - are in the same pass


            string[] Group = { "^" /* Power for normal product '*' */, 
                               "^." /* Power for dot product */ ,
                               "^x" /* Power for cross product */ };

            string[] Group1 = { "*" /* normal multiplication */, 
                                "." /* dot product */, 
                                "x" /* cross product */, 
                                "/" /* normal division */, 
                                "%" /*modulus*/ };

            string[] Group2 = { "+", "-" };

            string[] RelationalGroup = { "<", ">", "<=", ">=" };
            string[] EqualityGroup = { "==", "!=" };
            string[] AndGroup = { "and" };
            string[] OrGroup = { "or" };

            string[] WhenOtherwiseGroup = { "when", "otherwise"};


            string[][] OperatorGroups = { Group, Group1, Group2, RelationalGroup, EqualityGroup, AndGroup, OrGroup, WhenOtherwiseGroup };



            foreach (var opg in OperatorGroups)
            {
                ExprOp eop = FirstEop;

                //Pass for '[op]' and merge it  but from top to child :)  {forward)
                while (eop.Next != null)
                {
                    //if the operator in node found in the opg (current operator group) then execute the logic

                    if (opg.Count(c => c.Equals(eop.Operation, StringComparison.OrdinalIgnoreCase)) > 0)
                    {
                        short skip;
                        eop.QuantityExpression = ArithExpression(eop, out skip);

                        //drop eop.Next
                        if (eop.Next.Next != null)
                        {
                            while (skip > 0)
                            {
                                eop.Operation = eop.Next.Operation;

                                eop.Next = eop.Next.Next;

                                skip--;
                            }
                        }
                        else
                        {
                            //no more nodes exit the loop

                            eop.Next = null;      //last item were processed.
                            eop.Operation = string.Empty;
                        }
                    }
                    else
                    {
                        eop = eop.Next;
                    }
                }
            }

            return FirstEop.QuantityExpression;
        }


        /// <summary>
        /// The final result of the parsing and expression tree construction.
        /// </summary>
        public Expression ResultExpression
        {
            get;
            set;
        }
        #endregion

    }
}
