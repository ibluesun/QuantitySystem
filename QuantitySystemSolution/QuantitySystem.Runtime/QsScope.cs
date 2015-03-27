using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Linq.Expressions;

namespace Qs
{



    /// <summary>
    /// 
    /// </summary>
    public class QsScope : IDynamicMetaObjectProvider
    {
        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            throw new NotImplementedException("QsScope doesn't implement GetMetaObject function");
        }

        Dictionary<string, IQsStorageProvider> StorageProviders = new Dictionary<string, IQsStorageProvider>(StringComparer.OrdinalIgnoreCase);
        public QsScope()
        {
            StorageProviders["PrimaryStorage"] = new QsScopeStorage();
        }


        /// <summary>
        /// Register new storage that is implemented from IQsStorageProvider
        /// </summary>
        /// <param name="storageProviderName"></param>
        /// <param name="storage"></param>
        /// <returns></returns>
        public int RegisterScopeStorage(string storageProviderName, IQsStorageProvider storage)
        {
            StorageProviders.Add(storageProviderName, storage);
            return StorageProviders.Count;
        }

        public void UnRegisterScopeStorage(string storageProviderName)
        {
            var ss = StorageProviders[storageProviderName];
            ss.Dispose();
            StorageProviders.Remove(storageProviderName);
        }

        public void ReplacePrimaryScopeStorage(IQsStorageProvider storage)
        {
            StorageProviders["PrimaryStorage"] = storage;
        }


        /// <summary>
        /// Returns all the items in the registered storages
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, object>> GetItems()
        {
            List<KeyValuePair<string, object>> all = new List<KeyValuePair<string, object>>();
            foreach (var v in StorageProviders.Values)
            {
                all.AddRange(v.GetItems());
            }

            return all.AsEnumerable();
        }

        public IEnumerable<string> GetKeys()
        {
            List<string> all = new List<string>();
            foreach (var v in StorageProviders.Values)
            {
                all.AddRange(v.GetKeys());
            }

            return all.AsEnumerable();
        }

        public IEnumerable<object> GetValues()
        {
            List<object> all = new List<object>();
            foreach (var v in StorageProviders.Values)
            {
                all.AddRange(v.GetValues());
            }

            return all.AsEnumerable();
        }

        public bool HasValue(string variable)
        {
            foreach (var v in StorageProviders)
            {
                if (v.Value.HasValue(variable))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Getting the value of the last added provider as  LIFO   or stack
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public object GetValue(string variable)
        {
            for (int i = StorageProviders.Count; i > 0; i--)
            {
                var sprov = StorageProviders.Values.ElementAt(i - 1);

                if (sprov.HasValue(variable))
                    return sprov.GetValue(variable);
            }

            throw new QsException("variable not found in any storage provider");
        }

        public void SetValue(string variable, object value)
        {
            foreach (var v in StorageProviders.Values)
            {
                v.SetValue(variable, value);
            }
        }

        public bool TryGetValue(string variable, out object q)
        {
            bool result = false;

            for (int i = StorageProviders.Count; i > 0; i--)
            {
                var sprov = StorageProviders.Values.ElementAt(i - 1);

                result = result | sprov.TryGetValue(variable, out q);

                if (result) return true;
            }

            q = null;
            return false;
        }


        public bool DeleteValue(string variable)
        {
            bool deleted = false;

            foreach (var v in StorageProviders.Values)
            {
                if (v.HasValue(variable))
                {
                    v.DeleteValue(variable);
                    deleted = true;
                }
            }

            return deleted;
        }

        public void Clear()
        {
            foreach (var v in StorageProviders.Values)
                v.Clear();
        }
    }
}
