using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Core.OrderModel.ViewModels
{
    public class OrderByZoneViewModel
    {
        public string Zone;
        public List<OrderItemViewModel> Orders;
        public int Count;
    }
}
