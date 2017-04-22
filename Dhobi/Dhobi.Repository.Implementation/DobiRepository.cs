using Dhobi.Core.Dobi.DbModels;
using Dhobi.Repository.Implementation.Base;
using Dhobi.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Repository.Implementation
{
    public class DobiRepository : Repository<Dobi>, IDobiRepository
    {
        public async Task<bool> AddDobi(Dobi dobi)
        {
            await Collection.InsertOneAsync(dobi);
            return true;
        }
    }
}
