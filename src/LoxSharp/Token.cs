namespace LoxSharp 
{
    public readonly struct Token
    {
        public TokenType Type { get; }
        public string Lexeme { get; }
        public object Literal { get; }
        public int Line { get; }

        public Token(TokenType type, string lexeme, object literal, int line)
            => (Type, Lexeme, Literal, Line) = (type, lexeme, literal, line);

        public override string ToString() => $"<{Type} literal={Literal} line={Line}>";
    }
}

