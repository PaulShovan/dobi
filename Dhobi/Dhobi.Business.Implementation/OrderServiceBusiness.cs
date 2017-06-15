using Dhobi.Business.Interface;
using Dhobi.Common;
using Dhobi.Core.OrderService.DbModels;
using Dhobi.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Business.Implementation
{
    public class OrderServiceBusiness : IOrderServiceBusiness
    {
        private IOrderServiceRepository _orderServiceRepository;
        public OrderServiceBusiness(IOrderServiceRepository orderServiceRepository)
        {
            _orderServiceRepository = orderServiceRepository;
        }
        public async Task<bool> AddService(List<string> orderServices)
        {
            try
            {
                var services = new List<OrderService>();
                foreach (var service in orderServices)
                {
                    services.Add(new OrderService
                    {
                        OrderServiceId = Guid.NewGuid().ToString(),
                        ServiceName = service,
                        ServiceStatus = (int)ServiceStatus.Active
                    });
                }
                return await _orderServiceRepository.AddService(services);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding order service"+ex);
            }
        }

        public async Task<List<string>> GetOrderServices()
        {
            try
            {
                var services = new List<string>();
                var orderServices = await _orderServiceRepository.GetOrderServices();
                if(orderServices == null)
                {
                    return null;
                }
                foreach (var item in orderServices)
                {
                    services.Add(item.ServiceName);
                }
                return services;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting order service"+ex);
            }
        }
    }
}
