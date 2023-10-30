using Sat.Recruitment.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Interfaces
{
    public interface IUsersService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public Task<Result<User>> CreateUser(User newUser);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<Result<List<User>>> Getusers();
    }
}
