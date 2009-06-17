using System;
using QuantitySystem.Units;

using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities.DimensionlessQuantities;
using QuantitySystem;
using System.Reflection;
using System.Globalization;



namespace QuantitySystemCalculator
{
    class Program
    {


        static void Main(string[] args)
        {
            Run();
        }





        #region evaluator console

        static QsEvaluator qse = new QsEvaluator();
        static bool CommandProcessed = false;

        static void Run()
        {


            string line = string.Empty;


            StartConsole();


            while (CheckCommand(line))
            {
                if (!CommandProcessed)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        qse.Evaluate(line);
                    }

                }
                CommandProcessed = false;
                Console.WriteLine();
                Console.Write("Qs> ");

                line = Console.ReadLine();
            }
        }




        static void StartConsole()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            PrintCopyright();

            Console.WriteLine();
            Console.WriteLine();
        }

        static void PrintCopyright()
        {

            var qsc_ver = (AssemblyFileVersionAttribute)Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0];

            var lib_ver = (AssemblyFileVersionAttribute)Assembly.GetAssembly(typeof(QuantityDimension)).GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0];


            Console.WriteLine("Quantity System Calculator ver " + qsc_ver.Version);
            
            Console.WriteLine("Quantity System Framework  ver " + lib_ver.Version);


            var qsc_cwr = (AssemblyCopyrightAttribute)Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0];

            Console.WriteLine(qsc_cwr.Copyright);

            Console.WriteLine(); 
            Console.WriteLine("Type \"help\" for more information.");
        }

        /// <summary>
        /// Help Command.
        /// </summary>
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
            Console.WriteLine("    Type \"      Quantities\" for list of current quantities");
            Console.WriteLine("    Type \"      Units\" for list of current units");
            Console.WriteLine();
            Console.WriteLine("    Type \"New\" to clear all variables.");
            Console.WriteLine();
            Console.WriteLine("    Type \"Cls\" to clear the screen.");
            Console.WriteLine();
            Console.WriteLine("    Type \"Exit\" to terminate the console.");


        }


        /// <summary>
        /// Console Commands.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
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

                CommandProcessed = true;
            }

            if (command == "new")
            {
                qse.New();
                CommandProcessed = true;
            }

            if (command == "cls")
            {
                Console.Clear();
                CommandProcessed = true;
            }

            return true;

        }

        /// <summary>
        /// List Command
        /// </summary>
        static void ListVariables()
        {
            foreach (string var in qse.Variables.Keys)
            {
                Console.WriteLine("    " + var + "= " + qse.Variables[var].ToString());
            }
        }

        /// <summary>
        /// List Quantities Command
        /// </summary>
        static void ListQuantities()
        {
            foreach (Type QType in QuantitySystem.QuantityDimension.CurrentQuantitiesDictionary.Values)
            {
                Console.WriteLine("    " + QType.Name.Substring(0, QType.Name.Length - 2));

            }
        }


        /// <summary>
        /// List Units Command
        /// </summary>
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
                    string system =  utype.Namespace.Substring("QuantitySystem.Units".Length + 1).PadRight(12);

                    string qtype = ua.QuantityType.ToString().Substring(ua.QuantityType.Namespace.Length + 1).TrimEnd("`1[T]".ToCharArray());


                    Console.WriteLine("    " + uname + " " + symbol + " " + system + qtype);
                }
            }
        }

        #endregion
    }



}
