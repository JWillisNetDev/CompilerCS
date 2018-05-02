using System;
using System.Diagnostics;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexer Lexer = new Lexer();
            string testCode = "123.456 + 456.789 " +
                "24:12:06.00 12/06/03";
            foreach(var t in Lexer.Tokenize(testCode))
                Console.WriteLine($"{t.TokenType} // {t.Value}");
            Console.ReadKey();
        }
    }
}
