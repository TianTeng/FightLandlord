using System;
using System.Collections.Generic;
using System.Linq;

namespace BetGame.DDZ.WasmClient.Model
{
    public class GameInfo
    {
        /// <summary>
        /// 打多大（普通基数）结算：multiple * (multipleAddition + Bong)
        /// </summary>
        public decimal multiple { get; set; }
        /// <summary>
        /// 附加倍数，抢地主环节
        /// </summary>
        public decimal multipleAddition { get; set; }
        /// <summary>
        /// 设定最大附加倍数
        /// </summary>
        public decimal multipleAdditionMax { get; set; }
        /// <summary>
        /// 炸弹次数
        /// </summary>
        public decimal bong { get; set; }
        /// <summary>
        /// 游戏玩家
        /// </summary>
        public List<GamePlayer> players { get; set; }
        /// <summary>
        /// 轮到哪位玩家操作
        /// </summary>
        public int playerIndex { get; set; }
        /// <summary>
        /// 底牌
        /// </summary>
        public int[] dipai { get; set; }
        public string[] dipaiText { get; set; }
        /// <summary>
        /// 出牌历史
        /// </summary>
        public List<HandPokerInfo> chupai { get; set; }
        /// <summary>
        /// 当前游戏阶段
        /// </summary>
        public string stage { get; set; }
        public string operationTimeout { get; set; }
        public int operationTimeoutSeconds { get; set; }
    }


    public class GamePlayer
    {
        public string id { get; set; }
        /// <summary>
        /// 玩家
        /// </summary>
        //public Player player { get; set; }
        /// <summary>
        /// 玩家手上的牌
        /// </summary>
        public List<int> poker { get; set; }
        public string[] pokerText { get; set; }
        /// <summary>
        /// 玩家最初的牌
        /// </summary>
        public List<int> pokerInit { get; set; }
        /// <summary>
        /// 玩家角色
        /// </summary>
        public string role { get; set; }
        /// <summary>
        /// 计算结果
        /// </summary>
        public long score { get; set; }

        public string status { get; set; }
    }

}
