using Dhobi.Core;
using Dhobi.Core.PromoOffer.DbModels;
using Dhobi.Core.PromoOffer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Business.Interface
{
    public interface IPromoOfferBusiness
    {
        Task<GenericResponse<string>> AddPromoOffer(List<PromoViewModel> promoOffers);
    }
}
