using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Core
{
    public class GenericResponse<T> where T : class
    {
        public T Data;
        public bool ResponseStatus;
        public GenericResponse(bool status, T responseData)
        {
            ResponseStatus = status;
            Data = responseData;
        }
    }
}
