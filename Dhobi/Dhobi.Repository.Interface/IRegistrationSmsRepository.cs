using Dhobi.Core.RegistrationSms;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface IRegistrationSmsRepository
    {
        Task<bool> AddRegistrationSms(RegistrationSms sms);
        Task<bool> ValidateRegisteredUser(string code, string userId);
    }
}
