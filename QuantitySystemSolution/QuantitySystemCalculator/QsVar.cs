using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using QuantitySystem;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities.DimensionlessQuantities;
using QuantitySystem.Units;
using Microsoft.Linq.Expressions;

namespace QuantitySystemCalculator
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



        public const string VariableNameExpression = @"^\s*(\w+)\s*$";

        const string DoubleNumber = @"[-+]?\d+(\.\d+)*([eE][-+]?\d+)*";

        const string UnitizedNumber = "(?<num>" + DoubleNumber + ")\\s*<(?<unit>.+?)>";

        const string SimpleArithmatic = "\\s*(?<q>(" + UnitizedNumber + ")|(" + DoubleNumber + ")|\\w+)\\s*(?<op>[\\+\\-\\/\\*])?";

        public static bool IsMatch(string line)
        {
            Match c1 = Regex.Match(line, SimpleArithmatic);
            Match c2 = Regex.Match(line, UnitizedNumber);
            Match c3 = Regex.Match(line, DoubleNumber);
            Match c4 = Regex.Match(line, VariableNameExpression);

            return c1.Success | c2.Success | c3.Success;

        }

        public class ExprOp
        {
            public Expression QuantityExpression { get; set; }
            public string Operation { get; set; }
            public ExprOp Next { get; set; }

        }



        ExprOp ParentEop = null;


        public QsVar(QsEvaluator evaluator, string line)
        {
            this.evaluator = evaluator;


            Match varMatch = null;

            varMatch = Regex.Match(line, SimpleArithmatic);

            Expression quantityExpression = null;
            ExprOp eop = null;

            while (varMatch.Success)
            {
                string q = varMatch.Groups["q"].Value;
                string op = varMatch.Groups["op"].Value;


                if (isUnitizedNumber(q))
                {
                    //unitized number
                    quantityExpression = QuantityExpression(q);

                }
                else if (isNumber(q))
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
                    ParentEop = new ExprOp();
                    eop = ParentEop;

                }
                else
                {
                    //use the next object to be eop.
                    eop.Next = new ExprOp();
                    eop = eop.Next;
                }

                eop.Operation = op;
                eop.QuantityExpression = quantityExpression;


                varMatch = varMatch.NextMatch();
            }

        }

        public Expression RecurExpr(ExprOp eop)
        {
            if (string.IsNullOrEmpty(eop.Operation))
            {
                return eop.QuantityExpression;
            }
            else
            {
                if (eop.Operation == "+") return Expression.Add(eop.QuantityExpression, RecurExpr(eop.Next));
                if (eop.Operation == "-") return Expression.Subtract(eop.QuantityExpression, RecurExpr(eop.Next));
                if (eop.Operation == "*") return Expression.Multiply(eop.QuantityExpression, RecurExpr(eop.Next));
                if (eop.Operation == "/") return Expression.Divide(eop.QuantityExpression, RecurExpr(eop.Next));
                throw new NotSupportedException();
            }
        }

        public Expression CalcExpression
        {
            get
            {
                return RecurExpr(ParentEop);
            }
        }

        public AnyQuantity<double> Execute()
        {
            Expression<Func<AnyQuantity<double>>> cq = Expression.Lambda<Func<AnyQuantity<double>>>
                (this.CalcExpression);

            Func<AnyQuantity<double>> aqf = cq.Compile();

            AnyQuantity<double> result = aqf();
            return result;
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



    }
}
