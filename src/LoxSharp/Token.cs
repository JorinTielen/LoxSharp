namespace LoxSharp 
{
    public readonly struct Token
    {
        public TokenType type { get; }
        public string lexeme { get; }
        public object literal { get; }
        public int line { get; }

        public Token(TokenType type, string lexeme, object literal, int line)
            => (this.type, this.lexeme, this.literal, this.line) = (type, lexeme, literal, line);

        public override string ToString() => $"<{type} literal={literal} line={line}>";
    }
}

