using Dhobi.Core.OrderModel.DbModels;
using Dhobi.Core.OrderModel.ViewModels;
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
        Task<bool> AddServiceItems(List<ServiceItem> items);
        Task<bool> AddDetergents(List<string> detergents);
        Task<ServiceItemViewModel> GetServiceItems();
    }
}
