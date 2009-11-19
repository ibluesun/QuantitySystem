using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using ParticleLexer.TokenTypes;
using System.Diagnostics;

namespace ParticleLexer
{
    /// <summary>
    /// Agnostic token class hold any desirable value and can be nested.
    /// </summary>
    public sealed class Token : IEnumerable<Token>
    {
        public string Value = string.Empty;
        public Type TokenType { get; set; }

        private int _IndexInText;


        /// <summary>
        /// Test if one of the childs of this token is from the given token class
        /// </summary>
        /// <param name="tokenClass"></param>
        /// <returns></returns>
        public bool Contains(Type tokenType)
        {
            if (childTokens.Count(o => o.TokenType == tokenType) > 0)
                return true;
            else
                return false;
        }

        #region structure & methods

        private List<Token> childTokens = new List<Token>();

        public Token AppendSubToken()
        {
            return AppendSubToken(string.Empty);
        }

        public Token AppendSubToken(char value)
        {
            return AppendSubToken(value.ToString());
        }

        public Token AppendSubToken(string value)
        {
            Token token = new Token() { Value = value, ParentToken = this };

            childTokens.Add(token);

            return token;
        }
        public void AppendSubToken(Token token)
        {
            token.ParentToken = this;
            childTokens.Add(token);
        }

        public void AppendToken(Token token)
        {
            //token.ParentToken = this;
            //childTokens.Add(token);

            ParentToken.AppendSubToken(token);

        }

