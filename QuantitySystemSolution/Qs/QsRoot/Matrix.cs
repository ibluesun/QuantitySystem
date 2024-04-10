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

        public static QsMatrix Identity(int size)
        {

            return QsMatrix.MakeIdentity(size);
        }
        public static QsMatrix Random(int size)
        {

            return QsMatrix.Random(size);
        }

        public static QsMatrix Random(int n, int m)
        {

            return QsMatrix.Random(n, m);
        }

        public static QsValue Transpose(QsParameter matrix)
        {
            if (matrix.QsNativeValue is QsMatrix)
            {
                return ((QsMatrix)matrix.QsNativeValue).Transpose();
            }
            else
                throw new QsInvalidInputException("Expected matrix input");
        }

        public static QsValue Determinant(QsParameter matrix)
        {
            if (matrix.QsNativeValue is QsMatrix)
            {
                return QsMatrix.Determinant(((QsMatrix)matrix.QsNativeValue));
            }
            else
                throw new QsInvalidInputException("Expected matrix input");
        }


    }

}