using System;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sia.Shared.Protocol
{
    public class Response<T> : Response, IResponse<T>
    {
        public T Value { get; private set; }

        public static new async Task<Response<T>> Create(HttpResponseMessage message)
        {
            var response = new Response<T>();
            response.IsSuccessStatusCode = message.IsSuccessStatusCode;
            response.StatusCode = message.StatusCode;
            
            if (message.Content == null) return response;
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

    public class Response : IResponse
    {
        public HttpStatusCode StatusCode { get; protected set; }
        public bool IsSuccessStatusCode { get; protected set; }
        public string Content { get; protected set; }

        public static async Task<Response> Create(HttpResponseMessage message)
        {
            try
            {
                return new Response()
                {
                    IsSuccessStatusCode = message.IsSuccessStatusCode,
                    StatusCode = message.StatusCode,
                    Content = await message.Content.ReadAsStringAsync()
                };
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    IsSuccessStatusCode = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Content = null
                };
            }
        }
           
    }
}
