using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Business.Interface
{
    public interface IOrderServiceBusiness
    {
        Task<bool> AddService(List<string> orderServices);
        Task<List<string>> GetOrderServices();
    }
}
