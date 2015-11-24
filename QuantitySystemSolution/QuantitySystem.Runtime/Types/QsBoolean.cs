using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Qs.Types
{
    public class QsBoolean : QsValue
    {
        public bool Value;

        static QsBoolean _QsTrue = new QsBoolean { Value = true };
        static QsBoolean _QsFalse = new QsBoolean { Value = false };

        public static QsBoolean True
        {
            get
            {
                return _QsTrue;
            }
        }

        public static  QsBoolean False
        {
            get
            {
                return _QsFalse;
            }
        }


        public static implicit operator QsBoolean(bool b)
        {
            return new QsBoolean { Value = b };
        }

        public static implicit operator bool(QsBoolean b)
        {
            return b.Value;
        }

        public override string ToShortString()
        {
            #if WINRT
                return Value.ToString();
            #else
                return Value.ToString(CultureInfo.InvariantCulture);
            #endif
        }

        public override string ToString()
        {
            #if WINRT
                            return Value.ToString();
            #else
                        return Value.ToString(CultureInfo.InvariantCulture);
            #endif
        }

        public override QsValue Identity
        {
            get { return True; }
        }

        public static QsBoolean operator +(QsBoolean a, QsBoolean b)
        {
            return new QsBoolean { Value = a.Value | b.Value };
        }

        public static QsBoolean operator -(QsBoolean a, QsBoolean b)
        {
            return new QsBoolean { Value = a.Value | !b.Value };
        }

        public static QsBoolean operator *(QsBoolean a, QsBoolean b)
        {
            return new QsBoolean { Value = a.Value & b.Value };
        }
        

        public override QsValue AddOperation(QsValue value)
        {
            return new QsBoolean { Value = this.Value | ((QsBoolean)value).Value };
        }

        public override QsValue SubtractOperation(QsValue value)
        {
            return new QsBoolean { Value = this.Value | !((QsBoolean)value).Value };
        }

        public override QsValue MultiplyOperation(QsValue value)
        {
            return new QsBoolean { Value = this.Value & ((QsBoolean)value).Value };
        }

        public override QsValue DivideOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue PowerOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue ModuloOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool LessThan(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool GreaterThan(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool LessThanOrEqual(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool GreaterThanOrEqual(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool Equality(QsValue value)
        {
            return this.Value == ((QsBoolean)value).Value;
        }

        public override bool Inequality(QsValue value)
        {
            return this.Value != ((QsBoolean)value).Value;
        }

        public override QsValue DotProductOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue CrossProductOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue TensorProductOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue NormOperation()
        {
            throw new NotImplementedException();
        }

        public override QsValue AbsOperation()
        {
            throw new NotImplementedException();
        }

        public override QsValue RightShiftOperation(QsValue times)
        {
            throw new NotImplementedException();
        }

        public override QsValue LeftShiftOperation(QsValue times)
        {
            throw new NotImplementedException();
        }

        public override QsValue GetIndexedItem(QsParameter[] indices)
        {
            throw new NotImplementedException();
        }

        public override void SetIndexedItem(QsParameter[] indices, QsValue value)
        {
            throw new NotImplementedException();
        }
    }
}
