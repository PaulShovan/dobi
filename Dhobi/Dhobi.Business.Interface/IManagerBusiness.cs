﻿using Dhobi.Core;
using Dhobi.Core.Manager.DbModels;
using Dhobi.Core.Manager.ViewModels;
using System.Threading.Tasks;

namespace Dhobi.Business.Interface
{
    public interface IManagerBusiness
    {
        Task<GenericResponse<string>> AddManager(ManagerViewModel managerViewModel, ManagerBasicInformation addedBy);
        Task<bool> IsEmailAvailable(string email);
        Task<bool> IsUserNameAvailable(string userName);
        Task<ManagerBasicInformation> ManagerLogin(LoginViewModel loginModel);
    }
}
