using ICICI.AppCode.Interfaces;
using System;

namespace ICICI.Models
{
    public class Response<T> : IResponse<T>
    {
        public ResponseStatus StatusCode { get; set; }
        public string ResponseText { get; set; }
        public Exception Exception { get; set; }
        public T Result { get; set; }

        public Response()
        {
            StatusCode = ResponseStatus.Failed;
            ResponseText = ResponseStatus.Failed.ToString();
        }
    }

    public class Response : IResponse
    {
        public ResponseStatus StatusCode { get; set; }
        public string ResponseText { get; set; }
        public Exception Exception { get; set; }

        public Response()
        {
            this.StatusCode = ResponseStatus.Failed;
            this.ResponseText = ResponseStatus.Failed.ToString();
        }
    }

    public class Request<T> : IRequest<T>
    {
        public string AuthToken { get; set; }
        public T Param { get; set; }
    }

    public enum ResponseStatus
    {
        Failed = -1,
        Success = 1,
        Pending = 2,
        info = 3,
        warning = 4,
    }
}
