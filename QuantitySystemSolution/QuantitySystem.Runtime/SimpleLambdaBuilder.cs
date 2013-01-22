using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Qs
{
    /// <summary>
    /// Because Lambda Expression requires that everything should be prepared this class helps in making the expression later
    /// after completing all required fields
    /// </summary>
    class SimpleLambdaBuilder
    {
        private List<ParameterExpression> _params = new List<ParameterExpression>();
        private readonly List<KeyValuePair<ParameterExpression, bool>> _visibleVars = new List<KeyValuePair<ParameterExpression, bool>>();
        private string _name;
        private Type _returnType;
        private Expression _body;

        private static int _lambdaId; //for generating unique lambda name


        public SimpleLambdaBuilder(string functionName, Type type)
        {
            // TODO: Complete member initialization
            this._name = functionName;
            this._returnType = type;
        }

        /// <summary>
        /// The body of the lambda. This must be non-null.
        /// </summary>
        public Expression Body
        {
            get
            {
                return _body;
            }
            set
            {
                
                _body = value;
            }
        }

        /// <summary>
        /// Creates a parameter on the lambda with a given name and type.
        /// 
        /// Parameters maintain the order in which they are created,
        /// however custom ordering is possible via direct access to
        /// Parameters collection.
        /// </summary>
        public ParameterExpression Parameter(Type type, string name)
        {
            Contract.Requires(type != null);

            ParameterExpression result = Expression.Parameter(type, name);
            _params.Add(result);
            _visibleVars.Add(new KeyValuePair<ParameterExpression, bool>(result, false));
            return result;
        }

        /// <summary>
        /// List of lambda's parameters for direct manipulation
        /// </summary>
        public List<ParameterExpression> Parameters
        {
            get
            {
                return _params;
            }
        }



        private static Type GetLambdaType(Type returnType, IList<ParameterExpression> parameterList)
        {
            Contract.Requires(returnType != null);

            bool action = returnType == typeof(void);
            int paramCount = parameterList == null ? 0 : parameterList.Count;

            Type[] typeArgs = new Type[paramCount + (action ? 0 : 1)];
            for (int i = 0; i < paramCount; i++)
            {
                Contract.Requires(parameterList[i]!=null);
                typeArgs[i] = parameterList[i].Type;
            }

            Type delegateType;
            if (action)
                delegateType = Expression.GetActionType(typeArgs);
            else
            {
                typeArgs[paramCount] = returnType;
                delegateType = Expression.GetFuncType(typeArgs);
            }
            return delegateType;
        }


        /// <summary>
        /// Creates the LambdaExpression from the builder.
        /// After this operation, the builder can no longer be used to create other instances.
        /// </summary>
        /// <returns>New LambdaExpression instance.</returns>
        public LambdaExpression MakeLambda()
        {
            
            LambdaExpression lambda = Expression.Lambda(
                GetLambdaType(_returnType, _params),
                _body,
                _name + "$" + Interlocked.Increment(ref _lambdaId),
                _params
            );

            return lambda;
        }



        internal static SimpleLambdaBuilder Create(Type type, string functionName)
        {
            return new SimpleLambdaBuilder(functionName, type);
        }

    }
}
