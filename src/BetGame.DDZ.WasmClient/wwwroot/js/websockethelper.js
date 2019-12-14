var gsocket = null;
var gsocketTimeId = null;
function newWebSocket(url, dotnetHelper)
{
    console.log('newWebSocket');
    if (gsocket) gsocket.close();
    gsocket = null;
    gsocket = new WebSocket(url);
    gsocket.onopen = function (e) {
        console.log('websocket connect');
        dotnetHelper.invokeMethodAsync('onopen')
    };
    gsocket.onclose = function (e) {
        console.log('websocket disconnect');
        dotnetHelper.invokeMethodAsync('onclose')
        gsocket = null;
        clearTimeout(gsocketTimeId);
        gsocketTimeId = setTimeout(function () {
            console.log('websocket onclose ConnectWebsocket');
            dotnetHelper.invokeMethodAsync('ConnectWebsocket');
            //_self.ConnectWebsocket.call(_self);
        }, 5000);
    };
    gsocket.onmessage = function (e) {
        try {
            console.log('websocket onmessage');
            var msg = JSON.parse(e.data);
            dotnetHelper.invokeMethodAsync('onmessage', msg);
            //_self.onmessage.call(_self, msg);
        } catch (e) {
            console.log(e);
            return;
        }
    };
    gsocket.onerror = function (e) {
        console.log('websocket error');
        gsocket = null;
        clearTimeout(gsocketTimeId);
        gsocketTimeId = setTimeout(function () {
            console.log('websocket onerror ConnectWebsocket');
            dotnetHelper.invokeMethodAsync('ConnectWebsocket');
            //_self.ConnectWebsocket.call(_self);
        }, 5000);
    };
}
