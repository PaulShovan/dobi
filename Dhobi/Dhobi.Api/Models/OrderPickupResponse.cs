using Dhobi.Core.OrderModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dhobi.Api.Models
{
    public class OrderPickupResponse
    {
        public List<string> Services;
        public OrderPickupInformationViewModel Order;
    }
}