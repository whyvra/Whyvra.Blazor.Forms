using System.Collections.Generic;
using Whyvra.Blazor.Forms.Elements;

namespace Whyvra.Blazor.Forms
{
    public class FormModel<T>
    {
        public IEnumerable<FormElement> Elements { get; set; }

        public T DataModel { get; set; }
    }
}