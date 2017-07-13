using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Core.OrderModel.DbModels
{
    public class OrderItem
    {
        public string OrderItemId;
        public string OrderTitle;
        public List<Item> Items;
        public double Weight;
        public string Detergent;
        public decimal TotalCost;
        public decimal Promotion;
        public string Comment;
    }
}
