using System.Net;

namespace Sia.Shared.Protocol
{
    public interface IResponse<T> : IResponse
    {
        T Value { get; }

    }

    public interface IResponse
    {
        HttpStatusCode StatusCode { get; }
        bool IsSuccessStatusCode { get; }
        string Content { get; }
    }
}
