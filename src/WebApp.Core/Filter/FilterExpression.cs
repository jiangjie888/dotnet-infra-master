using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Stbis.Util;
using Abp.Extensions;
using System.ComponentModel;

namespace Stbis.Common
{
    public class QueryableSearcher<T>
    {
        protected FilterGroup Filter { get; set; }

        public QueryableSearcher(FilterGroup filter)
        {
            Filter = filter;
        }

        public LambdaExpression True()
        {
            ConstantExpression conRight = Expression.Constant(1, typeof(int));
            BinaryExpression binaryBody = Expression.Equal(conRight, conRight);
            LambdaExpression lambda = Expression.Lambda<Func<bool>>(binaryBody);

            return lambda;
        }

        public Expression<Func<T, bool>> TrueByT()
        {
            ConstantExpression conRight = Expression.Constant(1, typeof(int));
            ParameterExpression param = Expression.Parameter(typeof(T), "c");
            BinaryExpression binaryBody = Expression.Equal(conRight, conRight);
            LambdaExpression lambda = Expression.Lambda<Func<T, bool>>(binaryBody, param);

            return lambda as Expression<Func<T, bool>>;
        }

        public Expression<Func<T, bool>> Search()
        {
            if (Filter == null) return TrueByT();

            ParameterExpression param = Expression.Parameter(typeof(T), "c");

            var body = GetExpressoininBody(param, Filter);

            if (body == null) return TrueByT();

            var expression = Expression.Lambda<Func<T, bool>>(body, param);

            return expression;

        }

        public Expression<Func<T, bool>> GetQueryExpressoin()
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "c");

            var body = GetExpressoininBody(param, Filter);
            if (body == null)
                return null;
            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        private Expression GetExpressoininBody(ParameterExpression param, FilterGroup group)
        {
            Expression expression = null;

            if (group.rules != null && group.op == "and")
            {
                foreach (var rule in group.rules)
                {
                    Expression left = GetExpressionByCondition(param, rule);
                    expression = expression == null ? left : Expression.AndAlso(left, expression);
                }
            }

            if (group.rules != null && group.op == "or")
            {
                foreach (var rule in group.rules)
                {
                    Expression left = GetExpressionByCondition(param, rule);
                    expression = expression == null ? left : Expression.OrElse(left, expression);
                }
            }
            if (group.groups != null)
            {
                Expression groupExpression = null;
                foreach (var subgroup in group.groups)
                {
                    Expression left = GetExpressoininBody(param, subgroup);

                    if (group.op == "and")
                        groupExpression = groupExpression == null ? left : Expression.AndAlso(left, groupExpression);
                    else
                        groupExpression = groupExpression == null ? left : Expression.OrElse(left, groupExpression);
                }
                if (group.op == "and")
                    expression = expression == null ? groupExpression : Expression.AndAlso(groupExpression, expression);
                else
                    expression = expression == null ? groupExpression : Expression.OrElse(groupExpression, expression);
            }
            return expression;
        }


        /// <summary>
        /// 根据规则 生成表达式树
        /// </summary>
        /// <param name="param"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private Expression GetExpressionByCondition(ParameterExpression param, FilterRule rule)
        {
            LambdaExpression exp = GetPropertyLambdaExpression(rule, param);//得到属性 如果c.name


            var constant = ChangeTypeToExpression(rule, exp.Body.Type);//得到常量 例如 "wfl"

            var selectexp = ExpressionDict[rule.op];//选择连接方法 比如是等于还是like

            Expression returnexp = selectexp(exp.Body, constant); //通过方法连接属性以及参数 得到c.name=="wlf"的表达式

            return returnexp;

        }

