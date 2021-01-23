using System;
using System.Collections.Generic;

namespace Whyvra.Blazor.Forms.Elements
{
    public class InputElement : FormElement
    {
        public string DisplayName { get; set; }

        public bool IsHidden { get; set; }

        public string Placeholder { get; set; }

        public string Type { get; set; } = "text";

        public IEnumerable<string> ValidationMessages { get; set; } = new List<string>();

        public string ValidationPath { get; set; }

        public Func<object, object> Get { get; set; }

        public Action<object, object> Set { get; set; }
    }
}