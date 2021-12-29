using System.Collections.Generic;

namespace Whyvra.Blazor.Forms.Internal
{
    public class FormModel<TModel> : IFormModel<TModel>
    {
        public IEnumerable<IComponentRegistration> Components { get; set; }
        public TModel DataModel { get; set; }
    }
}
