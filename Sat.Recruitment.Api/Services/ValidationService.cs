using Sat.Recruitment.Api.Interfaces;
using Sat.Recruitment.Api.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sat.Recruitment.Api.Services
{
    public class ValidationService<T> : IValidationService<T>
    {
        public ValidationResult<T> Validate(T entity)
        {
            try
            {
                var context = new ValidationContext(entity, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(entity, context, validationResults, true);

                return new ValidationResult<T>
                {
                    Errors = validationResults.Select(x => x.ErrorMessage).ToList(),
                    IsSuccess = isValid,
                    ResultModel = entity,
                };
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
