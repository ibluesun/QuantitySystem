using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Scripting.Actions;
using Microsoft.Scripting.Runtime;
using Microsoft.Linq.Expressions;

namespace Qs.Runtime
{
    public class QsBinder:ActionBinder
    {


        public QsBinder(ScriptDomainManager manager)
            : base(manager)
        {
        }

        public override Microsoft.Scripting.Actions.Calls.Candidate PreferConvert(Type t1, Type t2)
        {
            throw new NotImplementedException();
        }
        public override bool CanConvertFrom(Type fromType, Type toType, bool toNotNullable, Microsoft.Scripting.Actions.Calls.NarrowingLevel level)
        {
            throw new NotImplementedException();
        }
        protected override void MakeRule(OldDynamicAction action, object[] args, RuleBuilder rule)
        {

            //at last the two lines didn't show an error when console finished
            //  lool those two lines were try and error :D

            rule.AddTest(Expression.Constant(true));
            
            rule.Target = rule.MakeReturn(this, Expression.Constant(0));
            
            
            
        }
    }
}
