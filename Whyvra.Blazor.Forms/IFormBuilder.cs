using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace Whyvra.Blazor.Forms
{
    public interface IFormBuilder<TModel>
    {
        IComponentRegistration CurrentComponent { get; }

        IFormModel<TModel> Build();

        IFormBuilder<TModel> Component<TComponent, TProperty>(Expression<Func<TModel, TProperty>> lambda, IDictionary<string, object> data = null) where TComponent : IComponent;

        IFormBuilder<TModel> Checkbox<TProperty>(Expression<Func<TModel, TProperty>> lambda);

        IFormBuilder<TModel> FileInput<TProperty>(Expression<Func<TModel, TProperty>> lambda, string accept = null) where TProperty : IFormFile;

        IFormBuilder<TModel> Input<TProperty>(Expression<Func<TModel, TProperty>> lambda);

        IFormBuilder<TModel> Number<TProperty>(Expression<Func<TModel, TProperty>> lambda);

        IFormBuilder<TModel> TagsInput<TProperty>(Expression<Func<TModel, TProperty>> lambda, string tagCss = null);

        IFormBuilder<TModel> TextArea<TProperty>(Expression<Func<TModel, TProperty>> lambda, int? columns = null, int? rows = null);

        IFormBuilder<TModel> HideOnCheck(Func<IDictionary<string, object>, bool> elementsToHide);

        IFormBuilder<TModel> WithCss(string css);

        IFormBuilder<TModel> WithIcon(string icon, string iconSize = null);

        IFormBuilder<TModel> WithModel(TModel model);

        IFormBuilder<TModel> WithPlaceholder(string placeholder);

        IFormBuilder<TModel> WithText(string text);
    }
}
