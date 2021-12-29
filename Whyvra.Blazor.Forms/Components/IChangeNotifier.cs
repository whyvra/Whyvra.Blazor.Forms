using System;

namespace Whyvra.Blazor.Forms.Components
{
    public interface IChangeNotifier
    {
        Action FormHasChanged { get; set; }
    }
}
