using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting;
using Microsoft.Linq.Expressions;

namespace QuantitySystem.Runtime
{
    public sealed class QsContext : LanguageContext
    {
        

        public QsContext(ScriptDomainManager manager, IDictionary<string, object> options)
            : base(manager)
        {

            
        }


        protected override ScriptCode CompileSourceCode(SourceUnit sourceUnit, CompilerOptions options, ErrorSink errorSink)
        {
            
            
            var sc = new QsScriptCode(sourceUnit);

            return sc;
        }


        public override Version LanguageVersion
        {
            get
            {
                return new Version(0, 1);
            }
        }
    }

}
