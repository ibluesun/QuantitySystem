using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Types;

namespace QsRoot
{
    public static class Quantity
    {
        /// <summary>
        /// Dimension of scalar quantities
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static QsValue Dim(QsParameter value)
        {
            
            QsScalar s = value.QsNativeValue as QsScalar;
            if (s != null)
            {
                return new QsText( s.Unit.UnitDimension.ToString());
            }


            return new QsText("Works on scalar quantities");
        }
    }
}
