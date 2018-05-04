using System;
using System.Diagnostics;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser();
            string testCode = "123 + 456";
            parser.Parse(testCode);
            Console.ReadKey();
        }
    }
}
