using System;
using Qs.Runtime;
using System.Collections.Generic;
using System.Linq;
using ParticleLexer;
using ParticleLexer.QsTokens;
using ParticleLexer.StandardTokens;
using Microsoft.Scripting.Ast;
using SymbolicAlgebra;

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
        private static string RemoveRedundantParameters(params string[] parameters)
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


        /// <summary>
        /// Join any set of given functions and returns the tokens of the new function along
        /// with the new function text.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="functionDeclaration"></param>
        /// <param name="functions"></param>
        /// <returns></returns>
        internal static Token JoinFunctionsArrayTokensWithOperation(string operation, out string functionDeclaration, params QsFunction[] functions)
        {
            var fname = "_";

            var fbody = "";
            List<string> fparamList = new List<string>();
            foreach (var f in functions)
            {
                fbody += " " + f.FunctionBody + " " + operation;
                fparamList.AddRange(f.ParametersNames);
            }
            fbody = fbody.TrimEnd(operation.ToCharArray());

            string[] fparam = fparamList.ToArray();

            var fNameParamPart = fname + "(" + RemoveRedundantParameters(fparam) + ") =";
            functionDeclaration = fNameParamPart + fbody;

            Token fTokens = new Token();

            // add first part tokens
            foreach (var t in QsFunction.TokenizeFunction(fNameParamPart))
            {
                fTokens.AppendSubToken(t);
            }

            for (int i = 0; i < functions.Length; i++ )
            {
                // prepare function body tokens
                foreach (var t in functions[i].FunctionBodyToken)
                {
                    fTokens.AppendSubToken(t);
                }

                if (i < functions.Length - 1) fTokens.AppendSubToken(Token.ParseText(operation.Trim())[0]);  //operation is only one charachter
            }


            return fTokens;
        }

        
        internal static Token JoinFunctionsBodiesTokensWithOperation(string operation, params QsFunction[] functions)
        {

            var fbody = "";
            foreach (var f in functions)
            {
                fbody += " " + f.FunctionBody + " " + operation;
            }

            fbody = fbody.TrimEnd(operation.ToCharArray());


            Token fTokens = new Token();

            for (int i = 0; i < functions.Length; i++)
            {
                // prepare function body tokens
                foreach (var t in functions[i].FunctionBodyToken)
                {
                    fTokens.AppendSubToken(t);
                }

                if (i < functions.Length - 1) fTokens.AppendSubToken(Token.ParseText(operation.Trim())[0]);  //operation is only one charachter
            }


            return fTokens;
        }


        /// <summary>
        /// Take the value and operation in text and return the current function operationed by value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        private QsFunction FOperation(QsValue value, string operation)
        {
            if (value is QsFunction)
            {
                QsFunction fn2 = (QsFunction)value;
                //if (operation == Operator.Plus)
                //{
                //    return SymbolicOperation(fn2, operation);
                //}
                //else
                {
                    string fDeclaration;

                    var fTokens = JoinFunctionsArrayTokensWithOperation(operation, out fDeclaration, this, fn2);
                    return QsFunction.ParseFunction(QsEvaluator.CurrentEvaluator, fDeclaration, fTokens);
                }
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


        internal static string FormFunctionHeadFromFunctions(string functionName, params QsFunction[] functions)
        {
            var fname = functionName;

            List<string> fparamList = new List<string>();
            foreach (var f in functions)
            {
                fparamList.AddRange(f.ParametersNames);
            }

            string[] fparam = fparamList.ToArray();

            var fNameParamPart = fname + "(" + RemoveRedundantParameters(fparam) + ")";

            return fNameParamPart;
        }

        /// <summary>
        /// Sums the current function with the parameter function
        /// symbolically.
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        private QsFunction SymbolicOperation(QsFunction function, string operation)
        {

            var fTokens = JoinFunctionsBodiesTokensWithOperation(operation, this, function);

            var sp = new SymbolicAlgebra.SymbolicParser();

            var SymbolicExpression = sp.ParseSymbols(fTokens);

            Expression<Func<SymbolicVariable>> cq = Expression.Lambda<Func<SymbolicVariable>>(SymbolicExpression);

            var fq = cq.Compile();
            var fs = fq().ToString();

            string funcHead = FormFunctionHeadFromFunctions("_", this, function);
            string wholeFunction = funcHead + " = " + fs;

            return QsFunction.ParseFunction(QsEvaluator.CurrentEvaluator, wholeFunction);
        }


        public override QsValue AddOperation(QsValue value)
        {
            return FOperation(value, Operator.Plus);
        }

        public override QsValue SubtractOperation(QsValue value)
        {
            return FOperation(value, Operator.Minus);

        }

        public override QsValue MultiplyOperation(QsValue value)
        {
            return FOperation(value, Operator.Multiply);
        }

        public override QsValue DivideOperation(QsValue value)
        {
            return FOperation(value, Operator.Divide);
        }

        public override QsValue PowerOperation(QsValue value)
        {
            return FOperation(value, Operator.Power);
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
            return FOperation(value, Operator.DotProduct);
        }

        public override QsValue CrossProductOperation(QsValue value)
        {
            return FOperation(value, Operator.CrossProduct);
        }

        public override QsValue TensorProductOperation(QsValue value)
        {
            return FOperation(value, Operator.TensorProduct);
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