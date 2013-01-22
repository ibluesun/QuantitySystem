using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qs.Types
{
    /// <summary>
    /// QsOperation Type is a class that express special holders for certain known mathematical operation
    ///     how to describe it is something that should be illustrated
    ///     it is indeed tough to describe it 
    ///     it is a value to store operation :) thats it
    ///     which means the operation will take place later on the program flow
    ///     consider @|$x  which means Differentiate for x symbol or (operation that says I will differentiate the function later)
    ///     @  contains the operation   | 
    /// </summary>
    public class QsOperation : QsValue
    {

        public override QsValue DifferentiateOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue MultiplyOperation(QsValue value)
        {
            throw new NotImplementedException();
        }
        
        public override string ToShortString()
        {
            return "QsOp";
        }

        #region ICloneable Members

        public virtual object Clone()
        {

            throw new QsException("Qs Base Operation Class cannot be cloned, It only occure in sub classes");
        }

        #endregion   
    
        public override QsValue Identity
        {
            get { throw new NotImplementedException(); }
        }

        public override QsValue AddOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue SubtractOperation(QsValue value)
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
