using System;

namespace LoxSharp 
{
    class Lox 
    {
        static bool HadError = false;

        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: cslox [script]");
            } 
            else if (args.Length == 1)
            {
                RunFile(args[0]);
            }
            else
            {
                RunPrompt();
            }
        }

        static void RunFile(string file)
        {
            Run(System.IO.File.ReadAllText(file));
            if (HadError) Environment.Exit(65);
        }

        static void RunPrompt()
        {
            for (;;)
            {
                Console.Write("> ");
                Run(Console.ReadLine());
                HadError = false;
            }
        }

        static void Run(string source)
        {
            // Scanner scanner = new Scanner(source);
            // List<Token> tokens = scanner.ScanTokens();

            // foreach (Token token in tokens) {
            //     Console.WriteLine(token);
            // }
        }

        public static void Error(int line, string message)
        {
            Console.WriteLine("[line " + line + "] Error: " + message);
            HadError = true;
        }
    }
}
