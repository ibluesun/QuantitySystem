using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;
using QuantitySystem;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using Microsoft.Linq.Expressions;


namespace Qs.Runtime
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





        Dictionary<string, AnyQuantity<double>> _Variables;

        public IEnumerable<string> VariablesKeys
        {
            get
            {
                if (Scope != null)
                {
                    var varo = from item in Scope.Items
                               select SymbolTable.IdToString(item.Key);
                    return varo;
                }
                else
                {
                    var varo = from item in _Variables.Keys
                               select item;
                    return varo;
                }
            }
        }


        public Scope Scope { get; set; }

        public void New()
        {
            _Variables = new Dictionary<string, AnyQuantity<double>>();

            if (Scope != null)
            {
                Scope.Clear();
            }
        }



        public object GetVariable(string varName)
        {
            if (Scope != null)
            {
                object q;
                Scope.TryGetName(SymbolTable.StringToId(varName), out q);
                return q;
            }
            else
            {
                throw new NotImplementedException("you should be running in DLR");

            }

        }

        public AnyQuantity<double> GetQuantity(string varName)
        {
            if (Scope != null)
            {
                object q;
                Scope.TryGetName(SymbolTable.StringToId(varName), out q);
                return (AnyQuantity<double>)q;
            }
            else
            {
                return _Variables[varName];
            }
        }

        public void SetVariable(string varName, AnyQuantity<double> varValue)
        {
            if (Scope != null)
            {
                Scope.SetName(SymbolTable.StringToId(varName), varValue);
            }
            else
            {
                _Variables[varName] = varValue;
            }
        }

        public static AnyQuantity<double> GetScopeQuantity(Scope scope, string name)
        {
            object q;


            scope.TryGetName(SymbolTable.StringToId(name), out q);


            return (AnyQuantity<double>)q;

        }


        public object Evaluate(string expr)
        {
            if (string.IsNullOrEmpty(expr)) return null;
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

                    return up.ConversionFactor;
                }
                catch (UnitNotFoundException)
                {
                    PrintError("Unit Not Found");
                }
                catch (UnitsNotDimensionallyEqualException)
                {
                    PrintError("Units not dimensionally equal");
                }

                return null;
            }
            #endregion

            #region Match Unit <unit> like  <kn>, <m>, <kW> etc.
            //match one unit first
            m = Regex.Match(expr, UnitExpression);
            if (m.Success)
            {
                //evaluate unit

                try
                {
                    Unit u = Unit.Parse(m.Groups[1].Value);
                    PrintUnitInfo(u);
                    return u;
                }
                catch (UnitNotFoundException)
                {
                    PrintError("Unit Not Found");
                }
                
                return null;
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
                        return null;
                    }

                }

                qty.Value = varVal;

                SetVariable(varName, qty);
                
                PrintQuantity(qty);
                return qty;
            }
            #endregion


            #region expression

            //check if the line has '='
            return ExtraEvaluate(expr);


            #endregion

        }


        internal object ExtraEvaluate(string line)
        {
            //test for lambda expression 
            // f(x,y,z)=> x + y + (z/2.04 * 32<kg>)

            QsFunction func = QsFunction.ParseFunction(this, line);
            if(func!=null)
            {
                //store the expression for later use 
                Scope.SetName(SymbolTable.StringToId(func.FunctionName), func);
                return func;
            }
            else
            {
                string varName = string.Empty;

                if (line.IndexOf('=') > -1)
                {
                    string[] ls = line.Split('=');
                    line = ls[1];
                    varName = ls[0].Trim();

                    if (char.IsNumber(varName[0]))
                    {
                        PrintError("Variable must start with letter");
                        return null;
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
                            SetVariable(varName, qv.Execute());
                            var q = GetQuantity(varName);
                            PrintQuantity(q);
                            return q;
                        }
                        else
                        {
                            //only print the result.
                            var q = qv.Execute();
                            PrintQuantity(q);
                            return q;
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
                    catch (QuantityException qe)
                    {
                        PrintError(qe.Message);
                    }
                    catch (UnitNotFoundException)
                    {
                        PrintError("Unit Not Found");
                    }
                    catch (OverflowException)
                    {
                        PrintError("Overflow");
                    }
                    catch (QsException qse)
                    {
                        PrintError(qse.Message);
                    }
                    return null;
                }
                return null;
            }
        }


        #region print information helpers
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

        #endregion
    }

}
