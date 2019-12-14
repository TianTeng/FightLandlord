using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BetGame.DDZ.WasmClient.Services
{
    public class LocalStorage
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly static JsonSerializerOptions SerializerOptions = new JsonSerializerOptions();

        public LocalStorage(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
        public ValueTask SetAsync(string key, object value)
        {

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot be null or empty", nameof(key));
            }

            var json = JsonSerializer.Serialize(value, options: SerializerOptions);

            return _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
        }
        public async ValueTask<T> GetAsync<T>(string key)
        {

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot be null or empty", nameof(key));
            }


            var json =await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            if (json == null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(json, options: SerializerOptions);
        }
        public ValueTask DeleteAsync(string key)
        {
            return _jsRuntime.InvokeVoidAsync(
                $"localStorage.removeItem",key);
        }
    }
}
