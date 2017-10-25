using System.Net;

namespace Sia.Shared.Transactions
{
    public interface IResponse<T>
    {
        HttpStatusCode StatusCode { get; }
        bool IsSuccessStatusCode { get; }
        string Content { get; }
        T Value { get; }
    }
}
