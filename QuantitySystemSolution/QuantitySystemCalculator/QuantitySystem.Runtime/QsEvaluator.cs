using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using QuantitySystem;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;

namespace QuantitySystem.Runtime
{

    /// <summary>
    /// used to evaluate the quantity system expressions.
    /// </summary>
    public class QsEvaluator
    {

        public const string UnitExpression = @"^\s*<(.+?)>\s*$";

        public const string UnitToUnitExpression = @"^\s*<(.+)>\s*[tT][oO]\s*<(.+)>\s*$";

        public const string DoubleNumber = @"[-+]?\d+(\.\d+)*([eE][-+]?\d+)*?";

        public const string VariableQuantityExpression = @"^(\w+)\s*=\s*(" + DoubleNumber + @")\s*(\[(.+)\])";

        private Dictionary<string, AnyQuantity<double>> variables = new Dictionary<string, AnyQuantity<double>>();
        public Dictionary<string, AnyQuantity<double>> Variables
        {
            get
            {
                return variables;
            }
        }


        public void Evaluate(string expr)
        {

            Match m = null;

            #region Match <unit> to <unit>

            //match unit to unit
            m = Regex.Match(expr, UnitToUnitExpression);
            if (m.Success)
            {
                //evaluate unit

                try
                {
                    Unit u1 = Unit.Parse(m.Groups[1].Value);
                    Unit u2 = Unit.Parse(m.Groups[2].Value);
                    //PrintUnitInfo(u);
                    UnitPath up = u1.PathToUnit(u2);

                    Console.ForegroundColor = ConsoleColor.Gray;

                    Console.WriteLine();

                    string cf = "    Conversion Factor => " + up.ConversionFactor.ToString();

                    foreach (UnitPathItem upi in up) Console.WriteLine("    -> {0}", upi);

                    string dashes = "    ".PadRight(cf.Length, '-');

                    Console.WriteLine(dashes);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(cf);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch (UnitNotFoundException)
                {
                    PrintError("Unit Not Found");
                }
                catch (UnitsNotDimensionallyEqualException)
                {
                    PrintError("Units not dimensionally equal");
                }

                return;
            }
            #endregion

            #region Match Unit "<kn>" 
            //match one unit first
            m = Regex.Match(expr, UnitExpression);
            if (m.Success)
            {
                //evaluate unit

                try
                {
                    Unit u = Unit.Parse(m.Groups[1].Value);
                    PrintUnitInfo(u);
                }
                catch (UnitNotFoundException)
                {
                    PrintError("Unit Not Found");
                }
                
                return;
            }
            #endregion

            string varName = string.Empty;

            #region Match variable Assignation with quantity "a=40[Acceleration]"
            //match variable assignation with quantity
            m = Regex.Match(expr, VariableQuantityExpression);
            if (m.Success)
            {
                //get the variable name
                varName = m.Groups[1].Value;
                double varVal = double.Parse(m.Groups[2].Value);

                AnyQuantity<double> qty = null;

                if (!string.IsNullOrEmpty(m.Groups[5].Value))
                {
                    try
                    {
                        //get the quantity
 
                        qty = AnyQuantity<double>.Parse(m.Groups[6].Value);

                        //get the quantity

                        qty.Unit = Unit.DiscoverUnit(qty); 

                    }
                    catch (QuantityNotFoundException)
                    {
                        Console.Error.WriteLine("Quantityt Not Found");
                        return;
                    }

                }

                qty.Value = varVal;

                variables[varName] = qty;
                PrintQuantity(qty);
                return;
            }
            #endregion


            #region expression
            //check if the line has '='
            string line = expr;
            

            if (expr.IndexOf('=') > -1)
            {
                string[] ls = expr.Split('=');
                line = ls[1];
                varName = ls[0].Trim();

                if (char.IsNumber(varName[0]))
                {
                    PrintError("Variable must start with letter");
                    return;
                }
            }

            //check if the line has '(' ')'


            if (QsVar.IsMatch(line))
            {
                try
                {

                    QsVar qv = new QsVar(this, line);
                    if (!string.IsNullOrEmpty(varName))
                    {
                        //assign the variable
                        variables[varName] = qv.Execute();
                        PrintQuantity(variables[varName]);
                    }
                    else
                    {
                        //only print the result.
                        PrintQuantity(qv.Execute());
                    }

                }
                catch (NullReferenceException nre)
                {
                    PrintError(nre.Message);
                }
                catch (QuantitiesNotDimensionallyEqualException)
                {
                    PrintError("Quantities Not Dimensionally Equal");
                }
                catch (UnitNotFoundException)
                {
                    PrintError("Unit Not Found");
                }
                catch (OverflowException)
                {
                    PrintError("Overflow");
                }
            }

            #endregion

        }


        public void PrintError(string str)
        {
            ConsoleColor cc = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(str);

            Console.ForegroundColor = ConsoleColor.White;
        }

        public void PrintUnitInfo(Unit unit)
        {
            

            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("    Unit:        {0}", unit.ToString());
            Console.WriteLine("    Quantity:    {0}", unit.QuantityType.Name);
            Console.WriteLine("    Dimension:   {0}", unit.UnitDimension);
            Console.WriteLine("    Unit System: {0}", unit.UnitSystem);

            Console.ForegroundColor = ConsoleColor.White;
        }

        public void PrintQuantity(BaseQuantity qty)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            
            Console.WriteLine("    {0}", qty.ToString());

            Console.ForegroundColor = ConsoleColor.White;
        }


        public void New()
        {
            variables = new Dictionary<string, AnyQuantity<double>>();
        }
    }

}
