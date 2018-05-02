using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Compiler
{
    public class Lexer
    {
        public enum TokenType
        {
            END_OF_LINE, // Semantics
            NUMBER, OPERATOR, // Math
            DATE, TIME, DATE_TIME, // Date Time
        }
        public class TokenDefinition
        {
            private Regex _regex;
            private readonly TokenType _tokenType;
            private readonly int _precedence;

            public TokenDefinition(TokenType retToken, string regexPattern, int precedence)
            {
                _regex = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                _tokenType = retToken;
                _precedence = precedence;
            }

            public IEnumerable<TokenMatch> FindMatches(string input)
            {
                var matches = _regex.Matches(input);
                for(int i=0; i < matches.Count; i++)
                {
                    yield return new TokenMatch()
                    {
                        StartIndex = matches[i].Index,
                        EndIndex = matches[i].Index + matches[i].Length,
                        TokenType = _tokenType,
                        Value = matches[i].Value,
                        Precedence = _precedence
                    };
                }
            }
        }
        public class TokenMatch
        {
            public TokenType TokenType { get; set; }
            public string Value { get; set; }
            public int StartIndex { get; set; }
            public int EndIndex { get; set; }
            public int Precedence { get; set; }
        }
        public class Token
        {
            public TokenType TokenType { get; set; }
            public string Value { get; set; }

            public Token(TokenType tokenType, string value = "")
            {
                TokenType = tokenType;
                Value = value;
            }

            public Token Clone() => new Token(TokenType, Value);
        }


        private List<TokenDefinition> _tokenDefinitions;

        public Lexer() {
            _tokenDefinitions = new List<TokenDefinition>();

            _tokenDefinitions.Add(new TokenDefinition(TokenType.END_OF_LINE, @";", 10));

            _tokenDefinitions.Add(new TokenDefinition(TokenType.NUMBER, @"\d+\.?\d+", 100));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.OPERATOR, @"[\+\-\/\*]", 100));

            _tokenDefinitions.Add(new TokenDefinition(TokenType.DATE, @"\d\d\/\d\d\/\d\d", 20)); // YYYY:MM:DD
            _tokenDefinitions.Add(new TokenDefinition(TokenType.TIME, @"\d\d\:\d\d\:\d\d\.?\d+", 20)); // HH:MM:SS.MS
            // _tokenDefinitions.Add(new TokenDefinition(TokenType.DATE_TIME, @"", 2)); // no
        }

        public IEnumerable<Token> Tokenize(string msg)
        {
            var tokenMatches = FindMatches(msg);

            var groupByIndex = tokenMatches.GroupBy(x => x.StartIndex)
                .OrderBy(x => x.Key)
                .ToList();

            TokenMatch lastMatch = null;
            for(int i=0; i < groupByIndex.Count; i++)
            {
                var bestMatch = groupByIndex[i].OrderBy(x => x.Precedence).First();
                if(lastMatch != null && bestMatch.StartIndex < lastMatch.EndIndex)
                    continue;
                yield return new Token(bestMatch.TokenType, bestMatch.Value);
                lastMatch = bestMatch;
            }
        }

        public List<Token> TokenizeToList(string msg)
        {
            List<Token> tokens = new List<Token>();
            foreach(var token in Tokenize(msg))
                tokens.Add(token);
            return tokens;
        }

        private List<TokenMatch> FindMatches(string msg)
        {
            var tokenMatches = new List<TokenMatch>();
            foreach(var tokenDefinition in _tokenDefinitions)
                tokenMatches.AddRange(tokenDefinition.FindMatches(msg).ToList());
            return tokenMatches;
        }
    }
}
