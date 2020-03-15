using System.Text;

namespace LoxSharp 
{
    public class AstPrinter : IExprVisitor<string>
    {
        public string Print(Expr expr) => expr.Accept(this);
    
        public string Visit(Expr.Assign expr) =>
            $"{expr.Name} = {expr.Val.Accept(this)}";

        public string Visit(Expr.Binary expr) =>
            Parenthesize(expr.Op.Lexeme, expr.Left, expr.Right);

        public string Visit(Expr.Get expr) =>
            Parenthesize(".", expr.Obj, expr.Name.Lexeme);

        public string Visit(Expr.Call expr) =>
            Parenthesize("call", expr.Callee, expr.Args);

        public string Visit(Expr.Grouping expr) =>
            Parenthesize("group", expr.Expression);

        public string Visit(Expr.Literal expr)
        {
            if (expr.Val == null) return "nil";
            return $"{expr.Val}";
        }

        public string Visit(Expr.Logical expr) =>
            Parenthesize(expr.Op.Lexeme, expr.Left, expr.Right);

        public string Visit(Expr.Unary expr) =>
            Parenthesize(expr.Op.Lexeme, expr.Right);

        public string Visit(Expr.Variable expr) =>
            expr.Name.Lexeme;

        public string Visit(Expr.Set expr) =>
            Parenthesize("=", expr.Obj, expr.Name.Lexeme, expr.Val); 

        public string Visit(Expr.Super expr) =>
            Parenthesize("super", expr.Method);

        public string Visit(Expr.This expr) => "this";

        private string Parenthesize(string name, params Expr[] exprs)
        {
            var builder = new StringBuilder();
            builder.Append($"({name}");

            foreach (var expr in exprs)
            {
                builder.Append($" {expr.Accept(this)}");
            }

            builder.Append(")");
            return builder.ToString();
        }

        private string Parenthesize(string name, params object[] parts)
        {
            var builder = new StringBuilder();
            builder.Append($"({name}");

            foreach (var part in parts)
            {
                builder.Append(" ");

                if (part is Expr)
                {
                    builder.Append((part as Expr).Accept(this));
                }
                else if (part is Token)
                {
                    builder.Append(((Token)part).Lexeme);
                }
                else
                {
                    builder.Append($"{part}");
                }
            }

            builder.Append(")");
            return builder.ToString();
        }
    }
}
