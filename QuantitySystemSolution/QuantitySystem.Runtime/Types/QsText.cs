using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qs.Types
{
    public partial class QsText : QsValue
    {
        public string Text { get; set; }

        public QsText(string text)
        {
            Text = text.Replace("\\\"", "\"");
        }


        public override string ToString()
        {

            return  Text ;
        }


        #region QsValue Operations
        public override QsValue Identity
        {
            get { throw new NotImplementedException(); }
        }

        public override QsValue AddOperation(QsValue value)
        {
            
            QsText t = new QsText(Text + value.ToString());
            return t;
        }

        public override QsValue SubtractOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue MultiplyOperation(QsValue value)
        {
            throw new NotImplementedException();
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

        public override QsValue LeftShiftOperation(QsValue times)
        {
            throw new NotImplementedException();
        }

        public override QsValue RightShiftOperation(QsValue times)
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
            throw new NotImplementedException();
        }

        public override bool Inequality(QsValue value)
        {
            throw new NotImplementedException();
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

        #endregion
    }
}
