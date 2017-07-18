using Dhobi.Core.OrderModel.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Core.OrderModel.ViewModels
{
    public class OrderSummaryViewModel
    {
        public string ServiceId;
        public string DobiName;
        public string Address;
        public string PickupTime;
        public double TotalWeight;
        public int TotalQuantity;
        public string Services;
        public string Detergents;
        public List<Item> Items;
        public decimal SubTotal;
        public decimal Promotion;
        public decimal Total;
        public int OrderStatus;
    }
}
