using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;

namespace QuantitySystemCalculator
{
    class Program
    {
        static QsEvaluator qse = new QsEvaluator();
        static void Main(string[] args)
        {
            string line = string.Empty;

            
            StartConsole();


            while (CheckCommand(line))
            {
                
                if (!string.IsNullOrEmpty(line))
                {
                    qse.Evaluate(line);
                }

                Console.WriteLine();
                Console.Write("Qs> ");

                line = Console.ReadLine();

            }
        }


        static bool CheckCommand(string command)
        {
            command = command.ToLower();
            
            if(command == "quit") return false;
            
            if(command == "exit") return false;

            if (command == "help") PrintHelp();

            if (command.StartsWith("list"))
            {
                string param = string.Empty;

                if(command.Length>4) param = command.Substring(5);

                if (string.IsNullOrEmpty(param))
                    ListVariables();
                else
                {
                    if (param == "quantities")
                        ListQuantities();
                    if (param == "units")
                        ListUnits();
                }
            }

            if (command == "new") qse.New();

            if (command == "cls") Console.Clear();

            return true;

        }


        static void StartConsole()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            Console.WriteLine("Quantity System 1.0 Calculator");
            Console.WriteLine("Copyright 2009 By Ahmed Sadek   http://QuantitySystem.CodePlex.com");
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("Type \"help\" for more information.");

            Console.WriteLine();
            Console.WriteLine();
        }

        static void PrintHelp()
        {

            Console.WriteLine();
            Console.WriteLine("    Type \"<unit>\" for information [case sensitive]");
            Console.WriteLine("    Example: <kn> for knot");
            Console.WriteLine("             <m> for Meter");
            Console.WriteLine("             <kg> for KiloGram");
            Console.WriteLine("             <ft> for Foot");
            Console.WriteLine();
            Console.WriteLine("    Type \"variable = number<unit>\" for quantity");
            Console.WriteLine("         omit <unit> for dimensionless quantity");
            Console.WriteLine();
            Console.WriteLine("    Type \"variable = number[Quantity Name]\" ");
            Console.WriteLine("         to make a variable with default SI units of the quantity");
            Console.WriteLine();
            Console.WriteLine("    Type \"variable\" alone to show its information");
            Console.WriteLine();
            Console.WriteLine("    Type \"<unit> to <unit>\" for conversion factor");
            Console.WriteLine();
            Console.WriteLine("    Type \"List\" for list of current variables");
            Console.WriteLine("    Type \"      Quantities\" for list of current variables");
            Console.WriteLine("    Type \"      Units\" for list of current variables");
            Console.WriteLine();
            Console.WriteLine("    Type \"New\" to clear all variables.");
            Console.WriteLine();
            Console.WriteLine("    Type \"Cls\" to clear the screen.");
            Console.WriteLine();
            Console.WriteLine("    Type \"Exit\" to terminate the console.");


        }

        static void ListVariables()
        {
            foreach (string var in qse.Variables.Keys)
            {
                Console.WriteLine("    " + var + "= " + qse.Variables[var].ToString());
            }
        }

        static void ListQuantities()
        {
            foreach (Type QType in QuantitySystem.QuantityDimension.CurrentQuantitiesDictionary.Values)
            {
                Console.WriteLine("    " + QType.Name.Substring(0, QType.Name.Length - 2));

            }
        }

        static void ListUnits()
        {
            foreach (Type utype in Unit.UnitTypes)
            {
                QuantitySystem.Attributes.UnitAttribute ua =  Unit.GetUnitAttribute(utype);
                if (ua != null)
                {
                    string uname = utype.Name.PadRight(16);
                    string symbol = "<" + ua.Symbol + ">";
                    symbol = symbol.PadRight(10);
                    string system =  utype.Namespace.Substring("QuantitySystem.Units".Length + 1);


                    Console.WriteLine("    " + uname+" "+ symbol+" "+system);
                }
            }
        }
    }
}