        public Token this[int index]
        {
            get
            {
                return childTokens[index];
            }
            set
            {
                childTokens[index] = value;
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

        public int TokenValueLength
        {
            get
            {
                return TokenValue.Length;
            }
        }


        public int IndexInText
        {
            get
            {
                if (this.childTokens.Count > 0)
                    return this.childTokens[0].IndexInText;
                else
                    return _IndexInText;
            }
        }

        #endregion


        public override string ToString()
        {
            return TokenType.ToString() + ": " + TokenValue;
        }

        #region Grouping Functions, Sequences

        /// <summary>
        /// Assemble Parenthesis '(' ((()()(()())))--- ')' into individual tokens and Parenthesis groups tokens
        /// Assemble Square Brakets '[' [[[][][[][]]]] --- ']' into individual tokens and Square Brackets groups tokens
        /// </summary>
        /// <returns></returns>
        public Token GroupBrackets()
        {
            Token first = new Token();
            Token current = first;

            int ci = 0;

            while (ci < childTokens.Count)
            {
                var c = childTokens[ci];


                if (c.TokenValue == "(")
                {

                    current = current.AppendSubToken();
                    current.AppendSubToken(c.TokenValue).TokenType = typeof(LeftParenthesisToken);
                }
                else if (c.TokenValue == "[")
                {

                    current = current.AppendSubToken();
                    current.AppendSubToken(c.TokenValue).TokenType = typeof(LeftSquareBracketToken);
                }
                else if (c.TokenValue == "{")
                {
                    current = current.AppendSubToken();
                    current.AppendSubToken(c.TokenValue).TokenType = typeof(LeftCurlyBracketToken);
                }
                else if (c.TokenValue == "]")
                {
                    current.AppendSubToken(c.TokenValue).TokenType = typeof(RightSquareBracketToken);

                    current.TokenType = typeof(SquareBracketGroupToken);

                    current = current.ParentToken;
                }
                else if (c.TokenValue == ")")
                {
                    current.AppendSubToken(c.TokenValue).TokenType = typeof(RightParenthesisToken);

                    current.TokenType = typeof(ParenthesisGroupToken);

                    current = current.ParentToken;

                }
                else if (c.TokenValue == "}")
                {
                    current.AppendSubToken(c.TokenValue).TokenType = typeof(RightCurlyBracketToken);

                    current.TokenType = typeof(CurlyBracketGroupToken);

                    current = current.ParentToken;
                }
                else
                {
                    current.AppendSubToken(c);
                }

                ci++;
            }

            return Zabbat(first);
        }

        /// <summary>
        /// After merging words and grouping brackets
        /// this function try to discover the calls to sequences and functions.
        /// </summary>
        /// <returns></returns>
        public Token DiscoverCalls()
        {
            return DiscoverCalls(StringComparer.OrdinalIgnoreCase, new string[] { });
        }

        /// <summary>
        /// Should be used after merging by word
        /// and grouping brackets
        /// The function tries to find Word followed by complete parenthesis group or square brackets group.
        /// And can parse this example:
        ///            fn(434,fn((434+434)+8/4,50,fifo(5)))
        ///            
        ///            S[5,F[4,2,1],G](4, 3, 2,R[2], p(30*o(9)))
        ///            -              - Parameters              -
        ///            -    Indexes   -
        ///            -              Sequence Call             -
        /// </summary>
        /// <param name="ignoreWords">list of words that should be ignored when discovering calls </param>
        /// <returns></returns>
        public Token DiscoverCalls(StringComparer stringComparer, params string[] ignoreWords)
        {
            Token first = new Token();
            Token current = first;

            int ci = 0;

            while (ci < childTokens.Count)
            {
                var c = childTokens[ci];

                if (c.Contains(typeof(ParenthesisGroupToken)) | c.Contains(typeof(SquareBracketGroupToken)))
                {
                    //recursive call if the token have inside groups
                    current.AppendSubToken(c.DiscoverCalls());
                }
                else
                {
                    //sub groups then test this token 
                    if (
                        (
                              c.TokenType == typeof(WordToken)               // word token
                           || c.TokenType == typeof(NameSpaceAndValueToken)  // or namespace:value token
                        )
                        && ignoreWords.Contains(c.TokenValue, stringComparer)==false         // and the whole value is not in  the ignore words
                        )
                    {
                        //check if the next token is group
                        if (ci < this.Count-1)
                        {
                            Token cnext = childTokens[ci + 1];

                            #region Parenthesis group discovery
                            if (cnext.TokenType == typeof(ParenthesisGroupToken))
                            {
                                // so this is a function
                                //take the current token with the next token and make it as functionToken

                                Token functionCallToken = new Token();
                                functionCallToken.TokenType = typeof(FunctionCallToken);
                                functionCallToken.AppendSubToken(c);



                                if (cnext.Contains((typeof(ParenthesisGroupToken))) | cnext.Contains(typeof(SquareBracketGroupToken)))
                                {
                                    cnext = cnext.DiscoverCalls();
                                }

                                cnext = SplitFunctionParamerers(cnext);


                                functionCallToken.AppendSubToken(cnext);

                                current.AppendSubToken(functionCallToken);

                                ci += 2;
                                continue;
                            }
                            #endregion

                            #region Square Brackets discovery
                            if (cnext.TokenType == typeof(SquareBracketGroupToken))
                            {
                                // so this is a sequence
                                //take the current token with the next token and make it as sequenceToken

                                Token sequenceCallToken = new Token();
                                sequenceCallToken.TokenType = typeof(SequenceCallToken);
                                sequenceCallToken.AppendSubToken(c);

                                if (cnext.Contains((typeof(SquareBracketGroupToken))) | cnext.Contains((typeof(ParenthesisGroupToken))))
                                {
                                    cnext = cnext.DiscoverCalls();
                                }

                                cnext = SplitSequenceIndexes(cnext);

                                sequenceCallToken.AppendSubToken(cnext);

                                if (childTokens.Count > ci + 2)
                                {
                                    //check if we have a Parenthesis parameters after Square Brackets
                                    Token cnextnext = childTokens[ci + 2];
                                    if (cnextnext.TokenType == typeof(ParenthesisGroupToken))
                                    {
                                        //then this is a sequence call with parameters.
                                        if ((cnextnext.Contains((typeof(SquareBracketGroupToken))) | cnextnext.Contains((typeof(ParenthesisGroupToken)))))
                                        {
                                            cnextnext = cnextnext.DiscoverCalls();
                                        }

                                        cnextnext = SplitFunctionParamerers(cnextnext);

                                        sequenceCallToken.AppendSubToken(cnextnext);

                                        ci += 3;
                                    }
                                    else
                                    {
                                        ci += 2;
                                    }
                                }
                                else
                                {
                                    ci += 2;
                                }
                                current.AppendSubToken(sequenceCallToken);
                                continue;
                            }

                            #endregion

                        }
                    }

                    // if all conditions failed we put the token and resume to another one
                    current.AppendSubToken(childTokens[ci]);
                }

                ci++ ;
            }
            first.TokenType = this.TokenType;

            return Zabbat(first);
        }

        private Token SplitFunctionParamerers(Token cnext)
        {
            //to make the parameters tokens.

            cnext = cnext.MergeTokens(new CommaToken());

            //here I must merge all but the parenthesis
            Token temp = new Token();

            for (int iy = 1; iy < cnext.Count - 1; iy++)
                temp.AppendSubToken(cnext[iy]);


            temp = temp.MergeAllBut(typeof(FunctionParameterToken), new CommaToken());

            Token tmp2 = new Token();
            tmp2.TokenType = cnext.TokenType;
            tmp2.AppendSubToken(cnext[0]);
            foreach (Token itmp in temp)
                tmp2.AppendSubToken(itmp);
            tmp2.AppendSubToken(cnext[cnext.Count - 1]);

            return Zabbat(tmp2);
        }

        private Token SplitSequenceIndexes(Token cnext)
        {
            //to make the parameters tokens.

            cnext = cnext.MergeTokens(new CommaToken());

            //here I must merge all but the parenthesis
            Token temp = new Token();

            for (int iy = 1; iy < cnext.Count - 1; iy++)
                temp.AppendSubToken(cnext[iy]);


            temp = temp.MergeAllBut(typeof(SequenceIndexToken), new CommaToken());

            Token tmp2 = new Token();
            tmp2.TokenType = cnext.TokenType;
            tmp2.AppendSubToken(cnext[0]);
            foreach (Token itmp in temp)
                tmp2.AppendSubToken(itmp);
            tmp2.AppendSubToken(cnext[cnext.Count - 1]);

            return Zabbat(tmp2);
        }

        /// <summary>
        /// remove the token from the start of tokens.
        /// </summary>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        public Token TrimStart(Type tokenType)
        {
            Token Trimmed = new Token();

            int ci = 0;
            while (ci < childTokens.Count)
            {
                var tok = childTokens[ci];
                if (tok.TokenType == tokenType)
                {
                    //ignore this token
                }
                else
                {
                    //from here take the rest tokens
                    while (ci < childTokens.Count)
                    {
                        tok = childTokens[ci];
                        Trimmed.AppendSubToken(tok);
                        ci++;
                    }

                    break;
                }

                ci++;
            }

            return Trimmed;
        }

        /// <summary>
        /// Remove the token type from the end of tokens
        /// </summary>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        public Token TrimEnd(Type tokenType)
        {
            Token Trimmed = new Token();

            int ci = childTokens.Count - 1;
            while (ci >= 0)
            {
                var tok = childTokens[ci];
                if (tok.TokenType == tokenType)
                {
                    //ignore this token
                }
                else
                {
                    //from here take the rest tokens
                    for (int i = 0; i <= ci; i++)
                    {
                        tok = childTokens[i];
                        Trimmed.AppendSubToken(tok);
                        
                    }

                    break;
                }

                ci--;
            }

            return Trimmed;
        }



        /// <summary>
        /// Merge all tokens into the one and exclude specific token
        /// </summary>
        /// <param name="tokenType">Excluded tokens or Token(s) that act as separators.</param>
        /// <param name="mergedTokensType">The type of the new token merged from sub tokens.</param>
        /// <returns></returns>
        public Token MergeAllBut(Type mergedTokensType, params TokenType[] tokenTypes)
        {
            return MergeAllBut(0, mergedTokensType, tokenTypes);
        }

        /// <summary>
        /// Merge all tokens into the one and exclude specific token
        /// </summary>
        /// <param name="tokenType">Excluded token or the separator token.</param>
        /// <param name="mergedTokensType">The type of the new token merged from sub tokens.</param>
        /// <param name="startIndex">Starting from token index</param>
        /// <returns></returns>
        public Token MergeAllBut(int startIndex, Type mergedTokensType, params TokenType[] tokenTypes)
        {
            Token first = this.MergeTokens(tokenTypes[0]);
            for (int i = 1; i < tokenTypes.Length; i++)
            {
                first = first.MergeTokens(tokenTypes[i]);
            }
            
            Debug.Assert(first != null);

            Token current = new Token();

            // walk on all tokens and accumulate them unitl you encounter separator

            int ci = 0;

            Token mergedTokens = new Token();

            while (ci < first.Count)
            {

                var c = first[ci];

                if (ci < startIndex)
                {
                    current.AppendSubToken(c);
                }
                else
                {
                    if (tokenTypes.Count(tok => tok.GetType() == c.TokenType) == 0)
                    {
                        mergedTokens.AppendSubToken(c);
                    }
                    else
                    {
                        //found a separator
                        if (mergedTokens.Count > 0)
                        {
                            mergedTokens.TokenType = mergedTokensType;

                            current.AppendSubToken(mergedTokens);
                        }
                        current.AppendSubToken(c);

                        mergedTokens = new Token();

                    }
                }
                
                ci++;
            }

            if (mergedTokens.Count > 0)
            {
                //the rest of merged tokens
                mergedTokens.TokenType = mergedTokensType;

                current.AppendSubToken(mergedTokens);
            }


            current.TokenType = first.TokenType;

            return Zabbat(current);

        }

        /// <summary>
        /// Merge Single Tokens into one token guided by regular expression.        
        /// </summary>
        /// <returns></returns>
        public Token MergeTokens(TokenType tokenType)
        {
            //Regex rx = new Regex("^" + pattern + "$", RegexOptions.Compiled);
            Regex rx = tokenType.Regex;

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

                    merged.AppendSubToken(tok);
                    
                    merged.TokenType = tokenType.GetType();
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
                                    merged.AppendSubToken(childTokens[tokIndex]);
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
                        if (rx.IsMatch(merged.TokenValue)) merged.TokenType = tokenType.GetType();
                        current.AppendSubToken(merged);
                        merged = new Token();


                        // for begining another test with the new token
                        merged.AppendSubToken(tok);  
                        
                        merged.TokenType = tok.TokenType;
                    }
                    else
                    {
                        //merged token is null
                        merged.AppendSubToken(tok);
                        merged.TokenType = tok.TokenType;
                    }


                }

            loopTail:                
                tokIndex++;

            }

