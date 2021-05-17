using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Whyvra.Blazor.Forms.Elements;

namespace Whyvra.Blazor.Forms
{
    public class FormBuilder<T>
    {
        private readonly IDictionary<string, FormElement> _elements = new Dictionary<string, FormElement>();
        private string _current;
        private T _data;

        public FormModel<T> Build()
        {
            return new FormModel<T>
            {
                Elements = _elements.Values.ToList(),
                DataModel = _data == null ? Activator.CreateInstance<T>() : _data
            };
        }

        public FormBuilder<T> Checkbox<TProperty>(Expression<Func<T, TProperty>> lambda)
        {
            var name = lambda.GetPropertyName();
            var expressionPath = lambda.ToString();

            var checkbox = new CheckboxElement
            {
                Name = name,
                DisplayName = name.AddSpaces(),
                ValidationPath = expressionPath[(expressionPath.IndexOf('.') + 1)..],
                Get = lambda.GetGetter(),
                Set = lambda.GetSetter()
            };

            _current = name;
            _elements.Add(name, checkbox);

            return this;
        }

        public FormBuilder<T> Input<TProperty>(Expression<Func<T, TProperty>> lambda)
        {
            var name = lambda.GetPropertyName();

            var input = new InputElement();
            input.Setup(lambda);

            _current = name;
            _elements.Add(name, input);

            return this;
        }

        public FormBuilder<T> List<TProperty>(Expression<Func<T, TProperty>> lambda, IEnumerable<PickOption> opts)
        {
            var name = lambda.GetPropertyName();
            var displayName = name.AddSpaces();
            var expressionPath = lambda.ToString();

            var list = new ListElement
            {
                Name = name,
                DisplayName = displayName,
                Placeholder = displayName,
                ValidationPath = expressionPath[(expressionPath.IndexOf('.') + 1)..],
                PickOptions = opts,
                Get = lambda.GetGetter(),
                Set = lambda.GetEnumSetter()
            };

            _current = name;
            _elements.Add(name, list);

            return this;
        }

        public FormBuilder<T> Number<TProperty>(Expression<Func<T, TProperty>> lambda)
        {
            var name = lambda.GetPropertyName();

            var input = new InputElement();
            input.Setup(lambda);
            input.Type = "number";

            _current = name;
            _elements.Add(name, input);

            return this;
        }

        public FormBuilder<T> TagsInput<TProperty>(Expression<Func<T, TProperty>> lambda)
        {
            var name = lambda.GetPropertyName();

            var tagsInput = new TagsInputElement();
            tagsInput.Setup(lambda);

            _current = name;
            _elements.Add(name, tagsInput);

            return this;
        }

        public FormBuilder<T> TextArea<TProperty>(Expression<Func<T, TProperty>> lambda, int? columns = null, int? rows = null)
        {
            var name = lambda.GetPropertyName();

            var textArea = new TextAreaElement();
            textArea.Setup(lambda);
            textArea.Columns = columns;
            textArea.Rows = rows;

            _current = name;
            _elements.Add(name, textArea);

            return this;
        }

        public FormBuilder<T> HideOnCheck(Func<InputElement, bool> elementsToHide)
        {
            var elem = _elements[_current];
            if (elem is CheckboxElement checkbox)
            {
                checkbox.ElementsToHide = elementsToHide;
            }

            return this;
        }

        public FormBuilder<T> WithEmptyValue(string empty)
        {
            var elem = _elements[_current];
            if (elem is TagsInputElement tagsInput)
            {
                tagsInput.EmptyValue = empty;
            }

            return this;
        }

        public FormBuilder<T> WithIcon(string icon, string size = "")
        {
            var elem = _elements[_current];
            elem.Icon = new IconElement
            {
                Name = icon,
                Size = size
            };

            return this;
        }

        public FormBuilder<T> WithModel(T model)
        {
            _data = model;
            return this;
        }

        public FormBuilder<T> WithPlaceholder(string placeholder)
        {
            var elem = _elements[_current];
            if (elem is InputElement input)
            {
                input.Placeholder = placeholder;
            }

            return this;
        }

        public FormBuilder<T> WithText(string text)
        {
            var elem = _elements[_current];
            if (elem is InputElement input)
            {
                input.DisplayName = text;
                input.Placeholder = text;
            }

            return this;
        }
    }
}