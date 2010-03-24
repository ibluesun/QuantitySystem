using System;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;
using ParticleLexer;
using System.Text;
using System.Linq;

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


            
            string code = string.Empty;
            try
            {
                code = SourceUnit.GetReader().ReadToEnd();
            }
            catch
            {
                code = LastLine;   //workaround because Host have something weird in SourceTextReader that don't work linux mono
            } 

            QsEvaluator qs = QsEvaluator.CurrentEvaluator;

            qs.Scope = scope;

            string[] lines = code.Split(Environment.NewLine.ToCharArray());

            object ret=null;

            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (!line.StartsWith("#"))
                    {

                        //I want to exclude # if it was between parentthesis.
                        //  oo(ferwe#kd adflk ) # 

                        // first pass (from left to right): find the # char which is the comment.
                        int pc = 0;

                        StringBuilder sb = new StringBuilder();
                        foreach (char c in line)
                        {
                            if (c == '(') pc++;

                            // is it a comment charachter.
                            if (c == '#')
                            {
                                if (pc == 0)
                                {
                                    // found the comment 
                                    //  break
                                    break;
                                }
                            }

                            if (c == ')') pc--;

                            sb.Append(c);
                        }

                        string l2 = sb.ToString().Trim();  // text without comment.

                        //check the last charachter
                        if (l2.EndsWith(";"))
                        {
                            //trim the ';' and silent evaluate the expression.
                            ret = qs.SilentEvaluate(l2.Trim(';'));
                        }
                        else
                        {
                            ret = qs.Evaluate(l2);
                        }


                    }
                }
            }

            return ret;
        }

    }
}
