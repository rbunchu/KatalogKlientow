using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KatalogKlientow.Services
{
    public interface IModelValidator
    {
        List<ValidationResult> Validate(object model);
    }
}
