using Dhobi.Core.Dobi.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface IDobiRepository
    {
        Task<bool> AddDobi(Dobi dobi);
        Task<Dobi> UpdateDobi(Dobi dobi);
        Task<bool> IsEmailAvailable(string email);
        Task<bool> IsPhoneNumberAvailable(string phone);
        Task<bool> IsPassportNumberAvailable(string passport);
        Task<bool> IsIcNumberAvailable(string icNo);
        Task<bool> IsDrivingLicenseAvailable(string drivingLicense);
        Task<DobiBasicInformation> DobiLogin(string phone);
        Task<List<Dobi>> GetDobi(int skip, int limit);
        Task<int> GetDobiCount();
        Task<Dobi> GetDobiById(string dobiId);
    }
}
