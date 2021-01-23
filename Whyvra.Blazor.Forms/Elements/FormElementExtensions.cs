using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Whyvra.Blazor.Forms.Elements
{
    public static class FormElementExtensions
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

        public static string GetPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> lambda)
        {
            var member = lambda.Body as MemberExpression;
            var propertyInfo = member.Member as PropertyInfo;

            return propertyInfo.Name;
        }

        public static void Setup<T, TProperty, TElement>(this TElement elem, Expression<Func<T, TProperty>> lambda)
            where TElement : InputElement
        {
            var name = lambda.GetPropertyName();
            var displayName = AddSpaces(name);

            var expressionPath = lambda.ToString();

            elem.Name = name;
            elem.DisplayName = displayName;
            elem.Placeholder = displayName;
            elem.ValidationPath = expressionPath[(expressionPath.IndexOf('.') + 1)..];
            elem.Get = lambda.GetGetter();
            elem.Set = lambda.GetSetter();
        }
    }
}