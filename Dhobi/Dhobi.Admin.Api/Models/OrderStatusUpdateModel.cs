using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dhobi.Admin.Api.Models
{
    public class OrderStatusUpdateModel
    {
        public List<string> Orders;
        public int UpdatedStatus;
    }
}