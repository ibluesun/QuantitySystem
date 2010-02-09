using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qs.Types.Attributes
{
    /// <summary>
    /// Quantity System Function Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class QsFunctionAttribute : Attribute
    {
        public string FunctionName { get; private set; }
        public bool DefaultScopeFunction { get; set; }
        public QsFunctionAttribute(string functionName)
        {
            FunctionName = functionName;
            DefaultScopeFunction = false;
        }

    }
}
