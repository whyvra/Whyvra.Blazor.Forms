using System.Collections.Generic;

namespace Whyvra.Blazor.Forms
{
    public interface IFormModel<TModel>
    {
        IEnumerable<IComponentRegistration> Components { get; set; }

        TModel DataModel { get; set; }
    }
}
