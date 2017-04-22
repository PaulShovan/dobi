using Dhobi.Core;
using Dhobi.Core.UserModel.DbModels;
using Dhobi.Core.UserModel.ViewModels;
using System.Threading.Tasks;

namespace Dhobi.Business.Interface
{
    public interface IUserBusiness
    {
        Task<User> AddUser(UserViewModel userModel);
        Task<User> ValidateRegisteredUser(string code, string userId);
        Task<User> GetUserById(string userId);
    }
}
