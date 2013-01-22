using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Linq.Expressions;

namespace Qs
{
    public class QsScope : IDynamicMetaObjectProvider
    {
        QsScopeStorage _Storage = new QsScopeStorage();

        public QsScopeStorage Storage 
        { 
            get
            {
                return _Storage;
            }
        }

        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            throw new NotImplementedException("QsScope doesn't implement GetMetaObject function");
        }
    }
}
