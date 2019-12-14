using System;

namespace BetGame.DDZ.WasmClient.Model
{
	public class HandPokerInfo {
		/// <summary>
		/// 出牌时间
		/// </summary>
		public DateTime time { get; set; }
		/// <summary>
		/// 这手牌出自哪位玩家
		/// </summary>
		public int playerIndex { get; set; }
		/// <summary>
		/// 牌编译结果
		/// </summary>
		public HandPokerComplieResult result { get; set; }
	}

	public class HandPokerComplieResult {
		public string type { get; set; }
		/// <summary>
		/// 相同类型比较大小
		/// </summary>
		public int compareValue { get; set; }
		/// <summary>
		/// 牌
		/// </summary>
		public int[] value { get; set; }
		/// <summary>
		/// 牌面字符串
		/// </summary>
		public string[] text { get; set; }
	}
}
