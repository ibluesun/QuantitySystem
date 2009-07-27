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

        public static string LastLine { get; set; }

        public override object Run(Scope scope)
        {
            CodeContext cc = new CodeContext(scope, this.LanguageContext);
            
            string code = string.Empty;
            try
            {
                code = SourceUnit.GetReader().ReadToEnd();
            }
            catch
            {
                code = LastLine;   //workaround because Host have something weird in SourceTextReader that don't work linux mono
            } 

            QsEvaluator qs = new QsEvaluator();

            qs.Scope = scope;

            string[] lines = code.Split(Environment.NewLine.ToCharArray());

            object ret=null;

            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    string[] cl = line.Trim().Split('#');  //split the comment and normal line.

                    if (!cl[0].StartsWith("#"))
                    {
                        ret = qs.Evaluate(cl[0].Trim());
                    }
                }

            }

            return ret;
        }

    }
}
