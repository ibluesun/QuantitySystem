using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting;

namespace Qs.Runtime
{

    public partial class QsFunction
    {



        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="nameSpace"></param>
        /// <param name="parametersCount">filter functions by number of parameters</param>
        /// <param name="parametersNames"></param>
        /// <returns></returns>
        public static QsFunction[] FindFunctionByParameters(
            Scope scope,
            string qsNamespace,
            string functionName,
            int parametersCount,
            params string[] parametersNames)
        {

            ScopeStorage storage = (ScopeStorage)scope.Storage;

            IEnumerable<KeyValuePair<string, object>> Items = null;

            if (!string.IsNullOrEmpty(qsNamespace))
            {
                var ns = QsNamespace.GetNamespace(scope,qsNamespace);
                Items = ns.GetItems();
            }
            else
            {
                Items = storage.GetItems();
            }

            var func_Pass1 = from item in Items
                             where item.Value is QsFunction
                             select (QsFunction)item.Value;

            var func_Pass2 = from func in func_Pass1
                             where func.ContainsParameters(parametersNames) && func.Parameters.Length == parametersCount
                             select func;

            var func_Pass3 = from fc in func_Pass2
                             where fc.FunctionName.Equals(functionName, StringComparison.OrdinalIgnoreCase)
                             select fc;


            return func_Pass3.ToArray();
        }



        /// <summary>
        /// Get the exact function with given parameters names.
        /// by comparing the names and the length of parameters.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="parametersNames"></param>
        /// <returns></returns>
        public static QsFunction GetExactFunctionWithParameters(
            Scope scope,
            string nameSpace,
            string functionName,
            params string[] parametersNames)
        {
            var funcs = FindFunctionByParameters(scope, nameSpace, functionName, parametersNames.Length, parametersNames);

            foreach (var func in funcs)
            {
                //double check parameters and their length to get the exact function.
                if (func.Parameters.Length == parametersNames.Length)
                    if (func.ContainsParameters(parametersNames)) return func;
            }

            return null;
        }


        /// <summary>
        /// Get the default function 
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="nameSpace"></param>
        /// <param name="functionName"></param>
        /// <param name="parameterCount"></param>
        /// <returns></returns>
        public static QsFunction GetDefaultFunction(
            Scope scope,
            string nameSpace,
            string functionName,
            int parametersCount)
        {

            string functionRealName = QsFunction.FormFunctionSymbolicName(functionName, parametersCount);

            QsFunction func = QsFunction.GetFunction(scope, nameSpace, functionRealName);

            return func;


        }
    }




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
