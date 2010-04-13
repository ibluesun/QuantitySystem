using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Ast;
using ParticleLexer;
using Qs;
using ParticleLexer.StandardTokens;

namespace SymbolicAlgebra
{
    public class SymbolicOperation
    {
        public Expression SymbolicExpression { get; set; }
        public string Operation { get; set; }
        public SymbolicOperation Next { get; set; }
    }
   
    public class SymbolicParser
    {

        public Expression ParseSymbols(Token tokens)
        {
            
            Expression SymbolicExpression = null;
            SymbolicOperation eop = null;

            SymbolicOperation FirstEop = null;

            int ix = 0;                 //this is the index in the discovered tokens
            
            while (ix < tokens.Count)
            {
                string q = tokens[ix].TokenValue;
                string op = ix + 1 < tokens.Count ? tokens[ix + 1].TokenValue : string.Empty;

                if (q == "+" || q == "-")
                {
                    // unary prefix operator.

                    //consume another token for number
                    
                    if (q == "+")
                    {
                        //q = tokens[ix].TokenValue;
                        SymbolicExpression = Expression.Constant(new SymbolicVariable("1"), typeof(SymbolicVariable));
                    }
                    else
                    {
                        SymbolicExpression = Expression.Constant(new SymbolicVariable("-1"), typeof(SymbolicVariable));
                    }

                    op = "*";
                    ix--;
                    goto ExpressionCompleted;
                    
                }


                
               SymbolicExpression = Expression.Constant(new SymbolicVariable(q));

            
                //apply the postfix here

                
            ExpressionCompleted:
                if (eop == null)
                {
                    //firs time creation
                    FirstEop = new SymbolicOperation();

                    eop = FirstEop;
                }
                else
                {
                    //use the next object to be eop.
                    eop.Next = new SymbolicOperation();
                    eop = eop.Next;
                }

                eop.Operation = op;
                eop.SymbolicExpression = SymbolicExpression;

                ix += 2;

            }

            if (eop.Next == null && string.IsNullOrEmpty(eop.Operation)==false)
            { 
                //eop hold the last node to be evaluated
                // if the next of eop is null then it means an operation without right term
                //    to do the operation on it.
                // 
                //  so raise an exception

                throw new QsException("Incomplete expression");

            }

            //then form the calculation expression
            return  ConstructExpression(FirstEop);

        }

        private Expression ConstructExpression(SymbolicOperation FirstEop)
        {
            //Treat operators as groups
            //  means * and /  are in the same pass
            //  + and - are in the same pass

            // passes depends on priorities of operators.


            string[] Group = { "^"    /* Power for normal product '*' */, 
                               "^."   /* Power for dot product */ ,
                               "^x"   /* Power for cross product */ };

            string[] Group1 = { "*"   /* normal multiplication */, 
                                "."   /* dot product */, 
                                "x"   /* cross product */, 
                                "(*)" /* Tensor product */,
                                "/"   /* normal division */, 
                                "%"   /* modulus */ };

            string[] Group2 = { "+", "-" };

            string[] Shift = { "<<", ">>" };

            string[] RelationalGroup = { "<", ">", "<=", ">=" };
            string[] EqualityGroup = { "==", "!=" };
            string[] AndGroup = { "and" };
            string[] OrGroup = { "or" };

            string[] WhenOtherwiseGroup = { "when", "otherwise" };


            /// Operator Groups Ordered by Priorities.
            string[][] OperatorGroups = { Group, Group1, Group2, Shift, RelationalGroup, EqualityGroup, AndGroup, OrGroup, WhenOtherwiseGroup };



            foreach (var opg in OperatorGroups)
            {
                SymbolicOperation eop = FirstEop;

                //Pass for '[op]' and merge it  but from top to child :)  {forward)
                while (eop.Next != null)
                {
                    //if the operator in node found in the opg (current operator group) then execute the logic

                    if (opg.Count(c => c.Equals(eop.Operation, StringComparison.OrdinalIgnoreCase)) > 0)
                    {
                        short skip;
                        eop.SymbolicExpression = ArithExpression(eop, out skip);

                        //drop eop.Next
                        if (eop.Next.Next != null)
                        {
                            while (skip > 0)
                            {
                                eop.Operation = eop.Next.Operation;

                                eop.Next = eop.Next.Next;

                                skip--;
                            }
                        }
                        else
                        {
                            //no more nodes exit the loop

                            eop.Next = null;      //last item were processed.
                            eop.Operation = string.Empty;
                        }
                    }
                    else
                    {
                        eop = eop.Next;
                    }
                }
            }

            return FirstEop.SymbolicExpression;
        }

        private Expression ArithExpression(SymbolicOperation eop, out short skip)
        {
            Expression left = eop.SymbolicExpression;
            string op = eop.Operation;
            Expression right = eop.Next.SymbolicExpression;

            skip = 1;

            Type aqType = typeof(SymbolicVariable);

            if (op == "*") return Expression.Multiply(left, right);

            if (op == "/") return Expression.Divide(left, right);
            if (op == "%") return Expression.Modulo(left, right);
            if (op == "+") return Expression.Add(left, right);
            if (op == "-") return Expression.Subtract(left, right);

            if (op == "^") return Expression.Power(left, right, aqType.GetMethod("Power"));



            throw new NotSupportedException("Not Supported Operator '" + op + "'");
        }


    }
}
