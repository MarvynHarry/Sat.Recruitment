using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Interfaces;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Models.Atributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase, IUsersService
    {

        private readonly IUsersService _UsersService;
        public UsersController(IUsersService _UsersService)
        {
            this._UsersService = _UsersService;
        }


        [HttpPost]
        [Route("/create-user")]
        [ValidateModel]
        public async Task<Result<User>> CreateUser(User newUser)
        {
            try
            {
                return await _UsersService.CreateUser(newUser);
            }
            catch (Exception ex)
            {
                return new ()
                {
                    ResultModel = newUser,
                    SuccesMessages = { },
                    Warmings = { },
                    IsSuccess = false,
                    Errors = { ex.Message }
                };
            }
        }


        [HttpGet]
        [Route("/get-users")]
        public async Task<Result<List<User>>> Getusers()
        {
            try
            {
                return await _UsersService.Getusers();
            }
            catch (Exception ex)
            {
                return new ()
                {
                    ResultModel = new List<User>(),
                    SuccesMessages = { },
                    Warmings = { },
                    IsSuccess = false,
                    Errors = { ex.Message }
                };
            }
        }
    }
}
