
using Sat.Recruitment.Api.Models;

namespace Sat.Recruitment.Api.Interfaces
{
    public interface IValidationService<T>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ValidationResult<T> Validate(T entity);
    }
}
