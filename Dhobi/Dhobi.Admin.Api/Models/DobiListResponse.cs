using Dhobi.Core.Dobi.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dhobi.Admin.Api.Models
{
    public class DobiListResponse
    {
        public List<Dobi> DobiList;
        public int TotalDobi;
    }
}