using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs;
using Qs.Types;

namespace QsTestProject
{

    /// <summary>
    /// Act as extra storage for circle objects [[ this is for testing ]]
    /// </summary>
    class CircleStorage : IQsStorageProvider
    {
        Dictionary<string, object> values = new Dictionary<string, object>();

        public IEnumerable<KeyValuePair<string, object>> GetItems()
        {
            return values.AsEnumerable();
        }

        public IEnumerable<string> GetKeys()
        {
            return values.Keys.AsEnumerable();
        }

        public IEnumerable<object> GetValues()
        {
            return values.Values.AsEnumerable();
        }

        public bool HasValue(string variable)
        {
            if (variable.StartsWith("_"))
                return true; // as I am creating the variable always
            else return false;
        }

        public object GetValue(string variable)
        {
            if (variable.StartsWith("_"))
            {
                if (values.ContainsKey(variable)) return values[variable];
                else
                {
                    var r = QsObject.CreateNativeObject(new Circle(variable));
                    values.Add(variable, r);

                    return values[variable];
                }
            }

            return null;
        }

        public void SetValue(string variable, object value)
        {
            
        }

        public bool TryGetValue(string variable, out object q)
        {
            if (variable.StartsWith("_"))
            {
                if (values.ContainsKey(variable)) q = values[variable];
                else
                {
                    var r = QsObject.CreateNativeObject(new Circle(variable));
                    values.Add(variable, r);

                    q = r;
                }
                return true;
            }
            else
            {
                q = null;
                return false;
            }
        }

        public bool DeleteValue(string variable)
        {
            if (variable.StartsWith("_"))
            {
                return values.Remove(variable);
            }
            return false;
        }

        public void Clear()
        {
            values.Clear();
        }

        public void Dispose()
        {
            // do nothing
        }
    }
}
