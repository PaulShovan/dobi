using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Core.OrderModel.ViewModels
{
    public class OrderListItemViewModel
    {
        public string ServiceId;
        public string UserName;
        public string Address;
        public string PhoneNumber;
        public double TotalWeight;
        public int TotalQuantity;
        public string Services;
        public string Detergents;
        public decimal Total;
        public int OrderStatus;
    }
}
