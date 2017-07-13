using Dhobi.Core.OrderModel.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dhobi.Api.Models
{
    public class SetOrderPickUpViewModel
    {
        public string ServiceId;
        public OrderItem OrderItems;
    }
}