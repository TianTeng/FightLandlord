using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BetGame.DDZ.WasmClient.Model;

namespace BetGame.DDZ.WasmClient.Services
{
    public class Utils
    {
        public static ClaimsPrincipal GetClaimsIdentity(Player player)
        {
            var identity = new ClaimsIdentity(new[]
                        {
                            new Claim(nameof(player.Balance), player.Balance.ToString()),
                            new Claim(nameof(player.Score), player.Score.ToString()),
                            new Claim(nameof(player.GameState), player.GameState??""),
                            new Claim(nameof(player.IsOnline), player.IsOnline.ToString()),
                            new Claim(nameof(player.Nick), player.Nick),
                            new Claim(nameof(player.Id), player.Id),
                        }, "Fake authentication type");

            return new ClaimsPrincipal(identity);
        }
    }
}
