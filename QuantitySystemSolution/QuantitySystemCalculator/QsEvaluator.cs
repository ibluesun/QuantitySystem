using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using QuantitySystem.Units;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystemCalculator
{

    /// <summary>
    /// used to evaluate the quantity system expressions.
    /// </summary>
    class QsEvaluator
    {

        public const string UnitExpression = @"^<(.+)>$";
        
        
        public const string VariableExpression = @"(\w+)\s*=\s*(\d+)\s*(<(.+)>)?";
        //Match	           $1	$2	$3	$4
        //a= 5	            a	5		
        //a = 6<m>	        a	6	<m>	m
        //B= 900 <m/s>	    B	900	<m/s>	m/s
        //c = 5000 <ft/s>	c	5000	<ft/s>	ft/s


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
                    Console.WriteLine("  Unit Not Found");
                }
                
                return;
            }

            //match variable assignation
            m = Regex.Match(line, VariableExpression);
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
                        Console.WriteLine("  Unit Not Found");
                        return;
                    }                   

                }
                else
                {
                    //dimensionless quantity
                    qty = new QuantitySystem.Quantities.DimensionlessQuantities.DimensionlessQuantity<double>();

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
                var left = variables[m.Groups[2].Value];
                var right = variables[m.Groups[4].Value];
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
                    Console.WriteLine("  {0}", ex.GetType().Name);
                }
                return;
            }

            //match simple operation {direct arithmatic operation}
            m = Regex.Match(line, SimpleOperationsExpression);
            if (m.Success)
            {
                string op = m.Groups[2].Value;
                var left = variables[m.Groups[1].Value];
                var right = variables[m.Groups[3].Value];

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
                    Console.WriteLine("  {0}", ex.GetType().Name);
                }
                return;
            }

        }

        public void PrintUnitInfo(Unit unit)
        {
            Console.WriteLine("  Unit:        {0} {1}", unit.GetType().Name, unit.Symbol);
            Console.WriteLine("  Quantity:    {0}", unit.QuantityType.Name);
            Console.WriteLine("  Unit System: {0}", unit.UnitSystem);
        }

        public void PrintQuantity(BaseQuantity qty)
        {
            Console.WriteLine("  {0}", qty.ToString());
        }
    }
}
