using System;
using Qs.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Qs.Types
{
    /// <summary>
    /// Function that declared in Qs
    /// </summary>
    public partial class QsFunction : QsValue
    {

        /// <summary>
        /// if you give it x,y,x,x,y,y will return string of "x,y"
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string RemoveRedundantParameters(params string[] parameters)
        {
            Dictionary<string, bool> rp = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (var prm in parameters)
            {
                if (!rp.ContainsKey(prm))
                    rp[prm] = true;
            }

            string all = string.Empty;
            foreach (var k in rp.Keys)
                all += "," + k;

            return all.TrimStart(',');
        }

        public override QsValue Identity
        {
            get { throw new NotImplementedException(); }
        }

        private QsFunction FOperation(QsValue value ,string operation)
        {
            if (value is QsFunction)
            {
                QsFunction fn2 = (QsFunction)value;
                var fname = "_";
                var fbody = "" + this.FunctionBody + " " + operation + " " + fn2.FunctionBody + "";

                var fparam = this.ParametersNames.Union(fn2.ParametersNames).ToArray();
                var f = fname + "(" + RemoveRedundantParameters(fparam) + ") = " + fbody;
                return QsFunction.ParseFunction(QsEvaluator.CurrentEvaluator, f);
            }
            else if (value is QsScalar)
            {
                var fname = "_";
                var fbody = "" + this.FunctionBody + " " + operation + " " + ((QsScalar)value).Quantity.ToShortString();
                var fparam = this.ParametersNames;
                var f = fname + "(" + RemoveRedundantParameters(fparam) + ") = " + fbody;
                return QsFunction.ParseFunction(QsEvaluator.CurrentEvaluator, f);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override QsValue AddOperation(QsValue value)
        {
            return FOperation(value, "+");


        }

        public override QsValue SubtractOperation(QsValue value)
        {
            return FOperation(value, "-");

        }

        public override QsValue MultiplyOperation(QsValue value)
        {
            return FOperation(value, "*");
        }

        public override QsValue DivideOperation(QsValue value)
        {
            return FOperation(value, "/");
        }

        public override QsValue PowerOperation(QsValue value)
        {
            return FOperation(value, "^");
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
            return FOperation(value, " . ");
        }

        public override QsValue CrossProductOperation(QsValue value)
        {
            return FOperation(value, " x ");
        }

        public override QsValue NormOperation()
        {
            throw new NotImplementedException();
        }

        public override QsValue AbsOperation()
        {
            throw new NotImplementedException();
        }

    }
}