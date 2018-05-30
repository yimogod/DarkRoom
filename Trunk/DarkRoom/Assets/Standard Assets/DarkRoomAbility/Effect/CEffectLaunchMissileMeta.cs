namespace DarkRoom.GamePlayAbility {
	public class CEffectLaunchMissileMeta : CEffectMeta {
		/// <summary>
		/// missle的移动方式
		/// </summary>
		public string Mover;

		/// <summary>
		/// 物体是否随着运动方向旋转
		/// </summary>
		public bool Slerp = false;

		/// <summary>
		/// 飞行物的运行速度
		/// </summary>
		public float MissileSpeed;

		/// <summary>
		/// 产生的missile的prefab的名称. 上面需要挂在CMissileEntity
		/// </summary>
		public string MisslePrefab;

		/// <summary>
		/// 启动时产生的效果
		/// </summary>
		public string LaunchEffect;

		/// <summary>
		/// 碰撞产生的效果
		/// </summary>
		public string ImpactEffect;

		/// <summary>
		/// 飞行物消失时调用的效果, 可以用于清理一些效果
		/// </summary>
		public string FinishEffect;

		/// <summary>
		/// 发射导弹时
		/// 可以理解为动作播放第一帧时产生的视觉效果
		/// TODO 应该用 演算体来表示
		/// </summary>
		public CVisualEffect LaunchVFX;

		/// <summary>
		/// 命中时的视觉效果
		/// </summary>
		public CVisualEffect ImpactVFX;

		public CEffectLaunchMissileMeta(string id) : base(id) {
			Type = EffectType.LaunchMissle;
		}
	}
}
