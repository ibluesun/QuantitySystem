using System;
using System.Collections.Generic;
using System.Text;

namespace Qs.Types.Attributes
{

    /// <summary>
    /// QsValue property that can be dynamically called for the QsValue Type from execute operator  --  not implemented yet
    /// <see cref="QsValue"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class QsValuePropertyAttribute : Attribute
    {
        public string[] PropertyNames { get; private set; }
        

        public QsValuePropertyAttribute(params string[] propertyNames)
        {
            PropertyNames = propertyNames;
            
        }


    }
}
