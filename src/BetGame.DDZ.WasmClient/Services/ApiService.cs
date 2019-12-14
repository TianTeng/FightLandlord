using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Tasks;
using BetGame.DDZ.WasmClient.Model;
using Microsoft.AspNetCore.Components;

namespace BetGame.DDZ.WasmClient.Services
{
    public class ApiService
    {
        LocalStorage _localStorage;
        HttpClient _httpclient;
        FunctionHelper _functionHelper;
        //ClientWebSocket _clientWebSocket;
        public ApiService(HttpClient httpclient, LocalStorage localStorage, FunctionHelper functionHelper/*, ClientWebSocket clientWebSocket*/)
        {
            if(!string.IsNullOrWhiteSpace(ClientDefaults.ApiHost))
            httpclient.BaseAddress = new Uri(ClientDefaults.ApiHost);
           // httpclient.DefaultRequestHeaders.Add("content-type", ClientDefaults.formcontentType);
            _httpclient = httpclient;
            _localStorage = localStorage;
            _functionHelper = functionHelper;
            //_clientWebSocket = clientWebSocket;
        }
        private string playerId;
        public async Task<Player> GetPlayer()
        {
            playerId = await _localStorage.GetAsync<string>(ClientDefaults.playerId);
            if (string.IsNullOrWhiteSpace(playerId) || playerId == "undefined")
            {
                return default;
            }
            else
            {
                var apireturn = await _httpclient.PostFormAsync<APIReturn>("/ddz/GetPlayer", $"playerId={playerId}");
                if (apireturn != null && !apireturn.Success)
                {
                    await _functionHelper.Alert(apireturn.Message);
                    return default;
                }
                if (apireturn != null && apireturn.Data != null && apireturn.Data.Any() && apireturn.Data.TryGetValue("player", out var playerstr))
                {
                    var player = JsonSerializer.Deserialize<Player>(playerstr.ToString());
                    if (player != null && !string.IsNullOrWhiteSpace(player.Nick))
                    {
                        playerId = player.Id;
                        return player;
                    }
                }
            }
            return default;
        }
        public async Task<Player> GetOrAddPlayer(string nick)
        {
            
            var apireturn = await _httpclient.PostFormAsync<APIReturn>("/ddz/GetOrAddPlayer", $"nick={nick}");
            if (apireturn != null && !apireturn.Success)
            {
                await _functionHelper.Alert(apireturn.Message);
                return default;
            }
            if (apireturn != null && apireturn.Data != null && apireturn.Data.Any() && apireturn.Data.TryGetValue("player", out object playerstr))
            {
                Console.WriteLine(playerstr.ToString());
                var player = JsonSerializer.Deserialize<Player>(playerstr.ToString());
                await _localStorage.SetAsync(ClientDefaults.playerId, player.Id);
                this.playerId = player.Id;
                return player;
            }
            return default;
        }
        public async Task<List<Desk>> GetDesks()
        {
            var apireturn = await _httpclient.PostFormAsync<APIReturn>("/ddz/GetDesks", $"playerId={playerId}" );
            if (apireturn != null && !apireturn.Success)
            {
                await _functionHelper.Alert(apireturn.Message);
                return default;
            }
            if (apireturn.Data.TryGetValue("desks", out var playerstr))
            {
                return JsonSerializer.Deserialize<List<Desk>>(playerstr.ToString());
            }
            return default;

        }
        public async Task<string> ConnectWebsocket()
        {
            var apireturn = await _httpclient.PostFormAsync<APIReturn>("/ddz/PrevConnectWebsocket", $"playerId={playerId}");
            if (apireturn != null && !apireturn.Success)
            {
                await _functionHelper.Alert(apireturn.Message);
                return default;
            }
            if (apireturn != null && apireturn.Data != null && apireturn.Data.Any() && apireturn.Data.TryGetValue("server", out var server))
            {
                return server.ToString();
            }
            return default;
        }

        public async Task Sitdown(int deskId, int pos)
        {
            var apireturn = await _httpclient.PostFormAsync<APIReturn>("/ddz/Sitdown", $"playerId={playerId}&deskId={deskId}&pos={pos}");
            if (apireturn != null && !apireturn.Success)
            {
                await _functionHelper.Alert(apireturn.Message);
            }
        }

        public async Task Standup()
        {
            var apireturn = await _httpclient.PostFormAsync<APIReturn>("/ddz/Standup", $"playerId={playerId}");
            if (apireturn != null && !apireturn.Success)
            {
                await _functionHelper.Alert(apireturn.Message);
            }
            else await _functionHelper.Alert("你已离开坐位");

        }
        public async Task SendChannelMsg(inputChannelMsg msg, string sender, string senderNick, string chan)
        {
            var data = new { type = "chanmsg", sender, senderNick, chan, time = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds, msg };
            await _httpclient.PostFormAsync("/ddz/SendChannelMsg", $"playerId={sender}&channel={chan}&message={JsonSerializer.Serialize(data) }");
        }
        public async Task SelectLandlord(string ddzid, string playerId, decimal multiple)
        {
            var apireturn = await _httpclient.PostFormAsync<APIReturn>("/ddz/SelectLandlord", $"id={ddzid}&playerId={playerId}&multiple={multiple}");
            if (apireturn != null && !apireturn.Success)
            {
                await _functionHelper.Alert(apireturn.Message);
            }
        }
        public async Task SelectFarmer(string ddzid, string playerId)
        {
            var apireturn = await _httpclient.PostFormAsync<APIReturn>("/ddz/SelectFarmer", $"id={ddzid}&playerId={playerId}");
            if (apireturn != null && !apireturn.Success)
            {
                await _functionHelper.Alert(apireturn.Message);
            }
        }

        public async Task Play(string ddzid, string playerId, IEnumerable<int> poker)
        {
            if (poker.Count() == 0)
            { 
                await _functionHelper.Alert("请选择要出的牌"); 
            }
            else
            {
                string pokers = "";
                foreach (var p in poker)
                {
                    pokers += $"&poker={p}";
                }
                var apireturn = await _httpclient.PostFormAsync<APIReturn>("/ddz/Play", $"id={ddzid}&playerId={playerId}{pokers}");
                if (apireturn != null && !apireturn.Success)
                {
                    await _functionHelper.Alert(apireturn.Message);
                }
            }
        }

        public async Task Pass(string ddzid, string playerId)
        {
            var apireturn = await _httpclient.PostFormAsync<APIReturn>("/ddz/Pass", $"id={ddzid}&playerId={playerId}");
            if (apireturn != null && !apireturn.Success)
            {
                await _functionHelper.Alert(apireturn.Message);
            }
        }

        public async Task<List<int[]>> PlayTips(string ddzid, string playerId)
        {
            var apireturn = await _httpclient.PostFormAsync<APIReturn>("/ddz/PlayTips", $"id={ddzid}&playerId={playerId}");
            if (apireturn != null && !apireturn.Success)
            {
                await _functionHelper.Alert(apireturn.Message);
            }
            if (apireturn != null && apireturn.Data != null && apireturn.Data.Any() && apireturn.Data.TryGetValue("tips", out var server))
            {
                return JsonSerializer.Deserialize<List<int[]>>(server.ToString());
            }
            return default;
        }

        public async Task CancelAutoPlay(string ddzid, string playerId)
        {
            var apireturn = await _httpclient.PostFormAsync<APIReturn>("/ddz/CancelAutoPlay", $"id={ddzid}&playerId={playerId}");
            if (apireturn != null && !apireturn.Success)
            {
                await _functionHelper.Alert(apireturn.Message);
            }
        }

    }
}
