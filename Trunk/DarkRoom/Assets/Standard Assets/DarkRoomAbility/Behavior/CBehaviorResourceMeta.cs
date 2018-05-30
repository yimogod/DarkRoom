using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 给玩家添加/减少资源的行为
	/// </summary>
	public class CBehaviorResourceMeta : CBehaviorMeta
	{
		public int ResourceType;
		public int ResourceNum;

		public CBehaviorResourceMeta(string idKey) : base(idKey) {
		}
	}
}
