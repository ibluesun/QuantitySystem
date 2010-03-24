using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qs.Runtime
{
    /// <summary>
    /// Types of paramteters that QsFunction accept.
    /// </summary>
    public enum QsParamType
    {
        /// <summary>
        /// Raw parameter will enter as it is to the parameter.
        /// </summary>
        Raw,


        /// <summary>
        /// Text parameter.
        /// </summary>
        Text,

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
    public sealed class QsParamInfoAttribute : Attribute
    {
        public QsParamType ParameterType { get; set; }

        public QsParamInfoAttribute(QsParamType parameterType)
        {
            ParameterType = parameterType;
        }
    }
}
