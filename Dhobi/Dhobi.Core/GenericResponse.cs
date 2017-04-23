using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Core
{
    public class GenericResponse<T>
    {
        public T Data;
        public bool ResponseStatus;
        public string Message;
        public GenericResponse(bool status, T responseData, string message = "")
        {
            ResponseStatus = status;
            Data = responseData;
            Message = message;
        }
    }
}
