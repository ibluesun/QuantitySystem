using System;
using System.Collections.Generic;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting.Actions;

namespace Qs.Runtime
{
    public sealed class QsContext : LanguageContext
    {
        

        public QsContext(ScriptDomainManager manager, IDictionary<string, object> options)
            : base(manager)
        {

            this.Binder = new QsBinder(manager);

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
