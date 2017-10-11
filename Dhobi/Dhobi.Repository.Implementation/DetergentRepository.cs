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
    public class DetergentRepository : Repository<Detergent>, IDetergentRepository
    {
        public async Task<bool> AddNewDetergents(List<Detergent> detergents)
        {
            try
            {
                await Collection.InsertManyAsync(detergents);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding detergents" + ex);
            }
        }

        public async Task<List<Detergent>> GetDetergents()
        {
            try
            {
                var projection = Builders<Detergent>.Projection.Exclude("_id");
                var items = await Collection.Find(s => s.Id != "").Project<Detergent>(projection).ToListAsync();
                return items;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in getting detergents" + ex);
            }
        }
    }
}
