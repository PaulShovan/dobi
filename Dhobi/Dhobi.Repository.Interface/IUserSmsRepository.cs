using Dhobi.Core.UserSms;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface IUserSmsRepository
    {
        Task<bool> AddUserSms(UserSms sms);
        Task<bool> ValidateUser(string code, string userId);
    }
}
