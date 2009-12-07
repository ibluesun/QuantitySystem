
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting;
using System.IO;
using System.Reflection;
using Qs.RuntimeTypes;
using Microsoft.Scripting.Utils;
using Microsoft.Scripting.Runtime;
using System.Text.RegularExpressions;

namespace Qs.Runtime
{
    /// <summary>
    /// Employing the namespace concept in the Qs
    /// Namespace is in the form of xml namespaces.
    /// Namespace are dual functionality:
    ///    1) normal namespace declared in qs context while execution.
    ///    2) custom namespace from built-in or external classes during the program load.
    /// For example Math namespace is built-in namespace in the program but you can add 
    ///     another variables and functions for it during running.
    ///     
    /// This class is responsible to act as a provider for the two functionalities.
    /// </summary>
    public class QsNamespace
    {
        public string Name { get; private set; }

        private System.Type _NamespaceType;
        public System.Type NamespaceType
        {
            get
            {
                return _NamespaceType;
            }
        }

        public QsNamespace(string name)
        {
            Name = name;
            _NamespaceType = null;
        }

        public QsNamespace(System.Type namespaceType)
        {
            Name = null;
            _NamespaceType = namespaceType;
        }

        private Dictionary<string, object> Values = new Dictionary<string, object>(System.StringComparer.OrdinalIgnoreCase);

