using LoxSharp.Exceptions;
using System;
using System.Collections.Generic;

namespace LoxSharp
{
    class Lox
    {
        private static readonly Interpreter interpreter = new Interpreter();

        static bool HadError = false;
        static bool HadRuntimeError = false;

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
            if (HadRuntimeError) Environment.Exit(70);
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
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();

            Parser parser = new Parser(tokens);
            List<Stmt> statements = parser.Parse();

            if (HadError) return;

            // Console.WriteLine(new AstPrinter().Print(expression));
            interpreter.Interpret(statements);
        }

        public static void Error(int line, string message) => Report(line, "", message);

        public static void Error(Token token, string message)
        {
            if (token.Type == TokenType.EOF)
            {
                Report(token.Line, "at end", message);
            }
            else
            {
                Report(token.Line, $"at '{token.Lexeme}'", message);
            }
        }

        public static void RuntimeError(RuntimeException e)
        {
            Console.WriteLine($"[line {e.Token.Line}] Runtime Error: {e.ToString()}");
            HadRuntimeError = true;
        }

        public static void Report(int line, string where, string message)
        {
            Console.WriteLine($"[line {line}] Error {where}: {message}");
            HadError = true;
        }
    }
}
