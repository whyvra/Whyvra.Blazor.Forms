using System;
using System.Collections.Generic;

namespace Whyvra.Blazor.Forms
{
    public interface IComponentRegistration
    {
        Type Type { get; }

        IDictionary<string, object> Parameters { get; }

        bool IsVisible { get; set; }
    }
}
