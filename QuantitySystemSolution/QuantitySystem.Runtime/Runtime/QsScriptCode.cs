using System;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;

namespace Qs.Runtime
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

            string code = SourceUnit.GetReader().ReadToEnd();
            
            //string code = QsCommandLine.LastLine;   //workaround because Host have something weird in SourceTextReader that don't work linux mono


            string[] lines = code.Split(Environment.NewLine.ToCharArray());

            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    QsEvaluator qs = new QsEvaluator();

                    qs.Scope = scope;

                    QsCommands.CheckCommand(line, qs);

                    if (!QsCommands.CommandProcessed)
                    {
                        qs.Evaluate(line);
                    }

                    QsCommands.CommandProcessed = false;
                }

            }

            return 0;
        }

    }
}
