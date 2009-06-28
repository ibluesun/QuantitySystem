using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using QuantitySystem;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities.DimensionlessQuantities;
using QuantitySystem.Units;
using Microsoft.Linq.Expressions;
using System.Linq;


namespace QuantitySystem.Runtime
{
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


        public QsVar(QsEvaluator evaluator, string line)
        {
            this.evaluator = evaluator;

            CalcExpression = ParseArithmatic(line);
        }


        private Expression ParseArithmatic(string line)
        {
            var tokens = Token.ParseText(line);

            tokens = tokens.RemoveSpaceTokens();                                            //remove all spaces
            tokens = tokens.MergeTokens("\\w+", TokenClass.Word);                           //discover words
            tokens = tokens.MergeTokens(Token.Number, TokenClass.Number);                   //discover the numbers
            tokens = tokens.MergeTokens(Token.UnitizedNumber, TokenClass.UnitizedNumber);   //discover the unitized numbers

            tokens = tokens.GroupParenthesis();                             // group (--()-) parenthesis

            Expression quantityExpression = null;
            ExprOp eop = null;

            ExprOp FirstEop = null;

            int ix = 0;
            while (ix < tokens.Count)
            {
                string q = tokens[ix].TokenValue;
                string op = ix + 1 < tokens.Count ? tokens[ix + 1].TokenValue : string.Empty;


                if (tokens[ix].TokenClass == TokenClass.Group)
                {
                    quantityExpression = ParseArithmatic(q.Substring(1, q.Length - 2));
                }
                else if (tokens[ix].TokenClass == TokenClass.UnitizedNumber)
                {
                    //unitized number
                    quantityExpression = QuantityExpression(q);

                }
                else if (tokens[ix].TokenClass == TokenClass.Number)
                {
                    //ordinary number
                    quantityExpression = DimensionlessQuantityExpression(double.Parse(q));

                }
                else
                {
                    try
                    {
                        //quantity variable  //get it from hash
                        quantityExpression = Expression.Constant(evaluator.Variables[q], typeof(AnyQuantity<double>));
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new NullReferenceException("Variable Not Found");

                    }

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


            return  CalculateExpression(FirstEop);

        }

        public AnyQuantity<double> Execute()
        {
            Expression<Func<AnyQuantity<double>>> cq = Expression.Lambda<Func<AnyQuantity<double>>>
                (this.CalcExpression);

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
        public static Expression DimensionlessQuantityExpression(double val)
        {
            DimensionlessQuantity<double> qty = new DimensionlessQuantity<double>();

            qty.Unit = Unit.DiscoverUnit(QuantityDimension.Dimensionless);
            qty.Value = val;

            return Expression.Constant(qty, typeof(AnyQuantity<double>));
        }


        public static Expression QuantityExpression(string expr)
        {
            // I was naieve 
            // why should I use expression 
            //   when I want to represent code formation in memory
            //   however all I want is AnyQuantity<> object and it doesn't need all of this hassle about dynamically creation
            // I was stupid :)

            Match um = Regex.Match(expr, UnitizedNumber);
            if (um.Success)
            {
                string varUnit = um.Groups["unit"].Value;
                double val = double.Parse(um.Groups["num"].Value);

                Unit un = Unit.Parse(varUnit);
                AnyQuantity<double> qty = un.GetThisUnitQuantity<double>(val);

                return Expression.Constant(qty, typeof(AnyQuantity<double>)); //you have to explicitly tell expression the type because it searches for the operators and can't find them
            }
            else
            {
                throw new NotSupportedException();
            }
        }


        private Expression ArithExpression(Expression left, string op, Expression right)
        {
            if (op == "*") return Expression.Multiply(left, right);
            if (op == "/") return Expression.Divide(left, right);
            if (op == "%") return Expression.Modulo(left, right);
            if (op == "+") return Expression.Add(left, right);
            if (op == "-") return Expression.Subtract(left, right);
            throw new NotSupportedException("not supported operator");
        }

        private Expression CalculateExpression(ExprOp FirstEop)
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


        public Expression CalcExpression
        {
            get;
            set;
        }
        #endregion

    }
}
