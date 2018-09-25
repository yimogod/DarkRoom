namespace DarkRoom.Game
{
	/// <summary>
	/// 玩家手动控制角色的controller
	/// </summary>
	public class CPlayerController : CAIController
	{
		/// <summary>
		/// 是否忽略输入控制
		/// </summary>
		public bool IgnoreInput = true;

		/// <summary>
		/// 是否处于选择目标命令状态下
		/// 比如去A别人. 或者技能选择别人
		/// </summary>
		public bool InCommandToSelectTarget = false;

		protected override void Start()
		{
			base.Start();
			SetupInputComponent();
		}

		/// <summary>
		/// 对操作用户做的操作映射. 比如左键点击对应的行为
		/// </summary>
		protected virtual void SetupInputComponent()
		{
			
		}
	}
}