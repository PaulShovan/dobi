using Dhobi.Core.OrderModel.DbModels;
using Dhobi.Repository.Implementation.Base;
using Dhobi.Repository.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Repository.Implementation
{
    public class ServiceItemRepository : Repository<ServiceItem>, IServiceItemRepository
    {
        public async Task<bool> AddNewServiceItems(List<ServiceItem> items)
        {
            try
            {
                await Collection.InsertManyAsync(items);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding service items" + ex);
            }
        }

        public async Task<List<ServiceItem>> GetServiceItems()
        {
            try
            {
                var projection = Builders<ServiceItem>.Projection.Exclude("_id");
                var items = await Collection.Find(s => s.Type != "").Project<ServiceItem>(projection).ToListAsync();
                return items;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in getting service items" + ex);
            }
        }
    }
}
