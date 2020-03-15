using System.Collections.Generic;

namespace LoxSharp
{
    public interface IStmtVisitor<T>
    {
        T Visit(Stmt.Print stmt);
        T Visit(Stmt.Expression stmt);
    }

    public abstract class Stmt
    {
        public abstract T Accept<T>(IStmtVisitor<T> visitor);

        public class Print : Stmt
        {
            public Expr Expr { get; }

            public Print(Expr expr) => Expr = expr;
  
            public override T Accept<T>(IStmtVisitor<T> visitor) =>
                visitor.Visit(this);
        }

        public class Expression : Stmt
        {
            public Expr Expr { get; }

            public Expression(Expr expr) => Expr = expr;

            public override T Accept<T>(IStmtVisitor<T> visitor) =>
                visitor.Visit(this);
        }
    }
}