        private Expression ChangeTypeToExpression(FilterRule rule, Type type)
        {
            #region 数组
            if (rule.op == "in")
            {
                var arr = (rule.value as Array);
                var expList = new List<Expression>();
                if (arr != null)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        var newvalue = ChangeType(arr.GetValue(i), type);
                        expList.Add(Expression.Constant(newvalue, type));
                    }
                    return Expression.NewArrayInit(type, expList);
                }
            }
            #endregion

            switch (rule.type)
            {
                case "string":
                    if (string.IsNullOrEmpty(rule.value.As<String>()))
                        return True().Body;
                    break;
                case "date":
                    if (rule.value == null || rule.value.ToString().Length == 0)
                        return True().Body;
                    break;
                case "number":
                    if (rule.value == null || rule.value.ToString().Length == 0)
                        return True().Body;
                    break;
                default:
                    break;
            }

            
            var elementType = TypeUtil.GetUnNullableType(type);
            if (elementType == typeof(Guid)) {
                return Expression.Constant(new Guid(rule.value.ToString()), type);
            }
            var value = Convert.ChangeType(rule.value, elementType);
            return Expression.Constant(value, type);
        }


        /// <summary>
        /// 类型转换，支持非空类型与可空类型之间的转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type conversionType)
        {
            if (value == null) return null;
            if (value is string && (conversionType == typeof(Guid) || conversionType == typeof(Nullable<Guid>))) return new Guid(value as string);
            return Convert.ChangeType(value, TypeUtil.GetUnNullableType(conversionType));
        }



        private LambdaExpression GetPropertyLambdaExpression(FilterRule rule, ParameterExpression param)
        {
            if (rule == null || rule.value == null || rule.value.ToString().IsNullOrEmpty())
                return True();

            //获取每级属性如c.StuClass.ClassName
            var props = rule.field.Split('.');
            var typeOfProp = typeof(T);
            Expression exppro = param;
            for (int i = 0; i < props.Length; i++)
            {
                PropertyInfo property = typeOfProp.GetProperties().FirstOrDefault(t => t.Name.ToLower() == props[i].ToLower());
                if (property == null) return null;
                typeOfProp = property.PropertyType;
                exppro = Expression.Property(exppro, property);
            }

            var lam = Expression.Lambda(exppro, param);
            return lam;
        }

        #region SearchMethod 操作方法

        private static readonly Dictionary<string, Func<Expression, Expression, Expression>> ExpressionDict =
            new Dictionary<string, Func<Expression, Expression, Expression>>
                {
                    {
                        "equal",
                        (left, right) => { return Expression.Equal(left, right); }
                        },
                    {
                        "greater",
                        (left, right) => { return Expression.GreaterThan(left, right); }
                        },
                    {
                        "greaterorequal",
                        (left, right) => { return Expression.GreaterThanOrEqual(left, right); }
                        },
                    {
                        "less",
                        (left, right) => { return Expression.LessThan(left, right); }
                        },
                    {
                        "lessorequal",
                        (left, right) => { return Expression.LessThanOrEqual(left, right); }
                        },
                    {
                        "like",
                        (left, right) =>
                            {
                                if (left.Type != typeof (string)) return null;
                                return Expression.Call(left, typeof (string).GetMethod("Contains"), right);
                            }
                        },
                    {
                        "in",
                        (left, right) =>
                            {
                                if (!right.Type.IsArray) return null;
                                //调用Enumerable.Contains扩展方法
                                MethodCallExpression resultExp =
                                    Expression.Call(
                                        typeof (Enumerable),
                                        "Contains",
                                        new[] {left.Type},
                                        right,
                                        left);

                                return resultExp;
                            }
                        },
                    {
                        "notequal",
                        (left, right) => { return Expression.NotEqual(left, right); }
                        },
                    {
                        "startwith",
                        (left, right) =>
                            {
                                if (left.Type != typeof (string)) return null;
                                return Expression.Call(left, typeof (string).GetMethod("StartsWith", new[] {typeof (string)}), right);

                            }
                        },
                    {
                        "endwith",
                        (left, right) =>
                            {
                                if (left.Type != typeof (string)) return null;
                                return Expression.Call(left, typeof (string).GetMethod("EndsWith", new[] {typeof (string)}), right);
                            }
                        }
                };

        #endregion
    }
}
