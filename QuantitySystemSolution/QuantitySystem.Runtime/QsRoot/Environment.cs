using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Types;
using Qs.Runtime;
using Qs;

namespace QsRoot
{
    /// <summary>
    /// System Environment but in unitized properties
    /// </summary>
    public static class Environment
    {
        public static QsValue TickCount
        {
            get
            {
                
                return System.Environment.TickCount.ToQuantity("ms").ToScalarValue();
            }
        }

        public static QsValue ProcessorCount
        {
            get
            {
                return System.Environment.ProcessorCount.ToQuantity().ToScalar();
            }
        }


#if !WINRT

        public static QsText CurrentDirectory
        {
            get
            {
                
                return new QsText(System.Environment.CurrentDirectory);
            }
        }

        public static QsText MachineName
        {
            get
            {
                
                return new QsText(System.Environment.MachineName);
            }
        }

        public static QsObject OsVersion
        {
            get
            {
                return  QsObject.CreateNativeObject( System.Environment.OSVersion);
            }
        }

        public static QsText SystemDirectory
        {
            get
            {
                return new QsText(System.Environment.SystemDirectory);
            }
        }

        public static QsText UserName
        {
            get
            {
                return new QsText(System.Environment.UserName);
            }
        }

#endif
    }

}