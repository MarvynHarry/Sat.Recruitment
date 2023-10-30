using Microsoft.Extensions.Configuration;
using Sat.Recruitment.Api.Interfaces;
using Sat.Recruitment.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services
{
    public class UsersService : IUsersService
    {
        private readonly IValidationService<User> _ValidationService;
        private readonly List<User> _users = new();
        private readonly string _txtPath;
        public UsersService(IValidationService<User> _ValidationService, IConfiguration configuration)
        {
            this._ValidationService = _ValidationService;
            _txtPath = Path.Combine(Directory.GetCurrentDirectory(), configuration.GetValue<string>("Settings:TxtFileLocaton"));
        }

        public async Task<Result<User>> CreateUser(User newUser)
        {
            try
            {
                var ValidationResult = _ValidationService.Validate(newUser);

                if (!ValidationResult.IsSuccess)
                    return new()
                    {
                        IsSuccess = ValidationResult.IsSuccess,
                        Errors = ValidationResult.Errors,
                        ResultModel = ValidationResult.ResultModel,
                        Warmings = { },
                        SuccesMessages = { },
                    };

                switch (newUser.UserType)
                {
                    case UserTypes.Normal:
                        if (newUser.Money > 100)
                            newUser.Money += newUser.Money * Convert.ToDecimal(0.12);

                        if (newUser.Money < 100 && newUser.Money > 10)
                            newUser.Money += newUser.Money * Convert.ToDecimal(0.8);
                        break;
                    case UserTypes.SuperUser:
                        if (newUser.Money > 100)
                            newUser.Money += newUser.Money * Convert.ToDecimal(0.20);

                        break;
                    case UserTypes.Premium:
                        if (newUser.Money > 100)
                            newUser.Money += newUser.Money * 2;

                        break;
                    default:
                        throw new("UserType dont Exist");
                }

                await ReadUsersFromFile();

                newUser.Email = NormalizeEmail(newUser.Email);


                if ((_users.Any(x => x.Email.Trim().ToLower() == newUser.Email.Trim().ToLower() || x.Phone.Trim().ToLower() == newUser.Phone.Trim().ToLower()))
                    || (_users.Any(x => x.Name.Trim().ToLower() == newUser.Name.Trim().ToLower() && x.Address.Trim().ToLower() == newUser.Address.Trim().ToLower())))
                    throw new Exception("User is duplicated");
                else
                {
                    File.AppendAllText(_txtPath, Environment.NewLine + string.Format("{0},{1},{2},{3},{4},{5}", newUser.Name, newUser.Email, newUser.Phone, newUser.Address, newUser.UserType, newUser.Money));
                    await FormatTXT();
                }

                return new()
                {
                    ResultModel = newUser,
                    SuccesMessages = { "User Created" },
                    Warmings = { },
                    IsSuccess = true,
                    Errors = { }
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    ResultModel = newUser,
                    SuccesMessages = { },
                    Warmings = { },
                    IsSuccess = false,
                    Errors = { ex.Message }
                };
            }
        }

        public async Task<Result<List<User>>> Getusers()
        {
            try
            {
                await ReadUsersFromFile();
                return new()
                {
                    IsSuccess = true,
                    ResultModel = _users,
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    SuccesMessages = { },
                    Warmings = { },
                    IsSuccess = false,
                    Errors = { ex.Message }
                };
            }
        }


        #region Private functions
        private string NormalizeEmail(string Email)
        {
            try
            {
                //Normalize email
                var aux = Email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);
                aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);
                return string.Join("@", new string[] { aux[0], aux[1] });
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task FormatTXT()
        {
            var Result = await File.ReadAllLinesAsync(_txtPath);
            var lines = Result.Where(arg => !string.IsNullOrWhiteSpace(arg));
            File.WriteAllLines(_txtPath, lines);
        }

        private async Task ReadUsersFromFile()
        {
            FileStream fileStream = new(_txtPath, FileMode.Open);
            StreamReader reader = new(fileStream);

            try
            {
                _users.Clear();
                while (reader.Peek() >= 0)
                {
                    var line = await reader.ReadLineAsync();

                    if (!string.IsNullOrWhiteSpace(line))
                        _users.Add(new()
                        {
                            Name = line.Split(',')[0].ToString(),
                            Email = line.Split(',')[1].ToString(),
                            Phone = line.Split(',')[2].ToString(),
                            Address = line.Split(',')[3].ToString(),
                            UserType = line.Split(',')[4].ToString(),
                            Money = decimal.Parse(line.Split(',')[5].ToString()),
                        });
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                fileStream.Close();
                reader.Close();
            }
        }
        #endregion
    }
}
