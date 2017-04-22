using Dhobi.Common;
using System;
using System.Web.Http;

namespace Dhobi.Api.Models
{
    public class ResponseModel<T> where T : class
    {
        public T Data;
        public string ResponseStatus;
        public ResponseModel(ResponseStatus status, T responseData)
        {
            ResponseStatus = Enum.GetName(typeof(ResponseStatus), status);
            Data = responseData;
        }
    }
}