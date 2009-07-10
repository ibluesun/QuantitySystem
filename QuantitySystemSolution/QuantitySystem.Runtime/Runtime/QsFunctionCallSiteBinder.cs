using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Runtime.CompilerServices;
using Microsoft.Linq.Expressions;

namespace Qs.Runtime
{
    public class QsFunctionCallSiteBinder:CallSiteBinder
    {
        public Expression ProcExpression;

        public QsFunctionCallSiteBinder(Expression procExpression)
        {
            this.ProcExpression = procExpression;
        }
        public override Microsoft.Linq.Expressions.Expression Bind(object[] args, System.Collections.ObjectModel.ReadOnlyCollection<Microsoft.Linq.Expressions.ParameterExpression> parameters, Microsoft.Linq.Expressions.LabelTarget returnLabel)
        {
            return Expression.Return(returnLabel, ProcExpression);

        }
    }
}
