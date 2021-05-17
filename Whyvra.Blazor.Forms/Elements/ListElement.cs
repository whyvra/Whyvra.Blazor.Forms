using System.Collections.Generic;

namespace Whyvra.Blazor.Forms.Elements
{
    public class ListElement : InputElement
    {
        public IEnumerable<PickOption> PickOptions { get; set; }
    }
}
