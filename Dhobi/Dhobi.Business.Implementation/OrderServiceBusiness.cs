using Dhobi.Business.Interface;
using Dhobi.Common;
using Dhobi.Core.OrderService.DbModels;
using Dhobi.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dhobi.Core.OrderModel.DbModels;
using Dhobi.Core.OrderModel.ViewModels;

namespace Dhobi.Business.Implementation
{
    public class OrderServiceBusiness : IOrderServiceBusiness
    {
        private IOrderServiceRepository _orderServiceRepository;
        private IServiceItemRepository _serviceItemRepository;
        private IDetergentRepository _detergentRepository;
        public OrderServiceBusiness(IOrderServiceRepository orderServiceRepository,
            IServiceItemRepository serviceItemRepository,
            IDetergentRepository detergentRepository)
        {
            _orderServiceRepository = orderServiceRepository;
            _serviceItemRepository = serviceItemRepository;
            _detergentRepository = detergentRepository;
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

        public async Task<bool> AddServiceItems(List<ServiceItem> items)
        {
            try
            {
                return await _serviceItemRepository.AddNewServiceItems(items);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddDetergents(List<string> detergents)
        {
            try
            {
                var items = new List<Detergent>();
                foreach (var item in detergents)
                {
                    items.Add(new Detergent
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = item
                    });
                }
                return await _detergentRepository.AddNewDetergents(items);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ServiceItemViewModel> GetServiceItems()
        {
            try
            {
                var serviceItems = await _serviceItemRepository.GetServiceItems();
                var detergents = await _detergentRepository.GetDetergents();
                if(serviceItems == null && detergents == null)
                {
                    return new ServiceItemViewModel();
                }
                return new ServiceItemViewModel
                {
                    Cloths = serviceItems,
                    Detergents = detergents.Select(i => i.Name).ToList()
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
