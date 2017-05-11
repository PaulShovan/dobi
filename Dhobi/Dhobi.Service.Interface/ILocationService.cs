﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Service.Interface
{
    public interface ILocationService
    {
        string GetZoneFromAddress(double lat, double lon, string address);
    }
}
