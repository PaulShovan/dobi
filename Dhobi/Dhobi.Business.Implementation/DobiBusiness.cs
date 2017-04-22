using Dhobi.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dhobi.Core;
using Dhobi.Core.Dobi.DbModels;
using Dhobi.Core.Manager.DbModels;
using Dhobi.Repository.Interface;

namespace Dhobi.Business.Implementation
{
    public class DobiBusiness : IDobiBusiness
    {
        private IDobiRepository _dobiRepository;
        public DobiBusiness(IDobiRepository dobiRepository)
        {
            _dobiRepository = dobiRepository;
        }
        public async Task<GenericResponse<string>> AddDobi(Dobi dobi)
        {
            try
            {
                //TODO: Validate dobi
                var response = await _dobiRepository.AddDobi(dobi);
                if (!response)
                {
                    return new GenericResponse<string>(false, "Error adding Dobi.");
                }
                return new GenericResponse<string>(true, "Dobi added successfully.");
            }
            catch (Exception exception)
            {
                throw new Exception("Error adding dobi" + exception);
            }
        }
    }
}
