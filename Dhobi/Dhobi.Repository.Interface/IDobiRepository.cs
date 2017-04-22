using Dhobi.Core.Dobi.DbModels;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface IDobiRepository
    {
        Task<bool> AddDobi(Dobi dobi);
    }
}
