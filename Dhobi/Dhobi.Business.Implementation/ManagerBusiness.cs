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
        public async Task<ResponseModel<string>> AddManager(ManagerViewModel managerViewModel, ManagerBasicInformation addedBy)
        {
            try
            {
                if(!await IsEmailAvailable(managerViewModel.Email))
                {
                    return new ResponseModel<string>(false, "Email is not available.");
                }
                if (!await IsUserNameAvailable(managerViewModel.UserName))
                {
                    return new ResponseModel<string>(false, "Username is not available");
                }
                var manager = new Manager();
                manager.UserId = Guid.NewGuid().ToString();
                manager.Password = _passwordHasher.GetHashedPassword(managerViewModel.Password);
                manager.JoinDate = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                manager.Email = managerViewModel.Email;
                manager.Address = managerViewModel.Address;
                manager.Name = managerViewModel.Name;
                manager.Roles = managerViewModel.Roles;
                manager.UserName = managerViewModel.UserName;
                manager.AddedBy = addedBy;

                var response = await _managerRepository.AddManager(manager);

                return new ResponseModel<string>(response, "Manager added successfully.");
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
