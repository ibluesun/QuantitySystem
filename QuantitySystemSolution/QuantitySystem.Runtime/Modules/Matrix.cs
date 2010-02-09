using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Types;
using Qs.Runtime;

namespace Qs.Modules
{
    public static class Matrix
    {

        public static QsValue Transpose(QsParameter matrix)
        {
            if (matrix.Quantity is QsMatrix)
            {
                return ((QsMatrix)matrix.Quantity).Transpose();
            }
            else
                throw new QsInvalidInputException("Expected matrix input");
        }

    }
}
