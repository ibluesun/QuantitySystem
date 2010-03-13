using System;
using Qs.Runtime;
using System.Collections.Generic;
using System.Linq;
using ParticleLexer;
using ParticleConsole.QsTokens;

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

        internal static Token JoinFunctionTokensWithOperation(string operation, out string functionDeclaration, QsFunction functionOne, QsFunction functionTwo)
        {
            var fname = "_";
            var fbody = "" + functionOne.FunctionBody + " " + operation + " " + functionTwo.FunctionBody + "";


            var fparam = functionOne.ParametersNames.Union(functionTwo.ParametersNames).ToArray();
            var fNameParamPart = fname + "(" + RemoveRedundantParameters(fparam) + ") = ";
            functionDeclaration = fNameParamPart + fbody;

            Token fTokens = new Token();

            // add first part tokens
            foreach (var t in QsFunction.TokenizeFunction(fNameParamPart))
            {
                fTokens.AppendSubToken(t);
            }

            // prepare function body tokens
            foreach (var t in functionOne.FunctionBodyToken)
            {
                fTokens.AppendSubToken(t);
            }
            fTokens.AppendSubToken(Token.ParseText(operation.Trim())[0]);   //operation is only one charachter
            
            foreach (var t in functionTwo.FunctionBodyToken)
            {
                fTokens.AppendSubToken(t);
            }

            return fTokens;
        }

        private QsFunction FOperation(QsValue value ,string operation)
        {
            if (value is QsFunction)
            {
                QsFunction fn2 = (QsFunction)value;

                string fDeclaration;

                var fTokens = JoinFunctionsArrayTokensWithOperation(operation, out fDeclaration, this, fn2);

                return QsFunction.ParseFunction(QsEvaluator.CurrentEvaluator, fDeclaration, fTokens);
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

        public override QsValue TensorProductOperation(QsValue value)
        {
            return FOperation(value, "(x)");
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