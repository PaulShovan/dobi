using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Core.OrderModel.ViewModels
{
    public class UserOrderStatusResponseModel
    {
        public List<UserOrderStatusViewModel> Orders;
        public int PageCount;
    }
}
