using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace QuantitySystem.Runtime
{
    /// <summary>
    /// Agnostic token class hold any desirable value and can be nested.
    /// </summary>
    public class Token : IEnumerable<Token>
    {
        public string Value = string.Empty;
        public TokenClass TokenClass { get; set; }

        /// <summary>
        /// Test if one of the childs of this token is from the given token class
        /// </summary>
        /// <param name="tokenClass"></param>
        /// <returns></returns>
        public bool Contains(TokenClass tokenClass)
        {
            if (childTokens.Count(o => o.TokenClass == tokenClass) > 0)
                return true;
            else
                return false;
        }

        #region structure & methods

        private List<Token> childTokens = new List<Token>();

        public Token AppendToken()
        {
            return AppendToken(string.Empty);
        }

        public Token AppendToken(char value)
        {
            return AppendToken(value.ToString());
        }

        public Token AppendToken(string value)
        {
            Token token = new Token() { Value = value, ParentToken = this };

            childTokens.Add(token);

            return token;
        }

        public void AppendToken(Token token)
        {
            token.ParentToken = this;

            childTokens.Add(token);

        }

        public Token this[int index]
        {
            get
            {
                return childTokens[index];
            }
        }

        public int Count
        {
            get { return childTokens.Count; }
        }

        public Token ParentToken { get; set; }

        public string TokenValue
        {
            get
            {
                if (childTokens.Count > 0)
                {
                    string total = string.Empty;
                    foreach (Token t in childTokens)
                    {
                        total += t.TokenValue;
                    }
                    return total;
                }
                else
                {
                    return Value;
                }
            }
        }

        #endregion


        public override string ToString()
        {
            return TokenClass.ToString() + ": " + TokenValue;
        }

        #region Grouping Functions

        /// <summary>
        /// assemble '(' --- ')' into individual tokens
        /// </summary>
        /// <returns></returns>
        public Token GroupParenthesis()
        {
            Token first = new Token();
            Token current = first;

            int ci = 0;
            while (ci < childTokens.Count)
            {
                var c = childTokens[ci];

                if (c.TokenValue == "(")
                {
                    current = current.AppendToken();
                    current.AppendToken(c.TokenValue).TokenClass = TokenClass.LeftParenthesis;
                }
                else if (c.TokenValue == ")")
                {
                    current.AppendToken(c.TokenValue).TokenClass = TokenClass.RightParenthesis;

                    current.TokenClass = TokenClass.Group;


                    current = current.ParentToken;

                }
                else
                {
                    current.AppendToken(c);

                }

                ci++;
            }

            return first;
        }





        /// <summary>
        /// Merge Single Tokens into one token guided by regular expression.        
        /// </summary>
        /// <returns></returns>
        public Token MergeTokens(string pattern, TokenClass tokenClass)
        {
            Regex rx = new Regex("^" + pattern + "$", RegexOptions.Compiled);

            Token current = new Token();

            Token merged = new Token();

            int tokIndex = 0;
            while (tokIndex < childTokens.Count)
            {
            loopHead:
                Token tok = childTokens[tokIndex];

                if (rx.Match(merged.TokenValue + tok.TokenValue).Success)
                {
                    //continue merge untill merged value fail then last merged value is the desired value.

                    merged.AppendToken(tok);
                    merged.TokenClass = tokenClass;
                }
                else
                {
                    //merge failed on last token value

                    //now there is a chance that if we consume another letters that we back into the success again
                    //   it is like if we want to compare  tamer , begining with t,a,m,e, will fail untill we reach ,r
                    //     I will make dirty solution to try
                    //      consume rest of tokens untill found a success or end the discussion :)

                    if (!string.IsNullOrEmpty(merged.TokenValue))
                    {
                        // inner sneaky loop. :)
                        int rtokIndex = tokIndex;
                        string emval = merged.TokenValue;
                        while (rtokIndex < childTokens.Count)
                        {
                            emval += childTokens[rtokIndex].TokenValue;
                            if (rx.IsMatch(emval))
                            {
                                //yaaaaahoooo

                                //  after we run over tokens for unknown steps we found a success

                                // merge all tokens that made the success

                                // alter the original loop index and go to the loop tail

                                for (; tokIndex <= rtokIndex; tokIndex++)
                                {
                                    merged.AppendToken(childTokens[tokIndex]);
                                }

                                if (tokIndex < childTokens.Count)
                                    goto loopHead;
                                else
                                    goto loopTail;

                            }
                            rtokIndex++;
                        }
                    }


                    // if merged token is not null put the merged value 
                    //  continue to test the last token with next tokens to the same regex

                    if (!string.IsNullOrEmpty(merged.TokenValue))
                    {
                        if (rx.IsMatch(merged.TokenValue)) merged.TokenClass = tokenClass;
                        current.AppendToken(merged);
                        merged = new Token();

                        merged.AppendToken(tok);  // for begining another test with the new token
                        merged.TokenClass = tok.TokenClass;
                    }
                    else
                    {
                        //merged token is null
                        merged.AppendToken(tok);
                        merged.TokenClass = tok.TokenClass;
                    }


                }

            loopTail:                
                tokIndex++;

            }

            if (!string.IsNullOrEmpty(merged.TokenValue))
            {
                if (rx.IsMatch(merged.TokenValue)) merged.TokenClass = tokenClass;
                current.AppendToken(merged);
            }
            return current;
        }


        public Token RemoveSpaceTokens()
        {
            Token first = new Token();
            Token current = first;

            int ci = 0;
            while (ci < childTokens.Count)
            {
                var tok = childTokens[ci];
                
                //make sure all chars in value are white spaces
                
                if (tok.TokenValue.ToCharArray().Count(w => char.IsWhiteSpace(w)) == tok.TokenValue.Length)
                {
                    //all string are white spaces
                }
                else
                {
                    current.AppendToken(tok);

                }

                ci++;
            }

            return first;
        }
        
        #endregion

        #region Helper Functions

        /// <summary>
        /// All Characters in text will be tokens inside the returned token.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Token ParseText(string text)
        {
            Token current = new Token();

            int ci = 0;
            while (ci < text.Length)
            {
                char c = text[ci];
                {
                    current.AppendToken(c).TokenClass= TokenClass.Char;
                }

                ci++;
            }
            return current;
        }
        #endregion


        #region Constant Regular Expressions
        public const string Number = @"\d+(\.|\.\d+)?([eE][-+]?\d+)?";
        public const string UnitizedNumber = Number + "\\s*<(\\w+(\\^\\d+)?\\.?)+(/(\\w+(\\^\\d+)?\\.?)+)?>";   //match float with <unit>

        #endregion


        #region IEnumerable<Token> Members

        public IEnumerator<Token> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return childTokens.GetEnumerator();
        }

        #endregion
    }
}
