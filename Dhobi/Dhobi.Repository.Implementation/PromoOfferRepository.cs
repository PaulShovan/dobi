using Dhobi.Core.PromoOffer.DbModels;
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
    public class PromoOfferRepository : Repository<Promo>, IPromoOfferRepository
    {
        public async Task<bool> AddPromoOffer(List<Promo> promoOffers)
        {
            await Collection.InsertManyAsync(promoOffers);
            return true;
        }
        public async Task<Promo> GetPromoOfferForUser(long nowDate)
        {
            try
            {
                var builder = Builders<Promo>.Filter;
                var filter1 = builder.Lte(p => p.StartDate, nowDate) & builder.Gte(p => p.EndDate, nowDate);
                var projection = Builders<Promo>.Projection.Exclude("_id");
                var promo = await Collection.Find(filter1).Project<Promo>(projection).FirstOrDefaultAsync();
                return promo;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in getting promotion offer" + ex);
            }
        }

        public async Task<bool> IsOverlappedPromoOffer(Promo promo)
        {
            try
            {
                var builder = Builders<Promo>.Filter;
                var filter1 = builder.Gte(p => p.StartDate, promo.StartDate) & builder.Lt(p => p.StartDate, promo.EndDate);
                var filter2 = builder.Lte(p => p.EndDate, promo.EndDate) & builder.Gte(p => p.EndDate, promo.StartDate);
                var filter3 = builder.Lte(p => p.StartDate, promo.StartDate) & builder.Gte(p => p.EndDate, promo.EndDate);
                var result = await Collection.CountAsync(filter1 | filter2 | filter3);
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in getting user" + ex);
            }
        }
    }
}
