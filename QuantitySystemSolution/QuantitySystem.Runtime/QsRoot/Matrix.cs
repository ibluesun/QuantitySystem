using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Types;
using Qs.Runtime;
using Qs;

namespace QsRoot
{
    public static class Matrix
    {

        public static QsValue Transpose(QsParameter matrix)
        {
            if (matrix.QsNativeValue is QsMatrix)
            {
                return ((QsMatrix)matrix.QsNativeValue).Transpose();
            }
            else
                throw new QsInvalidInputException("Expected matrix input");
        }

    }

}