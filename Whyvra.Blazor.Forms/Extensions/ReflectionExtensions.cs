using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Whyvra.Blazor.Forms.Extensions
{
    public static class ReflectionExtensions
    {
        public static string AddSpaces(this string name)
        {
            var builder = new StringBuilder();
            builder.Append(name[0]);

            for (var i = 1; i < name.Length; i++)
            {
                var c = name[i];
                builder.Append(char.IsUpper(c) ? $" {c}" : $"{c}");
            }

            return builder.ToString();
        }

        public static string GetPropertyName<TModel, TProperty>(this Expression<Func<TModel, TProperty>> lambda)
        {
            var member = lambda.Body as MemberExpression;
            var propertyInfo = member.Member as PropertyInfo;

            return propertyInfo.Name;
        }

        public static Func<TModel, object> ToGetter<TModel, TProperty>(this Expression<Func<TModel, TProperty>> lambda)
        {
            var expr = lambda.Compile();
            return x => expr(x);
        }

        public static Action<TModel, object> ToSetter<TModel, TProperty>(this Expression<Func<TModel, TProperty>> lambda)
        {
            var value = Expression.Parameter(typeof(TProperty));
            var body = Expression.Assign(lambda.Body, value);

            var expr = Expression.Lambda<Action<TModel, TProperty>>(body, lambda.Parameters.Single(), value);
            var setter = expr.Compile();

            var isEnum = typeof(TProperty).IsEnum;
            var underlyingType = Nullable.GetUnderlyingType(typeof(TProperty));
            if (isEnum || underlyingType?.IsEnum == true)
            {
                var sourceType = isEnum ? typeof(TProperty) : underlyingType;
                return (x, y) => setter(x, sourceType == null ? (TProperty) y : (TProperty) Enum.ToObject(sourceType, y));
            }

            return (x, y) => setter(x, (TProperty) y);
        }

        public static IEnumerable<IComponentRegistration> WhereTypeIs(this IEnumerable<IComponentRegistration> registrations, Type type)
        {
            return registrations.Where(x => type.IsAssignableFrom(x.Type));
        }
    }
}
