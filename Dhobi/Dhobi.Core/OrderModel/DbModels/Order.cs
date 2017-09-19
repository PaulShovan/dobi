using Dhobi.Core.Dobi.DbModels;
using Dhobi.Core.PromoOffer.ViewModels;
using Dhobi.Core.UserModel.DbModels;
using System.Collections.Generic;

namespace Dhobi.Core.OrderModel.DbModels
{
    public class Order
    {
        public string ServiceId;
        public int Status;
        public string Address;
        public double Lat;
        public double Lon;
        public string PickUpTime;
        public long PickUpDate;
        public User OrderBy;
        public DobiBasicInformation Dobi;
        public List<OrderItem> OrderItems;
        public PromoOfferBasicInformation Promotion;
        public decimal GrandTotal;
        public long OrderPlacingTime;
        public long DobiAcknowledgeTime;
        public long UserAcknowledgeTime;
        public long OrderDeliveredTime;
        public string Zone;
    }
}
