using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;
using Qs.Types;

namespace Qs.Runtime
{
    public sealed class QsContext : LanguageContext
    {
        
        QsBinder _binder;

        public QsContext(ScriptDomainManager manager, IDictionary<string, object> options)
            : base(manager)
        {

            _binder = new QsBinder();  // I have defined end statement to close the execution of the program.
                                              // the end statement will return null which in return the execution of current program will end.
                                              // always write end in the final line of your qs program.
           
        }



        public override ConvertBinder CreateConvertBinder(Type toType, bool? explicitCast)
        {

            
            if (toType == typeof(int))
                return new QsIntegerConvertBinder();

            else
                return base.CreateConvertBinder(toType, explicitCast);

        }



        public override ScriptCode CompileSourceCode(SourceUnit sourceUnit, CompilerOptions options, ErrorSink errorSink)
        {
            
            var sc = new QsScriptCode(sourceUnit);

            return sc;
        }


        public override Version LanguageVersion
        {
            get
            {
                return new Version(1, 2, 3);
            }
        }
    }





    public sealed class QsIntegerConvertBinder : ConvertBinder
    {
        public QsIntegerConvertBinder() : base(typeof(int), false) { }

        public override DynamicMetaObject FallbackConvert(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
        {
            if (target.Value is QsValue)
            {
                QsValue v = (QsValue)target.Value;
                int rvalue = 0;

                if (v is QsScalar) rvalue = (int)((QsScalar)v).NumericalQuantity.Value;

                if (v is QsVector) rvalue = (int)((QsVector)v)[0].NumericalQuantity.Value;

                if (v is QsMatrix) rvalue = (int)((QsMatrix)v)[0, 0].NumericalQuantity.Value;

                return target.Clone(Expression.Constant(rvalue, typeof(int)));
            }
            else
            {
                return target.Clone(Expression.Constant(0, typeof(int)));
            }
        }
    }
}
