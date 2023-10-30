using System.Collections.Generic;

namespace Sat.Recruitment.Api.Models
{
    public class Result<T>: ValidationResult<T>
    {
        public List<string> Warmings { get; set; } = new List<string>();
        public List<string> SuccesMessages { get; set; } = new List<string>();
    }
}
