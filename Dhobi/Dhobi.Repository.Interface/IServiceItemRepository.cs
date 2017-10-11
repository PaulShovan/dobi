using Dhobi.Core.OrderModel.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface IServiceItemRepository
    {
        Task<bool> AddNewServiceItems(List<ServiceItem> items);
        Task<List<ServiceItem>> GetServiceItems();
    }
}
