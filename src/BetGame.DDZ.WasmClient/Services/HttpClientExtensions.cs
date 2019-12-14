using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BetGame.DDZ.WasmClient.Model;

namespace BetGame.DDZ.WasmClient.Services
{
    public static class HttpClientExtensions
    {
        static JsonSerializerOptions settings = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public static async Task<T> PostFormAsync<T>(this HttpClient httpClient, string requestUri, string content)
        {
            HttpContent httpcontent = new StringContent(content ?? "", Encoding.UTF8);
            httpcontent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ClientDefaults.formcontentType);

            using HttpResponseMessage responseMessage = await httpClient.PostAsync(requestUri, httpcontent);

            return await JsonSerializer.DeserializeAsync<T>(await responseMessage.Content.ReadAsStreamAsync(), settings);
        }
        public static async Task PostFormAsync(this HttpClient httpClient, string requestUri, string content)
        {
            HttpContent httpcontent = new StringContent(content ?? "", Encoding.UTF8);
            httpcontent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ClientDefaults.formcontentType);

            await httpClient.PostAsync(requestUri, httpcontent);
        }
    }
}
