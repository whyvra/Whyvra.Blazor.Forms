using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Whyvra.Blazor.Forms.Elements;

namespace Whyvra.Blazor.Forms.Renderers
{
    public abstract class FormRenderer<T> : ComponentBase, IFormRenderer
    {
        [Parameter]
        public FormModel<T> FormModel { get; set; }

        [Parameter]
        public FormState FormState { get; set; }

        [Parameter]
        public Func<string, IEnumerable<string>> GetValidationMessages { get; set; } = _ => Enumerable.Empty<string>();

        public void ValidateForm()
        {
            if (GetValidationMessages == null) return;

            FormModel.Elements
                .OfType<InputElement>()
                .ToList()
                .ForEach(ValidateElement);
        }

        protected void HandleCheckbox(CheckboxElement checkbox, ChangeEventArgs e)
        {
            // Update model with value
            var isChecked = (bool) e.Value;
            checkbox.Set(FormModel.DataModel, isChecked);

            if (checkbox.ElementsToHide == null)  return;

            // Hide selected fields
            FormModel.Elements
                .Where(x => x is InputElement input && checkbox.ElementsToHide(input))
                .Cast<InputElement>()
                .ToList()
                .ForEach(x => x.IsHidden = isChecked);
        }

        protected void HandleInput(InputElement input, ChangeEventArgs e)
        {
            // Update data model
            var value = e.Value.ToString().Trim();
            if (input.Type == "number" && !string.IsNullOrWhiteSpace(value))
            {
                input.Set(FormModel.DataModel, int.Parse(value));
            }
            else
            {
                input.Set(FormModel.DataModel, value);
            }
        }

        protected void ValidateElement(InputElement input)
        {
            input.ValidationMessages = GetValidationMessages(input.ValidationPath);
        }
    }
}