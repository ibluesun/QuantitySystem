using System;
using System.Linq.Expressions;
using Microsoft.Scripting.Actions;
using Microsoft.Scripting.Ast;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting.Utils;

namespace Qs.Runtime
{
    public class QsBinder : DefaultBinder
    {
        

        public override Microsoft.Scripting.Actions.Calls.Candidate PreferConvert(Type t1, Type t2)
        {
            throw new NotImplementedException();
        }
        public override bool CanConvertFrom(Type fromType, Type toType, bool toNotNullable, Microsoft.Scripting.Actions.Calls.NarrowingLevel level)
        {
            throw new NotImplementedException();
        }


        public override Expression ConvertExpression(Expression expr, Type toType, ConversionResultKind kind, Microsoft.Scripting.Actions.Calls.OverloadResolverFactory resolverFactory)
        {
            ContractUtils.RequiresNotNull(expr, "expr");
            ContractUtils.RequiresNotNull(toType, "toType");

            Type exprType = expr.Type;

            if (toType == typeof(object))
            {
                if (exprType.IsValueType)
                {
                    return Utils.Convert(expr, toType);
                }
                else
                {
                    return expr;
                }
            }

            return base.ConvertExpression(expr, toType, kind, resolverFactory);
        }




        //protected override void MakeRule(OldDynamicAction action, object[] args, RuleBuilder rule)
        //{

        //    //at last the two lines didn't show an error when console finished
        //    //  lool those two lines were try and error :D

        //    rule.AddTest(Expression.Constant(true));

        //    rule.Target = rule.MakeReturn(this, Expression.Constant(0));

        //}


    }
}
