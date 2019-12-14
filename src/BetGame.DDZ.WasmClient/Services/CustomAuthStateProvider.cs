using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BetGame.DDZ.WasmClient.Model;
using Microsoft.AspNetCore.Components.Authorization;

namespace BetGame.DDZ.WasmClient.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        ApiService _apiService;
        Player _playerCache;
        public CustomAuthStateProvider(ApiService apiService)
        {
            _apiService = apiService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
               var player = _playerCache??= await _apiService.GetPlayer();

            if (player == null)
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }
            else
            {
                var user = Utils.GetClaimsIdentity(player);

                return new AuthenticationState(user);
            }

        }
        public void NotifyAuthenticationState()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public void NotifyAuthenticationState(Player player)
        {
            _playerCache = player;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
