using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Common
{
    public enum RegistrationSmsStatus
    {
        Unapproved = 0,
        Approved = 1
    }
    public enum ResponseStatus
    {
        Ok = 200,
        BadRequest = 400,
        NotFound = 404
    }
}
