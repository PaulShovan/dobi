using Dhobi.Core.Manager.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dhobi.Admin.Api.Models
{
    public class ManagerListResponse
    {
        public List<Manager> ManagerList;
        public int TotalManager;
    }
}