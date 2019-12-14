using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BetGame.DDZ.WasmClient.Model
{
    /// <summary>
    /// 玩家
    /// </summary>
    public class Player
    {
        public string Id { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nick { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public long Score { get; set; }
        public int Balance { get; set; }
        public bool IsOnline { get; set; }
        public string GameState { get; set; }
    }

    /// <summary>
    /// 桌位
    /// </summary>
    public class Desk
    {
        public int Id { get; set; }
        public int Sort { get; set; }
        public string Title { get; set; }
        public Player player1 { get; set; }
        public Player player2 { get; set; }
        public Player player3 { get; set; }
    }
    public class currentChannel
    {
        public string chan { get; set; }
        public List<JsonElement> msgs { get; set; }
    }
    public class inputChannelMsg
    {
        public string type { get; set; }
        public string content { get; set; }
    }
}
