using Dhobi.Common;
using System;
using System.Web.Http;

namespace Dhobi.Api.Models
{
    public class ResponseModel<T> where T : class
    {
        public T Data;
        public string ResponseStatus;
        public string Message;
        public ResponseModel(ResponseStatus status, T responseData, string message = "")
        {
            ResponseStatus = Enum.GetName(typeof(ResponseStatus), status);
            Data = responseData;
            Message = message;
        }
    }
}