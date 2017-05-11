using Dhobi.Core;
using Dhobi.Core.Dobi.DbModels;
using Dhobi.Core.Dobi.ViewModels;
using Dhobi.Core.Manager.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Business.Interface
{
    public interface IDobiBusiness
    {
        Task<GenericResponse<string>> AddDobi(Dobi dobi);
        Task<GenericResponse<string>> UpdateDobi(Dobi dobi);
        Task<string> GenerateDobiId();
        Task<DobiHomePageResponse> GetDobiHomePageResponse(string dobiId);
    }
}
