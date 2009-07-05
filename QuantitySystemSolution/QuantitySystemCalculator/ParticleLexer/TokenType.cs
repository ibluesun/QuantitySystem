using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ParticleLexer.TokenTypes
{
    public abstract class TokenType
    {

        public Regex Regex
        {
            get; 
            private set;
        }

        public TokenType()
        {

            Type tclass = this.GetType();
            
            TokenPatternAttribute TPA = this.GetType().GetCustomAttributes(false)[0] as TokenPatternAttribute;

            if (TPA != null)
            {
                Regex = new Regex("^" + TPA.RegexPattern + "$", RegexOptions.Compiled);
            }

        }

        
    }


    public class CharToken : TokenType
    {

    }

    public class GroupToken : TokenType
    {
        
    }

    [TokenPattern(RegexPattern = "(")]
    public class LeftParenthesisToken : TokenType
    {
    }

    [TokenPattern(RegexPattern = ")")]
    public class RightParenthesisToken : TokenType
    {
    }

    [TokenPattern(RegexPattern="\\w+")]
    public class WordToken : TokenType
    {
    }


    [TokenPattern(RegexPattern=@"\d+(\.|\.\d+)?([eE][-+]?\d+)?")]
    public class NumberToken : TokenType
    {
    }

    [TokenPattern(
        RegexPattern = @"\d+(\.|\.\d+)?([eE][-+]?\d+)?" 
        + "\\s*<(\\w+(\\^\\d+)?\\.?)+(/(\\w+(\\^\\d+)?\\.?)+)?>"
        )
    ]
    public class UnitizedNumberToken : TokenType
    {

    }


    [TokenPattern(RegexPattern="=>")]
    public class LambdaOperatorToken : TokenType
    {
    }

    [TokenPattern(RegexPattern = "=")]
    public class AssignmentOperator : TokenType
    {
    }



    [TokenPattern(RegexPattern = ",")]
    public class CommaOperatorToken : TokenType
    {
    }


    public class FunctionCallToken : TokenType
    {
    }

    public class FunctionParameterToken : TokenType
    {
    }

}
