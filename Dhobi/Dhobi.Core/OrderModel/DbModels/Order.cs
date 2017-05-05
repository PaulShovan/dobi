using Dhobi.Core.PromoOffer.ViewModels;
using Dhobi.Core.UserModel.DbModels;
using System.Collections.Generic;

namespace Dhobi.Core.OrderModel.DbModels
{
    public class Order
    {
        public string OrderId;
        public string ServiceId;
        public int Status;
        public string Address;
        public string PickUpTime;
        public string PickUpDate;
        public User OrderBy;
        public List<OrderItem> OrderItems;
        public PromoOfferBasicInformation Promotion;
        public decimal GrandTotal;
    }
}
