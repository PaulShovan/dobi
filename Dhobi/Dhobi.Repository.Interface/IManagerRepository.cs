using Dhobi.Core.Manager.DbModels;
using Dhobi.Core.Manager.ViewModels;
using Dhobi.Repository.Interface.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface IManagerRepository : IRepository<Manager>
    {
        Task<bool> AddManager(Manager manager);
        Task<bool> IsEmailAvailable(string email);
        Task<bool> IsUserNameAvailable(string userName);
        Task<ManagerBasicInformation> ManagerLogin(LoginViewModel loginModel);
        Task<List<Manager>> GetManager(int skip, int limit);
        Task<int> GetManagerCount();
        Task<bool> RemoveManager(string managerId);
    }
}
