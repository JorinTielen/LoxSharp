using System.Collections.Generic;

namespace LoxSharp 
{
    public interface IExprVisitor<T>
    {
        T Visit(Expr.Assign expr);
        T Visit(Expr.Binary expr);
        T Visit(Expr.Call expr);
        T Visit(Expr.Get expr);
        T Visit(Expr.Grouping expr);
        T Visit(Expr.Literal expr);
        T Visit(Expr.Logical expr);
        T Visit(Expr.Set expr);
        T Visit(Expr.Super expr);
        T Visit(Expr.This expr);
        T Visit(Expr.Unary expr);
        T Visit(Expr.Variable expr);
    }

    public abstract class Expr
    {
        public abstract T Accept<T>(IExprVisitor<T> visitor);

        public class Assign : Expr
        {
            public Token Name { get; }
            public Expr Val { get; }

            public Assign(Token name, Expr val) =>
                (Name, Val) = (name, val);

            public override T Accept<T>(IExprVisitor<T> visitor) =>
                visitor.Visit(this);
        }

        public class Binary : Expr
        {
            public Expr Left { get; }
            public Token Op { get; }
            public Expr Right { get; }

            public Binary(Expr left, Token op, Expr right) =>
                (Left, Op, Right) = (left, op, right);

            public override T Accept<T>(IExprVisitor<T> visitor) =>
                visitor.Visit(this);
        }

        public class Call : Expr
        {
            public Expr Callee { get; }
            public Token Paren { get; }
            public List<Expr> Args { get; }

            public Call(Expr callee, Token paren, List<Expr> args) =>
                (Callee, Paren, Args) = (callee, paren, args);

            public override T Accept<T>(IExprVisitor<T> visitor) =>
                visitor.Visit(this);
        }

        public class Get : Expr
        {
            public Expr Obj { get; }
            public Token Name { get; }

            public Get(Expr obj, Token name) =>
                (Obj, Name) = (obj, name);

            public override T Accept<T>(IExprVisitor<T> visitor) =>
                visitor.Visit(this);
        }

        public class Grouping : Expr
        {
            public Expr Expression { get; }

            public Grouping(Expr expression) => Expression = expression;
 
            public override T Accept<T>(IExprVisitor<T> visitor) =>
                visitor.Visit(this);
   
        }

        public class Literal : Expr
        {
            public object Val { get; }

            public Literal(object val) => Val = val;
  
            public override T Accept<T>(IExprVisitor<T> visitor) =>
                visitor.Visit(this);
      }

        public class Logical : Expr
        {
            public Expr Left { get; }
            public Token Op { get; }
            public Expr Right { get; }

            public Logical(Expr left, Token op, Expr right) =>
                (Left, Op, Right) = (left, op, right);
            
            public override T Accept<T>(IExprVisitor<T> visitor) =>
                visitor.Visit(this);
       }

        public class Set : Expr
        {
            public Expr Obj { get; }
            public Token Name { get; }
            public Expr Val { get; }

            public Set(Expr obj, Token name, Expr val) =>
                (Obj, Name, Val) = (obj, name, val);
            
            public override T Accept<T>(IExprVisitor<T> visitor) =>
                visitor.Visit(this);
        }

        public class Super : Expr
        {
            public Token Keyword { get; }
            public Token Method { get; }

            public Super(Token keyword, Token method) =>
                (Keyword, Method) = (keyword, method);
            
            public override T Accept<T>(IExprVisitor<T> visitor) =>
                visitor.Visit(this);
        }

        public class This : Expr
        {
            public Token Keyword { get; }

            public This(Token keyword) => Keyword = keyword;
            
            public override T Accept<T>(IExprVisitor<T> visitor) =>
                visitor.Visit(this);
        }

        public class Unary : Expr
        {
            public Token Op { get; }
            public Expr Right { get; }

            public Unary(Token op, Expr right) =>
                (Op, Right) = (op, right);

            public override T Accept<T>(IExprVisitor<T> visitor) =>
                visitor.Visit(this);
        }

        public class Variable : Expr
        {
            public Token Name { get; }

            public Variable(Token name) => Name = name;

            public override T Accept<T>(IExprVisitor<T> visitor) =>
                visitor.Visit(this);
        }
    }
}

