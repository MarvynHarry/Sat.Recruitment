using Microsoft.Extensions.Configuration;
using Moq;
using Sat.Recruitment.Api.Interfaces;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Services;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Sat.Recruitment.Test.Services
{
    public class UsersServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IValidationService<User>> mockValidationService;
        private Mock<IConfiguration> mockConfiguration;

        public UsersServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockValidationService = this.mockRepository.Create<IValidationService<User>>();
            this.mockConfiguration = this.mockRepository.Create<IConfiguration>();
        }

        private UsersService CreateService()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

            IConfiguration config = builder.Build();
            return new UsersService(new ValidationService<User>(), config);
        }

        [Fact]
        public async Task CreateUser_StateUnderTest_ExpectedBehavior()
        {

            // Arrange
            var service = this.CreateService();
            User newUser = new()
            {
                Address = "Alvear y Colombres",
                Email = "us er+pol o@example.com",
                Money = 59,
                Name = "Marvyn harryson Jeanty",
                Phone = "+8496262888",
                UserType = "Normal",
            };

            // Act
            var result = await service.CreateUser(newUser);

            // Assert
            Assert.True(result.IsSuccess);


            // Act
            result = await service.CreateUser(newUser);

            // Assert
            Assert.False(result.IsSuccess);

            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Getusers_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();

            // Act
            var result = await service.Getusers();

            // Assert
            Assert.True(result.IsSuccess);
            this.mockRepository.VerifyAll();
        }
    }
}
