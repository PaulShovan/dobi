using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Core.PromoOffer.DbModels
{
    public class Promo
    {
        public string PromoId;
        public string Text;
        public long StartDate;
        public long EndDate;
        public int Status;
        public decimal Amount;
    }
}
