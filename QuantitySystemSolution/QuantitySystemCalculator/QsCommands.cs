using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Qs.Runtime;
using Microsoft.Scripting.Hosting;
using QuantitySystem.Units;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting;

    public static class QsCommands
    {

        public static bool CommandProcessed;


        public static ScriptEngine Engine { get; set; }
        public static ScriptScope ScriptScope { get; set; }
        public const ConsoleColor BackgroundColor = ConsoleColor.White;
        public const ConsoleColor ForegroundColor = ConsoleColor.Black;
        public const ConsoleColor HelpColor = ConsoleColor.Blue;

        public const ConsoleColor ValuesColor = ConsoleColor.Green;
        public const ConsoleColor ExceptionColor = ConsoleColor.Red;

        /// <summary>
        /// Console Commands.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static bool CheckCommand(string command, Scope scope)
        {
            string[] commands = command.ToLower().Split(' ');

            //remove unnessacary spaces.
            for (int i = 0; i < commands.Length; i++)
                commands[i] = commands[i].Trim();

            if (commands[0] == "quit") return false;

            if (commands[0] == "exit")
            {
                Console.WriteLine("Press CTRL+Z to exit");
                CommandProcessed = true;
            }

            if (commands[0] == "help")
            {
                PrintHelp();
                CommandProcessed = true;
            }

            if (commands[0] == "list")
            {
                string param = string.Empty;

                if (commands.Length < 2)
                {
                    ListVariables(scope);
                }
                else
                {
                    if (commands[1] == "quantities")
                        ListQuantities();

                    if (commands[1] == "units")
                    {
                        if (commands.Length < 3)
                        {
                            ListUnits(string.Empty);
                        }
                        else
                        {
                            ListUnits(commands[2]);
                        }
                    }

                    if (commands[1] == "prefixes")
                    {
                        ListMetricPrefixes();
                    }
                }

                CommandProcessed = true;
            }

            if (commands[0] == "new")
            {
                ScopeStorage ss = (ScopeStorage)scope.Storage;
                string[] names = (from name in ss.GetItems() select name.Key).ToArray();
                foreach (var name in names)
                    ss.DeleteValue(name, true);

                GC.Collect();

                CommandProcessed = true;
            }

            if (commands[0] == "cls")
            {
                Console.Clear();
                CommandProcessed = true;
            }

            if (commands[0] == "copyright")
            {
                PrintCopyright();
                CommandProcessed = true;
            }

            if (commands[0] == "run")
            {
                //then we want to load a text file for evaluating its contents
                // and adding its content to this session.

                if (commands.Length > 1)
                {
                    string file = commands[1];

                    ScriptScope.Engine.ExecuteFile(file, ScriptScope);

                    CommandProcessed = true;
                }
            }
            return true;
        }

        private static void ListMetricPrefixes()
        {
            Console.ForegroundColor = HelpColor;

            int[] exo = { 24, 21, 18, 15, 12, 9, 6, 3, 2, 1, 0, -1, -2, -3, -6, -9, -12, -15, -18, -21, -24 };
            foreach (int e in exo)
            {
                MetricPrefix pr = MetricPrefix.FromExponent(e);
                Console.WriteLine("     {0}         {1}        10^{2}", pr.Prefix.PadRight(10), pr.Symbol.PadRight(10), pr.Exponent);
            }

            Console.ForegroundColor = ForegroundColor;
        }



        #region Console commands

        public static void StartConsole()
        {
            Console.BackgroundColor = BackgroundColor;
            Console.Clear();

            PrintCopyright();

            Console.WriteLine();
            Console.WriteLine("Type \"help\" for more information.");

            Console.ForegroundColor = ForegroundColor;
        }

        internal static void PrintCopyright()
        {
            Console.ForegroundColor = HelpColor;

            var lib_ver = (AssemblyFileVersionAttribute)Assembly.GetAssembly(typeof(QuantitySystem.QuantityDimension)).GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0];

            var qsc_ver = (AssemblyFileVersionAttribute)Assembly.GetAssembly(typeof(Qs.Qs)).GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0];

            var calc_ver = (AssemblyFileVersionAttribute)Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0];

            var symb_ver = (AssemblyFileVersionAttribute)Assembly.GetAssembly(typeof(SymbolicAlgebra.SymbolicVariable)).GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0];

            Console.WriteLine("Quantity System Framework  ver " + lib_ver.Version);
            Console.WriteLine("Quantity System DLR        ver " + qsc_ver.Version);
            Console.WriteLine("Symbolic Algebra Library   ver " + symb_ver.Version);
            Console.WriteLine("Quantity System Calculator ver " + calc_ver.Version);


            var qsc_cwr = (AssemblyCopyrightAttribute)Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0];
            Console.WriteLine(qsc_cwr.Copyright);
            Console.WriteLine();
            Console.WriteLine("Project Source: http://QuantitySystem.CodePlex.com");
            Console.WriteLine("Project Blog:   http://QuantitySystem.WordPress.com");
            Console.WriteLine();
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine("--                Ahmed Sadek Mohamed Tawfik                     --");
            Console.WriteLine("--              Ahmed.Sadek@LostParticles.net                    --");
            Console.WriteLine("-------------------------------------------------------------------");

            Console.ForegroundColor = ForegroundColor;

        }

        /// <summary>
        /// Help Command.
        /// </summary>
        internal static void PrintHelp()
        {
            Console.ForegroundColor = HelpColor;

            Console.WriteLine("    Type \"<unit>\" for information about unit");
            Console.WriteLine("           Example: <kn> for knot");
            Console.WriteLine();
            Console.WriteLine("       - var = number[<unit>] for making scalar value");
            Console.WriteLine("           Omit <unit> for dimensionless quantity");
            Console.WriteLine();

            Console.WriteLine("       - Scalar Quantity");
            Console.WriteLine("         Ordinary Real Number: 3, 3e4 or 2.3<L>");
            Console.WriteLine("         Symbolic Real Number: $x, $y<m>, or ${x^2-y^32}");
            Console.WriteLine("         Complex Number      : C{3 2} or C{2 1}<K>");
            Console.WriteLine("         Quaternion Number   : H{3 2 1 9} or H{8 4 2 1}<J>");
            Console.WriteLine();

            Console.WriteLine("       - Vector");
            Console.WriteLine("           vec = {3 4 5}");
            Console.WriteLine("           _|vec|_, or _||vec||_     to get magnitude");
            Console.WriteLine("           v1 * v2                   vector multiplication");
            Console.WriteLine("           v1 . v2                   Dot product.");
            Console.WriteLine("           v1 x v2                   Use x letter for cross product.");
            Console.WriteLine("           v1 (*) v2                 Use (*) letters for tensor product.");
            Console.WriteLine("           v = 5..20                 .. operator for genearting vector from .. to");
            Console.WriteLine();

            Console.WriteLine("       - Matrix");
            Console.WriteLine("           \" m = [3 4 5; 3 4 2; 9 3 2]");
            Console.WriteLine("           \" |m| \"             for determinant of square matrix");
            Console.WriteLine("           \" m1 * m2 \"         for ordinary matrix multiplication");
            Console.WriteLine("           \" m1 . m2 \"         for matrix element wise multiplication");
            Console.WriteLine("           \" m ^ 2 \"           for power with ordinary matrix multiplication");
            Console.WriteLine("           \" m ^. 2 \"          for power with element matrix multiplication");


            Console.WriteLine();
            Console.WriteLine("       - Tensor");
            Console.WriteLine("         <| Matrix | Matrix |> #3d Order");
            Console.WriteLine("         <| <| 5 3;2 1|> | <|3 2;1 9|> |> #4th Order");


            Console.WriteLine();
            Console.WriteLine("       - \"f(x,y,z) = x+y+z\"    to make a function.");
            Console.WriteLine("         \"f(u,v,w) = u/v*w\"    to make a function with the same name and different parameters.");
            Console.WriteLine("         \"r=f(v=10, w=3, u=3)\" to call specific function.");
            Console.WriteLine("         \"@f\"                  return the function body as a value.");

            Console.WriteLine();
            Console.WriteLine("       - Sequence:");
            Console.WriteLine("           Declaration");
            Console.WriteLine("           \"S[]     ..> 10; 20\"");
            Console.WriteLine("           \"S[n]    ..> 10; 20; n\"            sequence with index.");
            Console.WriteLine("           \"S[n](x) ..> 10; 20; n; x^n\"    sequence with index and parameter.");
            Console.WriteLine("           \"G[k=m->n](x) ..> n - (k*x)/m\"      sequence with start, end, and counter index plus parameter.");

            Console.WriteLine("           \"S[n:10](x) = n^2/x^(1/n)\"                        to set specific element of sequence.");
            Console.WriteLine("           \"S[10]+S[20]\"                                     to sum element 10 and 20");

            Console.WriteLine();
            Console.WriteLine("       - Sequence operators:");
            Console.WriteLine("             Series              [n ++ m]: S[0++40] (args)  to get series from 0 to 40.");
            Console.WriteLine("             Multiplication      [n ** m]: S[1**20) (args) to get products.");
            Console.WriteLine("             Average             [n !! m]: S[1!!20] sum from 1 to 20 and divide by (20 - 1)");
            Console.WriteLine("             Standard Deviation  [n !% m]: S[1!%20] sum from 1 to 20 and divide by (20 - 1)");
            Console.WriteLine("             Range               [n .. m]: S[0..20] Returns Vector or Matrix.");

            Console.WriteLine();
            Console.WriteLine("       - Calling parametererized series without specifying parameter return a function");
            Console.WriteLine("             \"P[n](x) ..> x^n");
            Console.WriteLine("             \"gf = P[2++5]  # returns x^2+x^3+x^4+x^5 as a function of _(x)");
            Console.WriteLine("             \"gv = P[2..5]  # returns vector x^2, x^3, x^4, x^5 as a function of _(x)");

            Console.WriteLine();
            Console.WriteLine("       - \"var = number[Quantity Name]\" ");
            Console.WriteLine("             to make a variable with default SI units of the quantity");
            Console.WriteLine();
            Console.WriteLine("       - \"variable name\" alone to show its information");
            Console.WriteLine();
            Console.WriteLine("       - \"<unit> to <unit>\" i.e. <m/s> to <kn> for conversion factor");
            Console.WriteLine();
            Console.WriteLine("       - \"List \" for list of variables");
            Console.WriteLine("         \"List [Quantities]\" for list of available Quantities");
            Console.WriteLine("         \"List [[Units] [Quantity Name]]\" for list of available Units ");
            Console.WriteLine("         \"List [Prefixes]\" list of available metric prefixes. ");
            Console.WriteLine();
            Console.WriteLine("       - \"New\" to clear all variables.");
            Console.WriteLine();
            Console.WriteLine("       - \"Run file.qs\" to execute external Qs commands into current session.");
            Console.WriteLine();
            Console.WriteLine("       - \"Cls\" to clear the screen.");
            Console.WriteLine();
            Console.WriteLine("       - \"Copyright\" for information about the product.");
            Console.WriteLine("       - \"CTRL+Z\" to terminate the console.");


            Console.ForegroundColor = ForegroundColor;
        }


        public static IEnumerable<string> GetVariablesKeys(Scope scope)
        {
          
            if (scope != null)
            {
                var varo = from item in ((ScopeStorage)scope.Storage).GetItems()
                           select item.Key;
                return varo;
            }

            throw new Exception("Where is the Scope");
                
        }


        public static object GetVariable(Scope scope, string varName)
        {
            if (scope != null)
            {
                object q;
                ((ScopeStorage)scope.Storage).TryGetValue(varName, true, out q);
                return q;
            }
            else
            {
                throw new NotImplementedException("you should be running in DLR");
            }
        }

        /// <summary>
        /// List Command
        /// </summary>
        internal static void ListVariables(Scope scope)
        {
            Console.WriteLine();
            Console.ForegroundColor = ValuesColor;

            foreach (string var in GetVariablesKeys(scope))
            {
                var v = GetVariable(scope, var);
                Console.WriteLine("    " + var + " = " + v.ToString());
                if (v is QsNamespace)
                {
                    QsNamespace ns = (QsNamespace)v;
                    foreach (string nsvar in ns.GetVariablesKeys())
                    {
                        Console.WriteLine("        " + nsvar + " = " + ns.GetValue(nsvar).ToString());
                    }                    
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = ForegroundColor;
            Console.WriteLine();
        }

        /// <summary>
        /// List Quantities Command
        /// </summary>
        internal static void ListQuantities()
        {
            Console.ForegroundColor = HelpColor;

            Console.WriteLine();
            List<string> quats = new List<string>();
            foreach (Type QType in QuantitySystem.QuantityDimension.CurrentQuantitiesDictionary.Values)
            {
                quats.Add ("    " + QType.Name.Substring(0, QType.Name.Length - 2).PadRight(30) + "    " + QuantitySystem.QuantityDimension.DimensionFrom(QType).ToString());
            }

            var qss = from q in quats orderby q select q;
            foreach (var qq in qss)
                Console.WriteLine(qq);

            Console.ForegroundColor = ForegroundColor;
        }


        private struct UnitInfo
        {
            public string uname;
            public string symbol;
            public string system;
            public string qtype;
        }

        /// <summary>
        /// List Units Command
        /// </summary>
        internal static void ListUnits(string quantity)
        {
            Console.ForegroundColor = HelpColor;

            var units = new List<UnitInfo>();

            foreach (Type utype in Unit.UnitTypes)
            {
                QuantitySystem.Attributes.UnitAttribute ua = Unit.GetUnitAttribute(utype);
                if (ua != null)
                {
                    string uname = utype.Name.PadRight(16);

                    string symbol = "<" + ua.Symbol + ">";
                    symbol = symbol.PadRight(10);

                    string system = utype.Namespace.Substring("QuantitySystem.Units".Length + 1).PadRight(18);

                    string qtype = ua.QuantityType.ToString().Substring(ua.QuantityType.Namespace.Length + 1).TrimEnd("`1[T]".ToCharArray());


                    if (string.IsNullOrEmpty(quantity))
                    {
                        UnitInfo ui = new UnitInfo
                        {
                            uname = uname,
                            symbol = symbol,
                            system = system,
                            qtype = qtype
                        };

                        units.Add(ui);
                    }
                    else
                    {
                        //print if only the unit is for this quantity
                        if (qtype.Equals(quantity, StringComparison.InvariantCultureIgnoreCase)) Console.WriteLine("    " + uname + " " + symbol + " " + system + qtype);
                    }
                }
            }

            var uts = from ut in units
                      orderby ut.qtype
                      select ("    " + ut.uname + " " + ut.symbol + " " + ut.system + ut.qtype);
                foreach (var ut in uts)
                {
                    
                    Console.WriteLine(ut);
                }
            Console.ForegroundColor = ForegroundColor;
        }

        #endregion

    }
