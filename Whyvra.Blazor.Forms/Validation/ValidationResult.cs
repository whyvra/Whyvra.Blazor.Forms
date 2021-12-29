using System.Collections.Generic;

namespace Whyvra.Blazor.Forms.Validation
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }

        public IDictionary<string, IEnumerable<string>> ValidationMessages { get; set; }
    }
}
