using Dhobi.Core.OrderService.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface IOrderServiceRepository
    {
        Task<bool> AddService(List<OrderService> orderServices);
        Task<List<OrderService>> GetOrderServices();
        Task<bool> RemoveService(string orderServiceId);
    }
}
