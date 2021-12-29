using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Whyvra.Blazor.Forms.Components;

namespace Whyvra.Blazor.Forms.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddComponentBaseProperties<TModel, TProperty>(this IDictionary<string, object> parameters, Expression<Func<TModel, TProperty>> lambda)
        {
            var name = lambda.GetPropertyName();
            var expressionPath = lambda.ToString();

            parameters.TryAdd(nameof(WhyvraComponentBase<TModel>.DisplayName), name.AddSpaces());
            parameters.TryAdd(nameof(WhyvraComponentBase<TModel>.InternalName), name);
            parameters.TryAdd(nameof(WhyvraComponentBase<TModel>.IsVisible), true);
            parameters.TryAdd(nameof(WhyvraComponentBase<TModel>.GetData), lambda.ToGetter());
            parameters.TryAdd(nameof(WhyvraComponentBase<TModel>.SetData), lambda.ToSetter());
            parameters.TryAdd(nameof(WhyvraComponentBase<TModel>.ValidationPath), expressionPath[(expressionPath.IndexOf('.') + 1)..]);
        }
    }
}
