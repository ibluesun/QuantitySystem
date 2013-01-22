using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qs
{
    public class QsScopeStorage
    {
        Dictionary<string, object> Storage = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<KeyValuePair<string, object>> GetItems()
        {
            return Storage.AsEnumerable();
        }

        public bool HasValue(string variable)
        {
            return Storage.ContainsKey(variable);
        }

        public  object GetValue(string variable)
        {
            return Storage[variable];
        }

        public void SetValue(string variable, object value)
        {
            Storage[variable] =  value;
        }

        public bool TryGetValue(string variable, out object q)
        {
            return Storage.TryGetValue(variable, out q);
        }

        public bool DeleteValue(string variable)
        {
            return Storage.Remove(variable);
        }

        public void Clear()
        {
            Storage.Clear();
        }
    }
}
