using System.Net;

namespace Zombie.Api.Mediator
{
    public class BaseResponse<T>
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode;
        public T? Value { get; set; }
    }
}
