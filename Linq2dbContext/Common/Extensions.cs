using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Expressions;

namespace Linq2dbContext.Common
{
    public static class Extensions
    {
        //[Sql.Expression("{0} BETWEEN {1} AND {2}", PreferServerSide = true)]
        //public static bool Between<T>(this T x, T low, T high) where T : IComparable<T>
        //{
        //    // x >= low && x <= high
        //    return x.CompareTo(low) >= 0 && x.CompareTo(high) <= 0;
        //}

        [Sql.Function("unaccent", ServerSideOnly = true)]
        public static string UnAccent(this string text)
        {
            return text.ToLower().NonUnicode();
        }

        [ExpressionMethod("ContainsUnAccentImpl")]
        public static bool ContainsUnAccent(this string text, string compareText)
        {
            return text.UnAccent().Contains(compareText);// this code will run if we execute the method locally
        }

        public static Expression<Func<string, string, bool>> ContainsUnAccentImpl()
        {
            // LINQ to DB will translate this expression into SQL
            // (it knows out of the box how to translate Contains()
            return (text, compareText) => text.UnAccent().Contains(compareText);
        }

        [ExpressionMethod("StartsWithUnAccentImpl")]
        public static bool StartsWithUnAccent(this string text, string compareText)
        {
            return text.UnAccent().StartsWith(compareText);// this code will run if we execute the method locally
        }

        public static Expression<Func<string, string, bool>> StartsWithUnAccentImpl()
        {
            // LINQ to DB will translate this expression into SQL
            // (it knows out of the box how to translate Contains()
            return (text, compareText) => text.UnAccent().StartsWith(compareText);
        }

        [ExpressionMethod("EndsWithUnAccentImpl")]
        public static bool EndsWithUnAccent(this string text, string compareText)
        {
            return text.UnAccent().EndsWith(compareText);// this code will run if we execute the method locally
        }

        public static Expression<Func<string, string, bool>> EndsWithUnAccentImpl()
        {
            // LINQ to DB will translate this expression into SQL
            // (it knows out of the box how to translate Contains()
            return (text, compareText) => text.UnAccent().EndsWith(compareText);
        }

        public static Expression ProcessExpression(Expression expression)
        {
            var result = expression.Transform<object>(null, static (_, e) =>
            {
                if (e.NodeType == ExpressionType.Call)
                {
                    var mtEx = (MethodCallExpression)e;
                    if (mtEx.Method == null
                        || mtEx.Method.DeclaringType != typeof(string)
                        || mtEx.Object == null
                        || mtEx.Object.NodeType != ExpressionType.MemberAccess) return e;
                    if (mtEx.Method.Name == "Contains"
                    || mtEx.Method.Name == "StartsWith"
                    || mtEx.Method.Name == "EndsWith")
                    {
                        var expV = mtEx.Arguments[0];
                        if (expV is ConstantExpression)
                        {
                            ConstantExpression arg = (ConstantExpression)expV;
                            var v = (string)arg.Value;
                            expV = Expression.Constant(v.ToLower().NonUnicode());
                        }
                        else if (expV.NodeType == ExpressionType.MemberAccess)
                        {
                            var arg = ((MemberExpression)expV).Expression;
                            if (arg is ConstantExpression)
                            {
                                var v = ((ConstantExpression)arg).Value;
                                expV = Expression.Call(typeof(StringHelper), "NonUnicode", null, expV);
                                MethodInfo miLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                expV = Expression.Call(expV, miLower);
                            }
                            else
                            {
                                expV = Expression.Call(typeof(Extensions), "UnAccent", null, expV);
                                expV = Expression.Call(typeof(Sql), "Lower", null, expV);
                            }
                        }

                        var extObj = Expression.Call(typeof(Extensions), "UnAccent", null, mtEx.Object);
                        extObj = Expression.Call(typeof(Sql), "Lower", null, extObj);
                        e = Expression.Call(extObj, mtEx.Method, expV);
                        return e;
                    }
                }
                return e;
            });

            return result;
        }
    }
}
