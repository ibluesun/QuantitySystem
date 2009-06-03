using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using QuantitySystem.Units;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem;

namespace QuantitySystemCalculator
{

    /// <summary>
    /// used to evaluate the quantity system expressions.
    /// </summary>
    class QsEvaluator
    {

        public const string UnitExpression = @"^<(.+)>$";



        public const string VariableDimensionlessUnitExpression = @"^(\w+)\s*=\s*(\d+)\s*$";
        
        public const string VariableUnitExpression = @"^(\w+)\s*=\s*(\d+)\s*(<(.+)>)$";
        //Match	           $1	$2	$3	$4
        //a= 5	            a	5		
        //a = 6<m>	        a	6	<m>	m
        //B= 900 <m/s>	    B	900	<m/s>	m/s
        //c = 5000 <ft/s>	c	5000	<ft/s>	ft/s

        public const string VariableQuantityExpression = @"^(\w+)\s*=\s*(\d+)\s*(\[(.+)\])$";



        public const string VariableNameExpression = @"^\s*(\w+)\s*$";

        public const string SimpleOperationsExpression = @"(\w+)\s*([\+\-\/\*])\s*(\w+)";
        //Match	$1	$2	$3
        //a + b	a	+	b
        //a -b	a	-	b
        //a/d	a	/	d
        //g* d	g	*	d


        public const string SimpleOperationAssignmentExpression = @"(\w+)\s*=\s*(\w+)\s*([\+\-\/\*])\s*(\w+)";
        //Match	    $1	$2	$3	$4
        //c= a + b	c	a	+	b
        //f=g/m	f	g	/	m


        public Dictionary<string, AnyQuantity<double>> variables = new Dictionary<string, AnyQuantity<double>>();


        public void Evaluate(string line)
        {
            //match unit first
            Match m = Regex.Match(line, UnitExpression);
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



            m = Regex.Match(line, VariableDimensionlessUnitExpression);
            if (m.Success)
            {
                //dimensionless quantity

                string varName = m.Groups[1].Value;
                double varVal = double.Parse(m.Groups[2].Value);

                AnyQuantity<double> qty = null;
                qty = new QuantitySystem.Quantities.DimensionlessQuantities.DimensionlessQuantity<double>();
                qty.Value = varVal;

                variables[varName] = qty;
                PrintQuantity(qty);
                return;

            }

            //match variable assignation
            m = Regex.Match(line, VariableUnitExpression);
            if (m.Success)
            {
                //get the variable name
                string varName = m.Groups[1].Value;
                double varVal = double.Parse(m.Groups[2].Value);

                AnyQuantity<double> qty = null;

                if (!string.IsNullOrEmpty(m.Groups[3].Value))
                {
                    try
                    {
                        //get the unit 
                        Unit u = Unit.Parse(m.Groups[4].Value);
                        //get the quantity
                        qty = u.GetThisUnitQuantity<double>();
                        qty.Unit = u;
                    }
                    catch (UnitNotFoundException)
                    {
                        Console.Error.WriteLine("Unit Not Found");
                        return;
                    }

                }


                qty.Value = varVal;

                variables[varName] = qty;
                PrintQuantity(qty);
                return;
            }

            //match variable assignation with quantity
            m = Regex.Match(line, VariableQuantityExpression);
            if (m.Success)
            {
                //get the variable name
                string varName = m.Groups[1].Value;
                double varVal = double.Parse(m.Groups[2].Value);

                AnyQuantity<double> qty = null;

                if (!string.IsNullOrEmpty(m.Groups[3].Value))
                {
                    try
                    {
                        //get the quantity
 
                        qty = AnyQuantity<double>.Parse(m.Groups[4].Value);

                        //get the quantity

                        qty.Unit = new Unit(qty); 

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

            //match variable name
            m = Regex.Match(line, VariableNameExpression);
            if (m.Success)
            {
                string varName = m.Groups[1].Value;
                if (variables.ContainsKey(varName))
                {
                    var qty = variables[varName];
                    PrintQuantity(qty);
                }
                return;
            }


            //match simple operation {direct arithmatic operation}
            m = Regex.Match(line, SimpleOperationAssignmentExpression);
            if (m.Success)
            {
                string op = m.Groups[3].Value;

                AnyQuantity<double> left = null;
                AnyQuantity<double> right = null;

                try
                {

                    left = variables[m.Groups[2].Value];
                    right = variables[m.Groups[4].Value];
                }
                catch (KeyNotFoundException)
                {
                    Console.Error.WriteLine("Variable Not Found");
                    return;
                }


                //the new variable name
                var varName = m.Groups[1].Value;

                AnyQuantity<double> result = null;

                try
                {
                    if (op == "+") result = left + right;
                    if (op == "-") result = left - right;
                    if (op == "*") result = left * right;
                    if (op == "/") result = left / right;

                    variables[varName] = result;
                    PrintQuantity(result);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("  {0}", ex.GetType().Name);
                }
                return;
            }

            //match simple operation {direct arithmatic operation}
            m = Regex.Match(line, SimpleOperationsExpression);
            if (m.Success)
            {
                string op = m.Groups[2].Value;

                AnyQuantity<double> left = null;
                AnyQuantity<double> right = null;

                try
                {
                    left = variables[m.Groups[1].Value];
                    right = variables[m.Groups[3].Value];
                }
                catch (KeyNotFoundException)
                {
                    Console.Error.WriteLine("Variable Not Found");
                    return;
                }

                AnyQuantity<double> result = null;

                try
                {
                    if (op == "+") result = left + right;
                    if (op == "-") result = left - right;
                    if (op == "*") result = left * right;
                    if (op == "/") result = left / right;

                    PrintQuantity(result);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("  {0}", ex.GetType().Name);
                }
                return;
            }

        }

        public void PrintUnitInfo(Unit unit)
        {
            Console.WriteLine("  Unit:        {0}", unit.ToString());
            Console.WriteLine("  Quantity:    {0}", unit.QuantityType.Name);
            Console.WriteLine("  Unit System: {0}", unit.UnitSystem);
        }

        public void PrintQuantity(BaseQuantity qty)
        {
            Console.WriteLine("  {0}", qty.ToString());
        }
    }
}
