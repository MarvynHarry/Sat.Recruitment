using Microsoft.Extensions.Configuration;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Services;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Sat.Recruitment.Test.Controllers
{
    public class UsersControllerTests
    {


        private UsersController CreateUsersController()
        {

            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddEnvironmentVariables();

            IConfiguration config = builder.Build();
            var userservice = new UsersService(new ValidationService<User>(), config);
            return new UsersController(userservice);
        }

        [Fact]
        public async Task CreateUser_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var usersController = this.CreateUsersController();
            User newUser = null;


            var allusers = await usersController.Getusers();

            // Act
            var result = await usersController.CreateUser(
                newUser);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(allusers.ResultModel.Count, (await usersController.Getusers()).ResultModel.Count);

            newUser = new()
            {
                Address = "Alvear y Colombres",
                Email = "user+polo@example.com",
                Money = 59,
                Name = "Marvyn harryson Jeanty",
                Phone = "+8496262888",
                UserType = "Normal",
            };


            result = await usersController.CreateUser(
                newUser);


            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(allusers.ResultModel.Count, (await usersController.Getusers()).ResultModel.Count);
        }

        [Fact]
        public async Task Getusers_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var usersController = this.CreateUsersController();

            // Act
            var result = await usersController.Getusers();

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
