using System.Collections.Generic;

namespace Sat.Recruitment.Api.Models
{
    public class ValidationResult<T>
    {
        public bool IsSuccess { get; set; } = false;    
        public T ResultModel { get; set; } = default;
        public List<string> Errors { get; set; } = new List<string>();
    }
}
