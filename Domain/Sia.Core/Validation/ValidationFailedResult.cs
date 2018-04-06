using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Sia.Core.Validation
{
    public class ValidationFailedResult : ObjectResult
    {
        const int BadRequestCode = 400;
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(new SerializableError(modelState))
        {
            if (modelState == null) throw new ArgumentNullException(nameof(modelState));
            StatusCode = BadRequestCode;
        }
    }
}
