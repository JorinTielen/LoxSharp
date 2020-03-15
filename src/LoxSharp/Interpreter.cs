using LoxSharp.Exceptions;
using System;
using static LoxSharp.TokenType;

namespace LoxSharp
{
    class Interpreter : IExprVisitor<object>
    {
        public void Interpret(Expr expr)
        {
            try
            {
                object val = Evaluate(expr);
                Console.WriteLine(Stringify(val));
            }
            catch (RuntimeException e)
            {
                Lox.RuntimeError(e);
            }
        }
        public object Visit(Expr.Literal expr) => expr.Val;

        public object Visit(Expr.Grouping expr) => Evaluate(expr.Expression);

        public object Visit(Expr.Unary expr)
        {
            object right = Evaluate(expr.Right);

            switch (expr.Op.Type)
            {
                case NEG:
                    return !IsTruthy(right);
                case MINUS:
                    CheckNumberOperand(expr.Op, right);
                    return -(double)right;
            }

            // Unreachable.
            return null;
        }

        public object Visit(Expr.Binary expr)
        {
            object left = Evaluate(expr.Left);
            object right = Evaluate(expr.Right);

            switch (expr.Op.Type)
            {
                case GREATER:
                    CheckNumberOperands(expr.Op, left, right);
                    return (double)left > (double)right;
                case GREATER_EQ:
                    CheckNumberOperands(expr.Op, left, right);
                    return (double)left >= (double)right;
                case LESS:
                    CheckNumberOperands(expr.Op, left, right);
                    return (double)left < (double)right;
                case LESS_EQ:
                    CheckNumberOperands(expr.Op, left, right);
                    return (double)left <= (double)right;

                case MINUS:
                    CheckNumberOperands(expr.Op, left, right);
                    return (double)left - (double)right;
                case PLUS:
                {
                    if (left is double && right is double)
                    {
                        return (double)left + (double)right;
                    }

                    if (left is string && right is string)
                    {
                        return (string)left + (string)right;
                    }

                    throw new RuntimeException(expr.Op,
                        "Operands must be two numbers or two strings.");
                    }
                case SLASH:
                    CheckNumberOperands(expr.Op, left, right);
                    return (double)left / (double)right;
                case STAR:
                    CheckNumberOperands(expr.Op, left, right);
                    return (double)left * (double)right;

                case NE: return !IsEqual(left, right);
                case EQ: return IsEqual(left, right);
            }

            // Unreachable.
            return null;
        }

        public object Visit(Expr.Assign expr)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Call expr)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Get expr)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Logical expr)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Set expr)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Super expr)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.This expr)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Variable expr)
        {
            throw new NotImplementedException();
        }

        private bool IsTruthy(object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;

            return true;
        }

        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;

            return a.Equals(b);
        }

        private string Stringify(object obj)
        {
            if (obj == null) return "nil";

            if (obj is double)
            {
                var text = obj.ToString();

                // Print integer without decimal point.
                if (text.EndsWith(".0"))
                    text = text.Substring(0, text.Length - 2);

                return text;
            }

            return obj.ToString();
        }

        private void CheckNumberOperands(Token op, object left, object right)
        {
            if (left is double && right is double) return;
            throw new RuntimeException(op, "Operands must be numbers.");
        }

        private void CheckNumberOperand(Token op, object operand)
        {
            if (operand is double) return;
            throw new RuntimeException(op, "Operand must be a number.");
        }

        private object Evaluate(Expr expr) => expr.Accept(this);
    }
}
