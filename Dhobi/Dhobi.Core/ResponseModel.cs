namespace Dhobi.Core
{
    public class ResponseModel<T> where T : class
    {
        public T data;
        public bool ResponseStatus;
        public ResponseModel(bool status, T responseData)
        {
            ResponseStatus = status;
            data = responseData;
        }
    }
}
