using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler
{
    public class Parser
    {
        public class GrammarErrorException : Exception
        {
        }
        public enum GrammarRuleResult
        {
            OKAY, WARN, STOP
        }
        public class GrammarDefinition
        {
            public delegate GrammarRuleResult GrammarRule(Grammar g, out string msg);
            public Lexer.TokenType TokenType;
            public List<GrammarRule> GrammarRules;
            
            public GrammarDefinition()
            {
                GrammarRules = new List<GrammarRule>();
            }

            public GrammarDefinition AddRule(GrammarRule rule)
            {
                GrammarRules.Add(rule);
                return this;
            }

            public void CheckGrammar(Grammar g)
            {
                foreach (var rule in GrammarRules)
                {
                    string msg;
                    GrammarRuleResult res = rule(g, out msg);
                    //if(res == GrammarRuleResult.STOP)
                        // throw an error.
                }
            }
        }
        public class Grammar
        {
            public Grammar NestedL, NestedR;
            public Lexer.Token Token;
        }

        private List<GrammarDefinition> _definitions;

        public Parser()
        {
            _definitions = new List<GrammarDefinition>();

            //out string msg;
            _definitions.Add(new GrammarDefinition().AddRule((Grammar g, out string msg) => {
                msg = "n/a";
            });
        }
    }
}
