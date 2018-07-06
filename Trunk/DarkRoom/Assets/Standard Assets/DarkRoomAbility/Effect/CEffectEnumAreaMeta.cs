namespace DarkRoom.GamePlayAbility {
	public class CEffectEnumAreaMeta : CEffectMeta
	{
		/// <summary>
		/// 查询目标的方法
		/// </summary>
		public enum Method
		{
			TeamAll, //如果有团队的概念的话, 那么就整个团队
			ImpactRange, //在圆/矩形里面的所有单位
			RandomInRange, //在圆/矩形里面的单个随机单位
		}

		/// <summary>
		/// 搜索的方法
		/// </summary>
		public Method SearchMethod = Method.ImpactRange;

		/// <summary>
		/// 搜索的中心位置
		/// </summary>
		public CAbilityEnum.Location ImpactLocation = CAbilityEnum.Location.TargetPoint;

		/// <summary>
		/// 查找目标时的过滤器
		/// </summary>
		public AbilityTargetTeam SearchTeamFilter;

		/// <summary>
		/// 查找范围
		/// </summary>
		public EffectArea Area = new EffectArea();

		public CEffectEnumAreaMeta(string id) : base(id) {
			Type = EffectType.EnumArea;
		}

		public struct EffectArea {
			/// <summary>
			/// 搜索范围为矩形的宽度
			/// </summary>
			public float RectangleWidth;

			/// <summary>
			/// 搜索范围为矩形的长度
			/// </summary>
			public float RectangleLength;

			/// <summary>
			/// 搜索范围半径
			/// </summary>
			public float Radius;

			/// <summary>
			/// 查找到的目标受到的效果
			/// </summary>
			public string Effect;
		}
	}
}
