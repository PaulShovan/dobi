using Dhobi.Core.PromoOffer.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface IPromoOfferRepository
    {
        Task<bool> AddPromoOffer(List<Promo> promoOffers);
        Task<bool> IsOverlappedPromoOffer(Promo promo);
        Task<Promo> GetPromoOfferForUser(long nowDate);
    }
}
