using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qs.Runtime
{
    public enum QsParamType
    {
        /// <summary>
        /// Raw parameter will enter as it is to the parameter.
        /// </summary>
        Raw,     
        /// <summary>
        /// Value parameter should be evaluated before going to the parameter.
        /// </summary>
        Value,
        /// <summary>
        /// Function parameter is a pointer to another function.
        /// </summary>
        Function
    }

    /// <summary>
    /// Function Parameter Information.
    /// </summary>
    public class QsParamInfo
    {
        public string Name { get; set; }
        public QsParamType Type { get; set; }
    }


    /// <summary>
    /// Function Parameter Information Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class QsParamInfoAttribute : Attribute
    {
        public string Name { get; private set; }
        
        public QsParamInfoAttribute(string name)
        {
            Name = name;
        }
    }
}
