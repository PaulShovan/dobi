using Dhobi.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dhobi.Core;
using Dhobi.Core.PromoOffer.DbModels;
using Itenso.TimePeriod;
using Dhobi.Repository.Interface;
using Dhobi.Core.PromoOffer.ViewModels;
using Dhobi.Common;

namespace Dhobi.Business.Implementation
{
    public class PromoOfferBusiness : IPromoOfferBusiness
    {
        private IPromoOfferRepository _promoOfferRepository;
        public PromoOfferBusiness(IPromoOfferRepository promoOfferRepository)
        {
            _promoOfferRepository = promoOfferRepository;
        }
        private bool IsDateOverlapped(long BS, long BE, long TS, long TE)
        {
            return (
                    // 1. Case:
                    //
                    //       TS-------TE
                    //    BS------BE 
                    //
                    // TS is after BS but before BE
                    (TS >= BS && TS <= BE)
                    || // or

                    // 2. Case
                    //
                    //    TS-------TE
                    //        BS---------BE
                    //
                    // TE is before BE but after BS
                    (TE <= BE && TE >= BS)
                    || // or

                    // 3. Case
                    //
                    //  TS----------TE
                    //     BS----BE
                    //
                    // TS is before BS and TE is after BE
                    (TS <= BS && TE >= BE));
        }
        private bool IsValidOfferPeriod(long nowDate, long promoStart, long promoEnd)
        {
            if(promoStart < nowDate || promoEnd < nowDate || promoStart > promoEnd)
            {
                return false;
            }
            return true;
        }
        public async Task<GenericResponse<string>> AddPromoOffer(List<PromoViewModel> promoOffers)
        {
            try
            {
                List<Promo> promos = new List<Promo>();
                foreach (var item in promoOffers)
                {
                    promos.Add(new Promo
                    {
                        PromoId = Guid.NewGuid().ToString(),
                        Text = item.Text,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        Status = (int)PromoStatus.Active,
                        Amount = item.Amount
                    });
                }
                foreach (var promo in promos)
                {
                    var excludedPromos = promos.FindAll(p => p.PromoId != promo.PromoId);
                    var nowDate = Utilities.GetPresentDate();
                    if (!IsValidOfferPeriod(nowDate, promo.StartDate, promo.EndDate))
                    {
                        return new GenericResponse<string>(false, null, "Invalid promo date");
                    }
                    foreach (var singlePromo in excludedPromos)
                    {
                        if (IsDateOverlapped(promo.StartDate, promo.EndDate, singlePromo.StartDate, singlePromo.EndDate))
                        {
                            return new GenericResponse<string>(false, null, "Date overlapped");
                        }
                    }
                    if (await _promoOfferRepository.IsOverlappedPromoOffer(promo))
                    {
                        return new GenericResponse<string>(false, null, "Date overlapped with existing promo offer.");
                    }
                }
                var response = await _promoOfferRepository.AddPromoOffer(promos);
                if (!response)
                {
                    return new GenericResponse<string>(false, null, "Error adding promo offer.");
                }
                return new GenericResponse<string>(true, null, "Promo offers added successfully.");
            }
            catch (Exception exception)
            {
                throw new Exception("Error in adding promo" + exception);
            }
        }

        public async Task<Promo> GetPromoOfferForUser()
        {
            try
            {
                var nowDate = Utilities.GetPresentDate();
                return await _promoOfferRepository.GetPromoOfferForUser(nowDate);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in getting promo offer." + ex);
            }
        }
    }
}
