using System;

namespace Whyvra.Blazor.Forms.Elements
{
    public class CheckboxElement : InputElement
    {
        public Func<InputElement, bool> ElementsToHide { get; set; }
    }
}