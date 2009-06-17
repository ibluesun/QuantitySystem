using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using QuantitySystem.Units;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem;
using QuantitySystem.Quantities.DimensionlessQuantities;
using System.Linq.Expressions;

namespace QuantitySystemCalculator
{

    /// <summary>
    /// used to evaluate the quantity system expressions.
    /// </summary>
    public class QsEvaluator
    {

        public const string UnitExpression = @"^\s*<(\w+)>\s*$";


        public const string UnitToUnitExpression = @"\s*<(\w+)>\s*[tT][oO]\s*<(\w+)>\s*";

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

            #region Match Unit "<kn>" 
            //match unit first
            Match m = Regex.Match(expr, UnitExpression);
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
                    Console.Error.WriteLine("Unit Not Found");
                }
                
                return;
            }
            #endregion

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
                    Console.WriteLine();
                    string cf = "    Conversion Factor => " + up.ConversionFactor.ToString();
                    Console.WriteLine(cf);

                    string dashes = "    ".PadRight(cf.Length, '-');
                    Console.WriteLine(dashes);
                    foreach (UnitPathItem upi in up) Console.WriteLine("    -> {0}", upi);
                }
                catch (UnitNotFoundException)
                {
                    Console.Error.WriteLine("Unit Not Found");
                }
                catch (UnitsNotDimensionallyEqualException)
                {
                    Console.Error.WriteLine("Units not dimensionally equal");
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
                    Console.Error.WriteLine("Variable must start with letter");
                    return;
                }
            }

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
                    Console.Error.WriteLine(nre.Message);
                }
                catch (QuantitiesNotDimensionallyEqualException)
                {
                    Console.Error.WriteLine("Quantities Not Dimensionally Equal");
                }
                catch (UnitNotFoundException)
                {
                    Console.Error.WriteLine("Unit Not Found");
                }
                catch (OverflowException)
                {
                    Console.Error.WriteLine("Overflow");
                }
            }

            #endregion

        }


        public void PrintUnitInfo(Unit unit)
        {
            Console.WriteLine("  Unit:        {0}", unit.ToString());
            Console.WriteLine("  Quantity:    {0}", unit.QuantityType.Name);
            Console.WriteLine("  Dimension:   {0}", unit.UnitDimension);
            Console.WriteLine("  Unit System: {0}", unit.UnitSystem);
        }

        public void PrintQuantity(BaseQuantity qty)
        {
            Console.WriteLine("  {0}", qty.ToString());
        }


        public void New()
        {
            variables = new Dictionary<string, AnyQuantity<double>>();
        }
    }

}
