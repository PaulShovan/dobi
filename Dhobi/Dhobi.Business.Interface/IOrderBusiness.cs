using Dhobi.Core.OrderModel.DbModels;
using Dhobi.Core.OrderModel.ViewModels;
using Dhobi.Core.UserModel.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Business.Interface
{
    public interface IOrderBusiness
    {
        Task<bool> AddNewOrder(NewOrderViewModel order, User orderedBy, string zone);
    }
}
