using Dhobi.Core;
using Dhobi.Core.UserModel.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface IUserRepository
    {
        Task<bool> AddUser(User user);
        Task<bool> IsPhoneNumberAvailable(string phoneNumber);
        Task<User> GetUserById(string userId);
        Task<User> UpdateUserAsVerified(string userId);
        Task<User> UserLogin(string phone);
    }
}
