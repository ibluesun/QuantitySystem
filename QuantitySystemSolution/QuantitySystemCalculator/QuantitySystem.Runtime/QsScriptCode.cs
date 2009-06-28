using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting;

namespace QuantitySystem.Runtime
{
    public class QsScriptCode : ScriptCode
    {

        string line; 

        public QsScriptCode(SourceUnit sourceUnit)
            : base(sourceUnit)
        {


            line = SourceUnit.GetCode();
            
        }



        public override object Run()
        {
            throw new NotImplementedException();
        }

        public override object Run(Microsoft.Scripting.Runtime.Scope scope)
        {

            QsEvaluator qev = ((QsContext)this.LanguageContext).QsEvaluator;

            qev.Evaluate(line);

            return 0;

            
        }

    }
}
