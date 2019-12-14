using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BetGame.DDZ.WasmClient.Services
{
    public class FunctionHelper
    {
        private readonly IJSRuntime _jsRuntime;

        public FunctionHelper(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public ValueTask Alert(object message)
        {
           return _jsRuntime.InvokeVoidAsync("alert", message);
        }
    }
}
