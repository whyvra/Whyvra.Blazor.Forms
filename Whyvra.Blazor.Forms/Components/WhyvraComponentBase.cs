using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Whyvra.Blazor.Forms.Validation;

namespace Whyvra.Blazor.Forms.Components
{
    public class WhyvraComponentBase<TModel> : ComponentBase
    {
        [CascadingParameter]
        public FormState FormState { get; set; }

        [Parameter]
        public string CssModifier { get; set; } = string.Empty;

        [Parameter]
        public string DisplayName { get; set; }

        [Parameter]
        public IFormModel<TModel> FormModel { get; set; }

        [Parameter]
        public string InternalName { get; set; }

        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public Func<TModel, object> GetData { get; set; }

        [Parameter]
        public Action<TModel, object> SetData { get; set; }

        [Parameter]
        public IEnumerable<string> ValidationMessages { get; set; } = Enumerable.Empty<string>();

        [Parameter]
        public string ValidationPath { get; set; }

        [Parameter]
        public Func<ValidationResult> GetValidationResult { get; set; }
    }
}