        public void SetValue(string name, object value)
        {
            if (char.IsLetter(name[0]))
            {
                if (_NamespaceType != null)
                {
                    var prop = _NamespaceType.GetProperty(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
                    if (prop != null)
                    {
                        prop.SetValue(null, value, null);
                    }
                    else
                    {
                        Values[name] = value;
                    }
                }
                else
                {
                    Values[name] = value;
                }
            }
            else
            {
                throw new QsSyntaxErrorException("Variable name must start with a letter.");
            }
        }


        /// <summary>
        /// It tries to get the value of the given key.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetValue(string name)
        {
            if (_NamespaceType != null)
            {
                if (QsFunction.IsItFunctionSymbolicName(name))
                {
                    if (HardCodedMethods == null) HardCodedMethods = GetQsNamespaceMethods(_NamespaceType);

                    if (HardCodedMethods.ContainsKey(name))
                    {
                        return HardCodedMethods[name];
                    }                    
                }
                else
                {
                    var prop = _NamespaceType.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
                    if (prop != null)
                        return prop.GetValue(null, null);

                }
            }

            // try in values hash

            object o;
            Values.TryGetValue(name, out o);

            if (o == null) throw new QsVariableNotFoundException("Variable '" + name + "' not found in '" + this.Name + "' namespace.");
            return o;        
        }


        public QsFunction GetQsFunctionFromTypeMethod(MethodInfo methodInfo, QsFunctionAttribute funcAttribute)
        {
            string qsNamespace = this._NamespaceType.Name;

            string qsFuncName = funcAttribute == null ? methodInfo.Name : funcAttribute.FunctionName;

            var miParameters = methodInfo.GetParameters();
            int qsFuncParamCount = miParameters.Length;


            QsFunction QsModFunc = new QsFunction("[Qs.Modules." + qsNamespace + "." + qsFuncName + "]", 
                funcAttribute == null ? true : funcAttribute.DefaultScopeFunction, true);
            QsModFunc.FunctionNamespace = qsNamespace;
            QsModFunc.FunctionName = qsFuncName;

            List<QsParamInfo> prms = new List<QsParamInfo>(miParameters.Length);

            foreach (var miParam in miParameters)
            {
                QsParamInfo prm = new QsParamInfo();

                //parameter name
                prm.Name = miParam.Name;

                var pis = miParam.GetCustomAttributes(typeof(QsParamInfoAttribute), true);

                if (pis.Length > 0)
                    prm.Type = ((QsParamInfoAttribute)pis[0]).ParameterType;
                else
                    prm.Type = QsParamType.Value;

                prms.Add(prm);
            }

            
            QsModFunc.Parameters = prms.ToArray();
            QsModFunc.FunctionBody += "(";
            StringBuilder sb = new StringBuilder();
            foreach (var p in prms) sb.Append(", " + p.Name);
            if (sb.Length > 0) QsModFunc.FunctionBody += sb.ToString().TrimStart(',', ' ');
            QsModFunc.FunctionBody += ")";

            #region Delegate creation section
            switch (qsFuncParamCount)
            {
                case 0:
                    QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                        typeof(Func<QsValue>),
                        methodInfo);
                    break;
                case 1:
                    QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsValue>),
                        methodInfo);
                    break;
                case 2:
                    QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsValue>),
                        methodInfo);
                    
                    break;
                case 3:
                    QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsValue>),
                        methodInfo);
                    break;
                case 4:
                    QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        methodInfo);
                    break;
                case 5:
                    QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        methodInfo);
                    break;
                case 6:
                    QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        methodInfo);
                    break;
                case 7:
                    QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        methodInfo);
                    break;
                case 8:
                    QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        methodInfo);
                    break;
                case 9:
                    QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        methodInfo);
                    break;
                case 10:
                    QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        methodInfo);
                    break;
                case 11:
                    QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        methodInfo);
                    break;
                case 12:
                    QsModFunc._FunctionDelegate = System.Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        methodInfo);
                    break;

                }
                #endregion

            return QsModFunc;

        }



        #region discover of items

        /// <summary>
        /// Cache the discovered QsNamespace inner class type members.
        /// </summary>
        private Dictionary<string, object> HardCodedMethods;

        /// <summary>
        /// Gets the members of the QsNamespace or (static class ) that hold the hard coded functions and properties.
        /// </summary>
        /// <param name="namespaceType"></param>
        /// <returns></returns>
        private Dictionary<string, object> GetQsNamespaceMethods(System.Type qsNamespaceType)
        {
            ContractUtils.RequiresNotNull(qsNamespaceType, "namespaceType");

            var methods = _NamespaceType.GetMethods(
             BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);


            Dictionary<string, object> CodedMembers = new Dictionary<string, object>(System.StringComparer.OrdinalIgnoreCase);

            foreach (var member in methods)
            {
                var method = ((MethodInfo)member);
                if (!method.IsSpecialName) //because properies getters are methods also :S
                {
                    ParameterInfo[] mps = method.GetParameters();
                    
                    
                    int paramCount = mps.Length;


                    var fAttributes = method.GetCustomAttributes(typeof(QsFunctionAttribute), false);

                    QsFunctionAttribute funcAttribute = null;
                    string method_name = string.Empty;
                    string methodTrueName = string.Empty;

                    if (fAttributes.Length > 0)
                    {
                        
                        funcAttribute = (QsFunctionAttribute)fAttributes[0];

                        // if the attribute were found treat the function as namedargument function that is not default function.
                        method_name = funcAttribute.FunctionName;

                        if (funcAttribute.DefaultScopeFunction == false)
                        {
                            // alter the name so it includes the parameters also
                            string[] ptext = (from p in mps select p.Name).ToArray();
                            methodTrueName = QsFunction.FormFunctionSymbolicName(method_name, ptext);
                        }
                        else
                        {
                            // don't use special parameter symbolic naming.
                            methodTrueName = QsFunction.FormFunctionSymbolicName(method_name, paramCount);
                        }

                        CodedMembers.Add(methodTrueName, GetQsFunctionFromTypeMethod(method, funcAttribute));

                    }
                    else
                    {
                        method_name = method.Name;
                        methodTrueName = QsFunction.FormFunctionSymbolicName(method_name, paramCount);

                        CodedMembers.Add(methodTrueName, GetQsFunctionFromTypeMethod(method, null));

                    }
                    

                }
            }

            return CodedMembers;
        }


        private IEnumerable<KeyValuePair<string, object>> GetQsNamespaceProperties(System.Type qsNamespaceType)
        {
            ContractUtils.RequiresNotNull(qsNamespaceType, "namespaceType");

            var properties = _NamespaceType.GetProperties(
             BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);


            List<KeyValuePair<string, object>> CodedMembers = new List<KeyValuePair<string, object>>();

            foreach (var prop in properties)
            {
                    CodedMembers.Add(new KeyValuePair<string, object>
                        (prop.Name, prop.GetValue(null,null)));
                
            }

            return CodedMembers;

        }

        public IEnumerable<KeyValuePair<string, object>> GetItems()
        {
            List<KeyValuePair<string, object>> items = new List<KeyValuePair<string, object>>();

            items.AddRange(Values);

            //also include functions of NamespaceType
            if (_NamespaceType != null)
            {
                // add the properties 
                items.AddRange(GetQsNamespaceProperties(_NamespaceType));

                if (HardCodedMethods == null)
                {
                    HardCodedMethods = GetQsNamespaceMethods(_NamespaceType);
                }
                items.AddRange(HardCodedMethods);
            }

            return items;
        }


        public IEnumerable<string> GetVariablesKeys()
        {
            var varo = from item in this.GetItems()
                       select item.Key;
            return varo;

        }

        #endregion

        #region Helpers for getting qsnampespaces

        /// <summary>
        /// The namespace is a static C# class under Qs.Modules.*
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        private static System.Type GetQsNameSpace(string qsNamespace)
        {
            string cls = "Qs.Modules." + qsNamespace;

            //try the current assembly

            System.Type ns = System.Type.GetType(cls, false, true);

            if (ns == null)
            {
                // try  another search in the Qs*.dll dlls

                DirectoryInfo di = new DirectoryInfo(System.Environment.CurrentDirectory + "/Modules");
                var files = di.GetFiles("Qs*.dll");
                foreach (var file in files)
                {
                    var a = Assembly.LoadFrom(file.FullName);
                    ns = a.GetType(cls, false, true);//+ ", " + file.Name.TrimEnd('.', 'd', 'l', 'l'));
                    if (ns != null) break;  //found the break and pop out from the loop.
                }
            }

            return ns;

        }



        /// <summary>
        /// try to get the name
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="moduleNamespace"></param>
        /// <returns></returns>
        public static QsNamespace GetNamespace(Scope scope, string moduleNamespace)
        {
            return GetNamespace(scope, moduleNamespace, false);
        }


        /// <summary>
        /// The primary  function that gets the namespace from the scope.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="moduleNamespace"></param>
        /// <param name="forceCreation">force creation of namespace in scope if dosen't exist before.</param>
        /// <returns></returns>
        public static QsNamespace GetNamespace(Scope scope, string moduleNamespace, bool forceCreation)
        {
            QsNamespace NameSpace = null;

            // try to get the namespace object from the scope
            //  then see if it represent a custom namespace also or not.
            // then return the object that hold the whole thing.
            if (scope != null)
            {

                if (((ScopeStorage)scope.Storage).HasValue(moduleNamespace, true))
                {
                    NameSpace = (QsNamespace)((ScopeStorage)scope.Storage).GetValue(moduleNamespace, true);
                }

                if (NameSpace == null)
                {
                    // no namespace in this scope
                    NameSpace = new QsNamespace(moduleNamespace);

                    // search if this namespace represent hardcoded namespace
                    System.Type nst = GetQsNameSpace(moduleNamespace);
                    NameSpace._NamespaceType = nst;

                    if (forceCreation | (nst != null))
                    {

                        ((ScopeStorage)scope.Storage).SetValue(moduleNamespace, true, NameSpace);
                    }
                }
            }


            return NameSpace;
        }
        #endregion


    }
}
