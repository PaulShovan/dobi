using Dhobi.Core.OrderModel.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface IDetergentRepository
    {
        Task<bool> AddNewDetergents(List<Detergent> detergents);
        Task<List<Detergent>> GetDetergents();
    }
}
