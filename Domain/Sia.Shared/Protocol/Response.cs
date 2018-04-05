using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sia.Shared.Protocol
{
    public class Response<T> : Response, IResponse<T>
    {
        public T Value { get; private set; }

        internal Response(HttpStatusCode statusCode, bool isSuccessCode, string content, T value) 
            : base(statusCode, isSuccessCode, content)
        {
            this.Value = value;
        }
    }

    public class Response : IResponse
    {
        public HttpStatusCode StatusCode { get; protected set; }
        public bool IsSuccessStatusCode { get; protected set; }
        public string Content { get; protected set; }

        internal Response(HttpStatusCode statusCode, bool isSuccessCode, string content)
        {
            this.StatusCode = statusCode;
            this.IsSuccessStatusCode = isSuccessCode;
            this.Content = content;
        }
    }

    public sealed class ResponseFactory
    {
        public static async Task<Response> CreateResponse(HttpResponseMessage message)
            => new Response(
                message.StatusCode,
                message.IsSuccessStatusCode,
                await message
                    .Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(continueOnCapturedContext: false)
            );


        public static async Task<Response<T>> CreateResponse<T>(HttpResponseMessage message)
        {
            string content = string.Empty;
            T value = default(T);

            if (message.IsSuccessStatusCode)
            {
                content = await message
                    .Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(continueOnCapturedContext: false);
                value = JsonConvert.DeserializeObject<T>(content);
            }

            return new Response<T>(
                message.StatusCode,
                message.IsSuccessStatusCode,
                content, 
                value
            );
        }
    }
}
