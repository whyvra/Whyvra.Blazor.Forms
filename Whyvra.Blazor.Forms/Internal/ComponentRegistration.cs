using System;
using System.Collections.Generic;

namespace Whyvra.Blazor.Forms.Internal
{
    public class ComponentRegistration<TComponent> : IComponentRegistration
    {
        public Type Type => typeof(TComponent);

        public IDictionary<string, object> Parameters { get; set; }

        public bool IsVisible { get; set; } = true;
    }
}
