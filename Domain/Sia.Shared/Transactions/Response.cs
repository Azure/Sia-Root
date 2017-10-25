using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sia.Shared.Transactions
{
    public class Response<T> : IResponse<T>
    {
        public HttpStatusCode StatusCode { get; private set; }
        public bool IsSuccessStatusCode { get; private set; }
        public string Content { get; private set; }
        public T Value { get; private set; }
        public static async Task<Response<T>> Create(HttpResponseMessage message)
        {
            var response = new Response<T>();
            response.IsSuccessStatusCode = message.IsSuccessStatusCode;
            response.StatusCode = message.StatusCode;
            var content = await message.Content.ReadAsStringAsync();
            if (message.IsSuccessStatusCode)
            {
                response.Value = JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                response.Content = content;
            }
            return response;
        }
    }
}
