using Dhobi.Core.OrderModel.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<bool> AddNewOrder(Order order);
        Task<int> GetOrderCount();
        Task<int> GetNewOrderCountByStatus(int orderStatus);
    }
}
