using System;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Whyvra.Blazor.Forms.Components;
using Whyvra.Blazor.Forms.Extensions;
using Whyvra.Blazor.Forms.Validation;

namespace Whyvra.Blazor.Forms
{
    public abstract class WhyvraFormBase<TModel> : ComponentBase
    {
        [Parameter]
        public IFormModel<TModel> FormModel { get; set; }

        [Parameter]
        public FormState FormState { get; set; }

        [Parameter]
        public Func<TModel, ValidationResult> OnValidate { get; set; }

        protected override void OnInitialized()
        {
            Func<ValidationResult> getValidationResult = null;
            if (OnValidate != null)
            {
                getValidationResult = () => OnValidate(FormModel.DataModel);
            }

            foreach (var component in FormModel.Components.WhereTypeIs(typeof(WhyvraComponentBase<TModel>)))
            {
                // Add the instance of FormModel on each component (required for data binding)
                component.Parameters.Add(nameof(FormModel), FormModel);

                // Add function to retrieve validation result (required for individual property validation)
                component.Parameters.Add(nameof(WhyvraComponentBase<TModel>.GetValidationResult), getValidationResult);
            }

            Action action = () => StateHasChanged();
            foreach(var component in FormModel.Components.WhereTypeIs(typeof(IChangeNotifier)))
            {
                component.Parameters.Add(nameof(IChangeNotifier.FormHasChanged), action);
            }
        }

        public ValidationResult ValidateForm()
        {
            if (OnValidate == null) return null;

            var result = OnValidate(FormModel.DataModel);
            var componentsToValidate = FormModel.Components.Where(x => typeof(WhyvraComponentBase<TModel>).IsAssignableFrom(x.Type));

            foreach(var component in componentsToValidate)
            {
                var validationPath = component.Parameters[nameof(WhyvraComponentBase<TModel>.ValidationPath)] as string;
                var messages = result.ValidationMessages.ContainsKey(validationPath) ? result.ValidationMessages[validationPath] : Enumerable.Empty<string>();
                component.Parameters[nameof(WhyvraComponentBase<TModel>.ValidationMessages)] = messages;
            }

            StateHasChanged();

            return result;
        }
    }
}
