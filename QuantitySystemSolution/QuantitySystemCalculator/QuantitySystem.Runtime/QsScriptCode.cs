using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting.Hosting;
using System.Text.RegularExpressions;
using QuantitySystem.Units;
using QuantitySystem.Quantities.BaseQuantities;
using Microsoft.Linq.Expressions;

namespace QuantitySystem.Runtime
{
    public class QsScriptCode : ScriptCode
    {


        public QsScriptCode(SourceUnit sourceUnit)
            : base(sourceUnit)
        {


        }



        public override object Run()
        {
            return Run(new Scope());
        }

        public override object Run(Scope scope)
        {

            //string code = SourceUnit.GetReader().ReadToEnd();
            
            string code = QsCommandLine.LastLine;   //workaround because Host have something weird in SourceTextReader that don't work linux mono


            string[] lines = code.Split(Environment.NewLine.ToCharArray());

            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    QsEvaluator qs = new QsEvaluator();

                    qs.Scope = scope;

                    CheckCommand(line, qs);

                    if (!CommandProcessed)
                    {
                        qs.Evaluate(line);
                    }

                    CommandProcessed = false;
                }
            }

            return 0;
        }


        static bool CommandProcessed;


        /// <summary>
        /// Console Commands.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static bool CheckCommand(string command, QsEvaluator qse)
        {
            string[] commands = command.ToLower().Split(' ');

            if (commands[0] == "quit") return false;

            if (commands[0] == "exit")
            {
                Console.WriteLine("Press CTRL+Z to exit");
                CommandProcessed = true;
            }

            if (commands[0] == "help")
            {
                QsHost.PrintHelp();
                CommandProcessed = true;
            }

            if (commands[0] == "list")
            {
                string param = string.Empty;

                if (commands.Length < 2)
                {
                    QsHost.ListVariables(qse);
                }
                else
                {
                    if (commands[1] == "quantities")
                        QsHost.ListQuantities();

                    if (commands[1] == "units")
                    {
                        if (commands.Length < 3)
                        {
                            QsHost.ListUnits(string.Empty);
                        }
                        else
                        {
                            QsHost.ListUnits(commands[2]);
                        }
                    }
                }

                CommandProcessed = true;
            }

            if (commands[0] == "new")
            {
                qse.New();
                CommandProcessed = true;
            }

            if (commands[0] == "cls")
            {
                Console.Clear();
                CommandProcessed = true;
            }

            if (commands[0] == "copyright")
            {
                QsHost.PrintCopyright();
                CommandProcessed = true;
            }



            return true;

        }

    }
}