            if (!string.IsNullOrEmpty(merged.TokenValue))
            {
                if (rx.IsMatch(merged.TokenValue)) merged.TokenType = tokenType.GetType();
                current.AppendSubToken(merged);
            }

            current.TokenType = this.TokenType;
            return Zabbat(current);
        }


        /// <summary>
        /// This function make sure that inner tokens are not the same as outer tokens by popping out the
        /// buried tokens to the surface.
        /// </summary>
        /// <param name="melakhbat"></param>
        /// <returns></returns>
        private static Token Zabbat(Token melakhbat)
        {
            Token Metzabbat = new Token();
            foreach (Token h in melakhbat)
            {
                if (h.Count == 1)
                {
                    if (h.TokenType == h[0].TokenType) Metzabbat.AppendSubToken(h[0]);
                    else Metzabbat.AppendSubToken(h);
                }
                else
                {
                    Metzabbat.AppendSubToken(h);
                }
            }

            Metzabbat.TokenType = melakhbat.TokenType;

            return Metzabbat;
        }


        /// <summary>
        /// Removes specific tokens from the current tokens.
        /// </summary>
        /// <param name="tokenType">Array of tokens that should be removed.</param>
        /// <returns></returns>
        public Token RemoveTokens(params Type[] tokenTypes)
        {
            Token first = new Token();
            Token current = first;

            int ci = 0;
            while (ci < childTokens.Count)
            {
                var tok = childTokens[ci];

                //make sure all chars in value are white spaces

                //if (tok.TokenValue.ToCharArray().Count(w => char.IsWhiteSpace(w)) == tok.TokenValue.Length)
                if (tokenTypes.Count(f => f == tok.TokenType) > 0)
                {
                    //all string are white spaces
                }
                else
                {
                    current.AppendSubToken(tok);
                }

                ci++;
            }

            return first;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        public int IndexOf(Type tokenType)
        {
            int idx = -1;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].TokenType == tokenType)
                {
                    idx = i;
                    break;
                }
            }
            return idx;
        }


        /// <summary>
        /// Remove specific token until we reach a closing token.
        /// </summary>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        public Token RemoveTokenUntil(Type tokenType, Type untilToken)
        {
            Token first = new Token();
            Token current = first;

            bool reached = false;  //specifiy if we reach the close token or not.

            int ci = 0;
            while (ci < childTokens.Count)
            {
                var tok = childTokens[ci];

                //make sure all chars in value are white spaces

                if (tokenType == tok.TokenType && reached == false)
                {
                    //all string are white spaces
                }
                else
                {
                    if (tok.TokenType == untilToken) reached = true;

                    //not the required token add it to the return value.
                    current.AppendSubToken(tok);
                }

                ci++;
            }

            return first;
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
                
                //if (tok.TokenValue.ToCharArray().Count(w => char.IsWhiteSpace(w)) == tok.TokenValue.Length)
                if(tok.TokenType == typeof(SpaceToken))
                {
                    //all string are white spaces
                }
                else
                {
                    current.AppendSubToken(tok);
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
                    Token tk = current.AppendSubToken(c);
                    tk.TokenType = typeof(CharToken);
                    tk._IndexInText = ci;
                }

                ci++;
            }
            return current;
        }
        #endregion



        #region IEnumerable<Token> Members

        public IEnumerator<Token> GetEnumerator()
        {
            return childTokens.GetEnumerator();
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
