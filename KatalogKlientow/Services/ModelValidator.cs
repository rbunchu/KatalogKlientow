using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KatalogKlientow.Services
{
    public class ModelValidator : IModelValidator
    {
        public List<ValidationResult> Validate(object model)
        {
            var results = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, results, true);
            return results;
        }
    }
}
