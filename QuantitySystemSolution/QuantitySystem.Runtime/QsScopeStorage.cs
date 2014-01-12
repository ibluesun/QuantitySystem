using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Runtime;

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
            var r = QsEvaluator.CurrentEvaluator.GetExternalValue(variable);
            if (r == null) return Storage[variable];
            else return r;
        }

        public void SetValue(string variable, object value)
        {
            Storage[variable] =  value;
        }


        /// <summary>
        /// Trys to get a variable content by its name .. the function also calls an delegate function for 
        /// external variables if needed
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public bool TryGetValue(string variable, out object q)
        {
            //
            q = QsEvaluator.CurrentEvaluator.GetExternalValue(variable);

            if (q == null) return Storage.TryGetValue(variable, out q);
            else return true;
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
