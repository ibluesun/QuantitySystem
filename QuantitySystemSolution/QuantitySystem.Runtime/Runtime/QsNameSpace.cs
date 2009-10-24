using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting;

namespace Qs.Runtime
{
    /// <summary>
    /// Employing the namespace concept in the Qs
    /// Namespace is in the form of xml namespaces.
    /// it hold all object types of Qs
    /// </summary>
    public class QsNameSpace
    {
        public QsNameSpace(string name)
        {
            Name = name;
        }

        public string Name { get; set; }


        private Dictionary<SymbolId, object> Values = new Dictionary<SymbolId, object>();

        public void SetName(string name, object value)
        {
            Values[SymbolTable.StringToCaseInsensitiveId(name)] = value;
        }

        public object GetName(string name)
        {
            object o;
            Values.TryGetValue(SymbolTable.StringToCaseInsensitiveId(name),out o);
            return o;
        }

        public T GetName<T>(string name)
        {
            return (T)GetName(name);
        }



    }
}
