using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Whyvra.Blazor.Forms.Components;
using Whyvra.Blazor.Forms.Extensions;
using Whyvra.Blazor.Forms.Internal;

namespace Whyvra.Blazor.Forms.Builders
{
    public abstract class FormBuilder<TModel> : IFormBuilder<TModel>
    {
        private readonly IList<IComponentRegistration> _components = new List<IComponentRegistration>();
        private IComponentRegistration _current;
        private TModel _data;

        public IComponentRegistration CurrentComponent => _current;

        public IFormModel<TModel> Build()
        {
            return new FormModel<TModel>
            {
                Components = _components,
                DataModel = _data ?? Activator.CreateInstance<TModel>()
            };
        }

        public virtual IFormBuilder<TModel> Component<TComponent, TProperty>(Expression<Func<TModel, TProperty>> lambda, IDictionary<string, object> data = null) where TComponent : IComponent
        {
            var parameters = data ?? new Dictionary<string, object>();
            if (typeof(WhyvraComponentBase<TModel>).IsAssignableFrom(typeof(TComponent)))
            {
                parameters.AddComponentBaseProperties(lambda);
            }

            var registration = new ComponentRegistration<TComponent>
            {
                Parameters = parameters
            };

            _components.Add(registration);
            _current = registration;

            return this;
        }

        public abstract IFormBuilder<TModel> Checkbox<TProperty>(Expression<Func<TModel, TProperty>> lambda);

        public abstract IFormBuilder<TModel> FileInput<TProperty>(Expression<Func<TModel, TProperty>> lambda, string accept = null) where TProperty : IFormFile;

        public abstract IFormBuilder<TModel> Input<TProperty>(Expression<Func<TModel, TProperty>> lambda);

        public abstract IFormBuilder<TModel> Number<TProperty>(Expression<Func<TModel, TProperty>> lambda);

        public abstract IFormBuilder<TModel> TagsInput<TProperty>(Expression<Func<TModel, TProperty>> lambda, string tagCss = null);

        public abstract IFormBuilder<TModel> TextArea<TProperty>(Expression<Func<TModel, TProperty>> lambda, int? columns = null, int? rows = null);

        public abstract IFormBuilder<TModel> HideOnCheck(Func<IDictionary<string, object>, bool> elementsToHide);

        public virtual IFormBuilder<TModel> WithCss(string css)
        {
            if (typeof(WhyvraComponentBase<TModel>).IsAssignableFrom(CurrentComponent.Type))
            {
                CurrentComponent.Parameters[nameof(WhyvraComponentBase<TModel>.CssModifier)] = css;
            }

            return this;
        }

        public abstract IFormBuilder<TModel> WithIcon(string icon, string iconSize = null);

        public virtual IFormBuilder<TModel> WithModel(TModel model)
        {
            _data = model;

            return this;
        }

        public abstract IFormBuilder<TModel> WithPlaceholder(string placeholder);

        public virtual IFormBuilder<TModel> WithText(string text)
        {
            if (typeof(WhyvraComponentBase<TModel>).IsAssignableFrom(CurrentComponent.Type))
            {
                CurrentComponent.Parameters[nameof(WhyvraComponentBase<TModel>.DisplayName)] = text;
            }

            return this;
        }
    }
}
