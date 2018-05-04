using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Compiler
{
    public class Parser
    {
        public enum Symbol
        {
            NODE_TYPE_ROOT,
            KEYWORD, IDENTIFIER, CONSTANT, PUNCTUATOR, STRING_LITERAL
        }
        public class GrammarDefinition
        {
            public List<Symbol> Accepts;
            public Lexer.TokenType TokenType;
            public Symbol EvaluatesTo;

            public GrammarDefinition(Lexer.TokenType tType, Symbol evalTo, params Symbol[] accepts)
            {
                Accepts = new List<Symbol>();
                if(Accepts.Count > 0)
                    Accepts.AddRange(accepts);

                TokenType = tType;
                EvaluatesTo = evalTo;
            }

            public void AddAccepts(params Symbol[] accepts) => Accepts.AddRange(accepts);
        }
        public class GrammarNode
        {
            public List<GrammarNode> Children;
            public Symbol Symbol;
            public Lexer.Token Token;

            public GrammarNode(Symbol sym, Lexer.Token toke = null)
            {
                Children = new List<GrammarNode>();
                Symbol = sym;
                Token = toke;
            }

            public void AddChild(GrammarNode node) => Children.Add(node);

            public void GrammarCheck()
            {
            }
        }

        public GrammarNode Root;

        private List<GrammarDefinition> _definitions;

        public Parser()
        {
            _definitions = new List<GrammarDefinition>();

            _definitions.Add(new GrammarDefinition(Lexer.TokenType.END_OF_FILE, Symbol.NODE_TYPE_ROOT));
            _definitions.Add(new GrammarDefinition(Lexer.TokenType.START_OF_FILE, Symbol.NODE_TYPE_ROOT));

            _definitions.Add(new GrammarDefinition(Lexer.TokenType.NUMBER, Symbol.CONSTANT, null));
            _definitions.Add(new GrammarDefinition(Lexer.TokenType.OPERATOR, Symbol.PUNCTUATOR, Symbol.CONSTANT));
        }

        public GrammarNode Parse(string msg)
        {
            Lexer parser = new Lexer();
            var root = new GrammarNode(Symbol.NODE_TYPE_ROOT);

            var nodes = new List<GrammarNode>();
            foreach(var token in parser.Tokenize(msg))
            {
                var match = _definitions.First((x) => x.TokenType == token.TokenType); // Find the grammar definition
                nodes.Add(new GrammarNode(match.EvaluatesTo, token));

            }
            nodes.ForEach((node) => System.Console.WriteLine($"{node.Symbol} // {node.Token.Value}"));
            return root;
        }
    }
}
