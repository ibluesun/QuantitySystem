using System;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;
using ParticleLexer;
using System.Text;

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
                    
                    //I want to exclude # if it was between parentthesis.
                    //  oo(ferwe#kd adflk ) # 

                    // find the # char which is the comment.
                    int pc=0;

                    StringBuilder sb = new StringBuilder();
                    foreach (char c in line)
                    {
                        if (c == '(') pc++;
                        if (c == '#')
                        {
                            if (pc == 0)
                            {
                                //found the comment 
                                // break
                                break;
                            }
                        }

                        if (c == ')') pc--;

                        sb.Append(c);
                    }

                    string l2 = sb.ToString();

                    if (!l2.Trim().StartsWith("#"))
                    {
                        ret = qs.Evaluate(l2.Trim());
                    }
                }

            }

            return ret;
        }

    }
}
