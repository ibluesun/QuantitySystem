using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qs.Types
{
    /// <summary>
    /// The class will hold intermediate operations that could be applied to function
    /// example  @ alone will do nothing
    /// @|$x  is deriving function operator over x symbol [this will store this operation inside this class]
    /// 
    /// </summary>
    public class QsFunctionOperation : QsOperation
    {
        /*
         * The function will store the operations in some cases and release those operations when 
         * applied to a function.
         * @|$x will store differentiate to x
         * 
         */

        private struct InnerOperation
        {
            public string Operation;
            public QsValue value;
        }

        private List<InnerOperation> operations = new List<InnerOperation>();

        public override object Clone()
        {
            QsFunctionOperation fo = new QsFunctionOperation();
            fo.operations.AddRange(operations);
            return fo;
        }

        /// <summary>
        /// @|$value  will store this operation 
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Function Operation</returns>
        public override QsValue DifferentiateOperation(QsValue value)
        {
            var sc = value as QsScalar;
            if (!Object.ReferenceEquals(sc , null))
            {
                if (!Object.ReferenceEquals(sc.SymbolicQuantity, null))
                {
                    var a = sc;

                    operations.Add(new InnerOperation { Operation = Operator.Differentiate, value = a });

                    return new QsScalar(ScalarTypes.QsOperation) { Operation = (QsOperation)this.Clone() };
                }
            }
            throw new NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue MultiplyOperation(QsValue value)
        {
            var sc = value as QsScalar;
            if (!Object.ReferenceEquals(sc, null))
            {
                if (sc.ScalarType == ScalarTypes.FunctionQuantity)
                {
                    if (!Object.ReferenceEquals(sc.FunctionQuantity, null))
                    {
                        var f = (QsFunction)sc.FunctionQuantity.Value;

                        foreach (var op in operations)
                        {
                            switch (op.Operation)
                            {
                                case Operator.Differentiate:
                                    f = (QsFunction)f.DifferentiateOperation(op.value);
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                        }

                        return f.ToQuantity().ToScalar();
                    }
                }
                else if (sc.ScalarType == ScalarTypes.SymbolicQuantity)
                {
                    var f = sc.SymbolicQuantity.Value;

                    foreach (var op in operations)
                    {
                        switch (op.Operation)
                        {
                            case Operator.Differentiate:
                                {
                                    var dsv = ((QsScalar)op.value).SymbolicQuantity.Value;
                                    int times = (int)dsv.SymbolPower;
                                    while (times > 0)
                                    {
                                        f = f.Differentiate(dsv.Symbol);
                                        times--;
                                    }
                                }
                                
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }

                    return f.ToQuantity().ToScalar();
                }
                else
                    throw new QsException("Can't multiply current operation for this scalar " + sc.ScalarType.ToString());
            }

            throw new NotImplementedException();
        }


        public override string ToString()
        {
            string g = "@";
            foreach (var o in operations)
            {
                g += o.Operation + o.value.ToValueString();
            }
            return g;

        }

        public override string ToShortString()
        {
            return ToString();
        }
    }
}
