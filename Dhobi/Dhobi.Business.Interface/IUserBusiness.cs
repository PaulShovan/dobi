using Dhobi.Common;
using Dhobi.Core;
using Dhobi.Core.UserInbox.DbModels;
using Dhobi.Core.UserModel.DbModels;
using Dhobi.Core.UserModel.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dhobi.Business.Interface
{
    public interface IUserBusiness
    {
        Task<User> AddUser(UserViewModel userModel);
        Task<User> ValidateRegisteredUser(string code, string userId);
        Task<User> GetUserById(string userId);
        Task<User> UserLogin(string phone, bool isVerificationRequired);
        Task<bool> SendUserSms(User user, SmsType smsType);
    }
}
