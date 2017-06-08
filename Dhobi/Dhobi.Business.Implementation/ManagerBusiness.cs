using System;
using Dhobi.Business.Interface;
using Dhobi.Core;
using Dhobi.Core.Manager.ViewModels;
using Dhobi.Core.Manager.DbModels;
using Dhobi.Common;
using Dhobi.Repository.Interface;
using System.Threading.Tasks;

namespace Dhobi.Business.Implementation
{
    public class ManagerBusiness : IManagerBusiness
    {
        private PasswordHasher _passwordHasher;
        private IManagerRepository _managerRepository;
        public ManagerBusiness(IManagerRepository managerRepository)
        {
            _passwordHasher = new PasswordHasher();
            _managerRepository = managerRepository;

        }
        public async Task<bool> IsEmailAvailable(string email)
        {
            return await _managerRepository.IsEmailAvailable(email);
        }
        public async Task<bool> IsUserNameAvailable(string userName)
        {
            return await _managerRepository.IsUserNameAvailable(userName);
        }
        public async Task<GenericResponse<string>> AddManager(Manager manager)
        {
            try
            {
                if(!await IsEmailAvailable(manager.Email))
                {
                    return new GenericResponse<string>(false, "Email is not available.");
                }
                if (!await IsUserNameAvailable(manager.UserName))
                {
                    return new GenericResponse<string>(false, "Username is not available.");
                }
                manager.UserId = Guid.NewGuid().ToString();
                manager.Password = _passwordHasher.GetHashedPassword(manager.Password);
                manager.JoinDate = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                var response = await _managerRepository.AddManager(manager);
                if (!response)
                {
                    return new GenericResponse<string>(false, "Error adding user");
                }
                return new GenericResponse<string>(true, null,"Manager added successfully.");
            }
            catch (Exception exception)
            {
                throw new Exception("Error occured" + exception);
            }
        }
        public async Task<ManagerBasicInformation> ManagerLogin(LoginViewModel loginModel)
        {
            try
            {
                loginModel.Password = _passwordHasher.GetHashedPassword(loginModel.Password);
                return await _managerRepository.ManagerLogin(loginModel);
            }
            catch (Exception exception)
            {
                throw new Exception("Error in manager login" + exception);
            }
        }
    }
}
