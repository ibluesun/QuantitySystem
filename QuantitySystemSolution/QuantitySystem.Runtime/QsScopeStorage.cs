using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Runtime;

namespace Qs
{
    public interface IQsStorageProvider : IDisposable
    {
        IEnumerable<KeyValuePair<string, object>> GetItems();
        IEnumerable<string> GetKeys();
        IEnumerable<object> GetValues();

        bool HasValue(string variable);
        object GetValue(string variable);
        void SetValue(string variable, object value);
        bool TryGetValue(string variable, out object q);
        bool DeleteValue(string variable);
        void Clear();
    }

    /// <summary>
    /// Serves the storage for the scope.
    /// </summary>
    public class QsScopeStorage : IQsStorageProvider
    {

        Dictionary<string, object> _Storage = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<KeyValuePair<string, object>> GetItems()
        {
            return _Storage.AsEnumerable();
        }

        public IEnumerable<string> GetKeys()
        {
            return _Storage.Keys.AsEnumerable();
        }

        public IEnumerable<object> GetValues()
        {
            return _Storage.Values.AsEnumerable();
        }

        public bool HasValue(string variable)
        {
            return _Storage.ContainsKey(variable);
        }

        public object GetValue(string variable)
        {
            return _Storage[variable];
        }

        public void SetValue(string variable, object value)
        {
            _Storage[variable] =  value;
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
            return _Storage.TryGetValue(variable, out q);
        }

        public bool DeleteValue(string variable)
        {
            return _Storage.Remove(variable);
        }

        public void Clear()
        {
            _Storage.Clear();
        }

        public void Dispose()
        {
            // do nothing
        }
    }
}
