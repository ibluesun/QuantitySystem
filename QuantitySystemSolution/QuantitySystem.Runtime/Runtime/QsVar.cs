using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Linq.Expressions;
using Microsoft.Scripting;

using Microsoft.Scripting.Ast;

using ParticleLexer;
using ParticleLexer.TokenTypes;
using QuantitySystem;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities.DimensionlessQuantities;
using QuantitySystem.Units;
using System.Globalization;
using Qs.QsTypes;
using System.Reflection;


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

        /// <summary>
        /// Evaluate the function body taking into considerations
        /// the parameters of the lambda function.
        /// </summary>
        /// <param name="evaluator"></param>
        /// <param name="line"></param>
        /// <param name="lb"></param>
        public QsVar(QsEvaluator evaluator, string line, LambdaBuilder lb)
        {
            this.evaluator = evaluator;
            if (lb != null)
            {
                lambdaBuilder = lb;
                ParseMode = QsVarParseModes.Function;
            }
            ResultExpression = ParseArithmatic(line);

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
        /// Main parsing method.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private Expression ParseArithmatic(string line)
        {
            var tokens = Token.ParseText(line);

            tokens = tokens.RemoveSpaceTokens();                                            //remove all spaces
            tokens = tokens.MergeTokens(new WordToken());                           //discover words
            tokens = tokens.MergeTokens(new NumberToken());                   //discover the numbers
            tokens = tokens.MergeTokens(new UnitizedNumberToken());   //discover the unitized numbers

            tokens = tokens.GroupBrackets();                             // group (--()-) parenthesis
            tokens = tokens.DiscoverCalls();


            Expression quantityExpression = null;
            ExprOp eop = null;

            ExprOp FirstEop = null;

            int ix = 0;
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
                    
                    quantityExpression = Expression.Constant(Unit.ParseQuantity(q), typeof(AnyQuantity<double>)); //you have to explicitly tell expression the type because it searches for the operators and can't find them

                }
                else if (tokens[ix].TokenType == typeof(NumberToken))
                {
                    //ordinary number

                    AnyQuantity<double> qty = Unit.DiscoverUnit(QuantityDimension.Dimensionless).GetThisUnitQuantity<double>(double.Parse(q));

                    quantityExpression = Expression.Constant(qty, typeof(AnyQuantity<double>));

                }
                else
                {
                    if (ParseMode == QsVarParseModes.Function)
                    {
                        //get it from the parameters of the lambda
                        //  :) if it is found here then it will not be obtained from the global heap :)
                        //      now I understand how variable scopes occur :D

                        try
                        {
                            quantityExpression = lambdaBuilder.Parameters.Single(c => c.Name == q);
                        }
                        catch
                        {
                            //quantity variable  //get it from evaluator  global heap
                            quantityExpression = GetVariable(q);
                        }
                    }
                    else if (ParseMode == QsVarParseModes.Sequence)
                    {
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

                                quantityExpression = Expression.Call(typeof(Qs).GetMethod("ToQuantity", new Type[] { typeof(int) }), quantityExpression);

                            }
                            
                        }
                        catch
                        {
                            //quantity variable  //get it from evaluator  global heap
                            quantityExpression = GetVariable(q);
                        }

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
                    quantityExpression = Expression.Call(typeof(Gamma).GetMethod("Factorial"), quantityExpression);

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

        public Expression GetVariable(string name)
        {

            Type ScopeType = this.Evaluator.Scope.GetType();

            //store the scope
            var ScopeExp = Expression.Constant(Evaluator.Scope, ScopeType);

            var fe = Expression.Call(
                typeof(QsEvaluator).GetMethod("GetScopeQuantity"),
                ScopeExp, Expression.Constant(name));


            return fe;
        }

        public AnyQuantity<double> Execute()
        {
            Expression<Func<AnyQuantity<double>>> cq = Expression.Lambda<Func<AnyQuantity<double>>>
                (this.ResultExpression);

            Func<AnyQuantity<double>> aqf = cq.Compile();

            AnyQuantity<double> result = aqf();
            return result;
        }

        #region checks
        public static bool IsMatch(string line)
        {
            Match c1 = Regex.Match(line, SimpleArithmatic);
            Match c2 = Regex.Match(line, UnitizedNumber);
            Match c3 = Regex.Match(line, DoubleNumber);
            Match c4 = Regex.Match(line, VariableNameExpression);

            return c1.Success | c2.Success | c3.Success;

        }
        bool isUnitizedNumber(string str)
        {
            Match siso = Regex.Match(str, UnitizedNumber);
            return siso.Success;
        }

        bool isNumber(string str)
        {
            Match siso = Regex.Match(str, "^" + DoubleNumber + "$");
            return siso.Success;
        }
        #endregion

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

            string seqo = QsSequence.FormSequenceName(sequenceName, 1, parameters.Count);

            //get the sequence dynamically because sequence can be recursive :)

            Expression QsSequExpression = Expression.Call(typeof(QsSequence).GetMethod("GetSequence"),
                Expression.Constant(this.Evaluator.Scope),
                Expression.Constant(seqo)
                );

            #region Index manipulation
            Expression IndexExpression = null;
            Expression FromExpression = null;
            Expression ToExpression = null;

            // now the index could be
            // integer number
            int n;

            string methodName = "GetElementQuantity";

            Token itok = Token.ParseText(indexText);
            itok = itok.RemoveSpaceTokens();
            itok = itok.MergeTokens(new WordToken());
            itok = itok.MergeAllBut(new SequenceRangeToken(),typeof(WordToken));            
            if (itok.Count == 3 && itok[1].TokenType == typeof(SequenceRangeToken))
            {
                FromExpression = Expression.Call(typeof(Qs).GetMethod("IntegerFromQuantity"), ParseArithmatic(itok[0].TokenValue));
                ToExpression = Expression.Call(typeof(Qs).GetMethod("IntegerFromQuantity"), ParseArithmatic(itok[2].TokenValue));

                methodName = "SumElements";
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
                IndexExpression = Expression.Call(typeof(Qs).GetMethod("IntegerFromQuantity"), IndexExpression);

            }
            #endregion

            #region GetElementQuantity determination
            MethodInfo mi;
            if (methodName == "GetElementQuantity")
            {
                switch (parameters.Count)
                {
                    case 0:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int) });
                        break;
                    case 1:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(AnyQuantity<double>) });
                        break;
                    case 2:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>) });
                        break;
                    case 3:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>) });
                        break;
                    case 4:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>) });
                        break;
                    case 5:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>) });
                        break;
                    case 6:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>) });
                        break;
                    case 7:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>) });
                        break;
                    default:
                        throw new QsException("Number of sequence parameters exceed the built in functionality");
                }
                // insert the index parameter.
                parameters.Insert(0, IndexExpression);

            }
            else if (methodName == "SumElements")
            {
                switch (parameters.Count)
                {
                    case 0:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(int) });
                        break;
                    case 1:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(int), typeof(AnyQuantity<double>) });
                        break;
                    case 2:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(int), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>) });
                        break;
                    case 3:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(int), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>) });
                        break;
                    case 4:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(int), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>) });
                        break;
                    case 5:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(int), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>) });
                        break;
                    case 6:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(int), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>) });
                        break;
                    case 7:
                        mi = typeof(QsSequence).GetMethod(methodName, new Type[] { typeof(int), typeof(int), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>), typeof(AnyQuantity<double>) });
                        break;
                    default:
                        throw new QsException("Number of sequence parameters exceed the built in functionality");
                }
                // insert the index parameter.
                parameters.Insert(0, ToExpression);
                parameters.Insert(0, FromExpression);

            }
            else
            {
                throw new QsException("Sequence method not found.");
            }

            #endregion

            var finale = Expression.Call(QsSequExpression
                , mi
                , parameters.ToArray()
                );

            return finale;

        }

        /// <summary>
        /// Function Expression result.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Expression FunctionCallExpression(string functionName, Token args)
        {
            //fn(x,y,ff(y/x,e+fr(d)))     <== sample form :D


            string fn = functionName;

            List<Expression> parameters = new List<Expression>();

            //now parameters separated

            for (int ai = 1; ai < args.Count - 1; ai++ )
            {
                if (args[ai].TokenValue != ",")
                    parameters.Add(ParseArithmatic(args[ai].TokenValue));
            }

            string fnr = fn + "#" + parameters.Count; //to call the right function 

            object QsFunc;

            //find the function
            if (this.Evaluator.Scope.TryGetName(SymbolTable.StringToId(fnr), out QsFunc))
            {

                //I need to call the function by its reference
                //  which means get the function reference and make an expression which call it 
                //    how can I make this here?????
                
                
                Type ScopeType = this.Evaluator.Scope.GetType();

                //store the scope
                var ScopeExp = Expression.Constant(Evaluator.Scope, ScopeType);

                
                var fe = Expression.Call(
                    typeof(QsFunction).GetMethod("GetFunction"), 
                    ScopeExp, Expression.Constant(fnr));



                string param_count = parameters.Count.ToString(CultureInfo.InvariantCulture);


                return Expression.Invoke(Expression.Property(fe, "FunctionDelegate_" + param_count), parameters);

                

            }
            else
            {
                throw new QsException(fn + " function with " + parameters.Count.ToString() + " parameter(s) not found.");
            }


            
            
        }




        /// <summary>
        /// Just take the left and right expression with the operator and make arithmatic expression.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="op"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private Expression ArithExpression(Expression left, string op, Expression right)
        {
            Type aqType = typeof(AnyQuantity<double>);
            
            if (op == "^") return Expression.Power(left, right, aqType.GetMethod("Power"));
            if (op == "*") return Expression.Multiply(left, right);
            if (op == "/") return Expression.Divide(left, right);
            if (op == "%") return Expression.Modulo(left, right);
            if (op == "+") return Expression.Add(left, right);
            if (op == "-") return Expression.Subtract(left, right);

            throw new NotSupportedException("not supported operator");
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


            string[] Group = { "^" };
            string[] Group1 = { "*", "/", "%" /*modulus*/ };
            string[] Group2 = { "+", "-" };

            string[][] OperatorGroups = { Group, Group1, Group2 };


            foreach (var opg in OperatorGroups)
            {
                ExprOp eop = FirstEop;

                //Pass for '[op]' and merge it  but from top to child :)  {forward)
                while (eop.Next != null)
                {
                    //if the operator in node found in the opg (current operator group) then execute the logic

                    if (opg.Count(c => c.Equals(eop.Operation)) > 0)
                    {

                        eop.QuantityExpression = ArithExpression(eop.QuantityExpression, eop.Operation, eop.Next.QuantityExpression);

                        //drop eop.Next
                        if (eop.Next.Next != null)
                        {

                            eop.Operation = eop.Next.Operation;

                            eop.Next = eop.Next.Next;


                        }
                        else
                        {
                            //no more nodes exit the loop
                            eop = eop.Next;   //this will be always null.
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
