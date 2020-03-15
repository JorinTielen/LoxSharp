using System.Collections.Generic;
using System.Globalization;
using static LoxSharp.TokenType;

namespace LoxSharp
{
    class Scanner
    {
        private readonly string source;
        private List<Token> tokens = new List<Token>();

        private static readonly Dictionary<string, TokenType> keywords =
            new Dictionary<string, TokenType>
        {
            { "and", AND },
            { "class", CLASS },
            { "else", ELSE },
            { "false", FALSE },
            { "for", FOR },
            { "fun", FUN },
            { "if", IF },
            { "nil", NIL },
            { "or", OR },
            { "print", PRINT },
            { "return", RETURN },
            { "super", SUPER },
            { "this", THIS },
            { "true", TRUE },
            { "var", VAR },
            { "while", WHILE }
        };

        private int start = 0;
        private int current = 0;
        private int line = 1;
        
        public Scanner(string source) => this.source = source;

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }


            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(LEFT_PAREN); break;
                case ')': AddToken(RIGHT_PAREN); break;
                case '{': AddToken(LEFT_BRACE); break;
                case '}': AddToken(RIGHT_BRACE); break;
                case ',': AddToken(COMMA); break;
                case '.': AddToken(DOT); break;
                case '-': AddToken(MINUS); break;
                case '+': AddToken(PLUS); break;
                case ';': AddToken(SEMICOLON); break;
                case '*': AddToken(STAR); break;

                case '!': AddToken(Match('=') ? NE : NEG); break;
                case '=': AddToken(Match('=') ? EQ : ASSIGN); break;
                case '<': AddToken(Match('=') ? LESS_EQ : LESS); break;
                case '>': AddToken(Match('=') ? GREATER_EQ : GREATER); break;

                case '"': String(); break;

                case '/':
                {
                    if (Match('/'))
                    {
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else
                    {
                        AddToken(SLASH);
                    }
                    break;
                }

                case ' ': break;                                    
                case '\r': break;                             
                case '\t': break;                            
                case '\n': line++; break;

                default:
                {
                    if (IsDigit(c))
                    {
                        Number();
                        break;
                    }

                    if (IsAlpha(c))
                    {
                        Identifier();
                        break;
                    }

                    Lox.Error(line, "Unexpected character.");
                    break;
                }
            }
        }

        private char Advance()
        {
            current++;
            return source[current - 1];
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return source[current];
        }

        private char PeekNext()
        {
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];
        }

        private bool Match(char expected)
        {
            if (!IsAtEnd()) return false;
            if (source[current] != expected) return false;

            current++;
            return true;
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek())) Advance();

            string text = source.Substring(start, current - start);

            TokenType type = IDENTIFIER;
            keywords.TryGetValue(text, out type);
            AddToken(type);
        }

        private void Number()
        {
            while (IsDigit(Peek())) Advance();

            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                Advance();

                while (IsDigit(Peek())) Advance();
            }

            double value = double.Parse(source.Substring(start, current - start), CultureInfo.InvariantCulture);
            AddToken(NUMBER, value);
        }

        private void String()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') line++;
                Advance();
            }

            if (IsAtEnd())
            {
                Lox.Error(line, "Unterminated string.");
                return;
            }

            Advance();

            string value = source.Substring(start + 1, current - start);
            AddToken(STRING, value);
        }

        private void AddToken(TokenType type, object literal = null)
        {
            string text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }

        private bool IsAlphaNumeric(char c) => IsAlpha(c) || IsDigit(c);

        private bool IsAlpha(char c) =>
            (c >= 'a' && c <= 'z') ||
            (c >= 'A' && c <= 'Z') ||
             c == '_';

        private bool IsDigit(char c) => c >= '0' && c <= '9';

        private bool IsAtEnd() => current >= source.Length - 1;
    }
}

