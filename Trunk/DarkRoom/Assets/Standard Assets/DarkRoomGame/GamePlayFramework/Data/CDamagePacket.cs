namespace DarkRoom.Game {
	/// <summary>
	/// CDamageType 用来定义一系列伤害方式.并提供手段用来响应不同的伤害来源
	/// </summary>
	public class CDamagePacket
	{
		/// <summary>
		/// 是否由时间造成的伤害. 比如从高地掉落, 或者走进岩浆
		/// </summary>
		public bool CausedByWorld;

		/// <summary>
		/// 遭受辐射状脉冲伤害时是否当作普通脉冲. 也就是说--
		/// 是否会影响速度--第一人称因为脉冲武器产生冲量, 让角色受到阻力
		/// TODO 对于ARPG 感觉应该做到技能里面, 添加减速buff
		/// </summary>
		public bool RadialDamageVelocityChange;

		/// <summary>
		/// 脉冲伤害等级
		/// </summary>
		public float DamageImpulse;

		/// <summary>
		/// 圆形范围伤害的衰减系数
		/// Default 1.0=linear, 2.0=square of distance
		/// 0 以为着不衰减
		/// </summary>
		public float DamageFallOff;
	}

    /// <summary>
    /// 伤害事件
    /// </summary>
    public struct CDamageEvent
    {
        public CDamagePacket DamageType;

        /** ID for this class. NOTE this must be unique for all damage events. */
        public int ClassId;

        public bool IsOfType(int inId) { return ClassId == inId; }
    }
}