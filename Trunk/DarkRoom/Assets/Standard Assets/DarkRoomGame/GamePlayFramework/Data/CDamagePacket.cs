namespace DarkRoom.Game {
	/// <summary>
	/// CDamageType 用来定义一系列伤害方式.并提供手段用来响应不同的伤害来源
	/// </summary>
	public abstract class CDamagePacket
	{

	}

    /// <summary>
    /// 伤害事件
    /// </summary>
    public struct CDamageEvent
    {
        /** ID for this class. NOTE this must be unique for all damage events. */
        public int ClassId;

        public bool IsOfType(int inId) { return ClassId == inId; }
    }
}