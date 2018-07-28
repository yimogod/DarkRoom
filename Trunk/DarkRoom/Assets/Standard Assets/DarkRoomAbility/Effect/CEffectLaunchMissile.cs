using DarkRoom.Core;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 发射飞弹, 理论上飞弹只能对其他人或者地方使用
	/// </summary>
	public class CEffectLaunchMissile : CEffect
	{
		private CMissleEntity m_entity;

		protected CEffectLaunchMissileMeta m_meta
		{
			get { return MetaBase as CEffectLaunchMissileMeta; }
		}

		/*public override void Apply(CAIController from, CAIController to)
		{
			Apply(from, to.LocalPosition);
			m_entity.FlyTarget = to;
		}

		public override void Apply(CAIController from, Vector3 to)
		{
			if (m_meta.WhichUnit == CAbilityEnum.Location.TargetUnit) {
				Debug.LogError("CEffectLaunchMissile must attach on the launcher (which is owner too)");
				return;
			}

			base.Apply(from, to);

			//创建missle, 并且放在释放者身上的位置. 更详细的. 如果是武器, 其实应该是武器的位置
			GameObject missle = new GameObject(m_meta.Id);
			//TODO 自己的加载器
			//UIUtil.SetAbilityEffect(missle.transform, m_meta.MisslePrefab);

			Vector3 newPos = m_owner.LocalPosition + Vector3.up * m_owner.Pawn.SpacialComp.Height * 0.7f;
			CDarkUtil.AddChild(CWorldContainer.Instance.EffLayer, missle.transform, newPos);

			m_entity = missle.AddComponent<CMissleEntity>();
			m_entity.OnImpact = OnMissileImpact;

			//给导弹添加移动信息
			CMissleMovementComp mover = missle.AddComponent<CMissleMovementComp>();
			mover.Speed = m_meta.MissileSpeed * Random.Range(0.9f, 1.1f); ;
			mover.Height = 10f * Random.Range(0.9f, 1.1f);
			mover.Slerp = m_meta.Slerp;
            if (m_meta.Mover == "curve") {
				mover.Launch(CMissleMovementComp.MoveType.ParaCurve, to);
			} else {
				mover.Launch(CMissleMovementComp.MoveType.FlyTo, to);
			}

			//添加自毁程序. 3s后自动销毁
			CAutoDestoryComp des = missle.AddComponent<CAutoDestoryComp>();
			des.Run(3f);
		}*/

		//private void OnImpactCallBack (object sender, XEventArgs e){
		//	OnImpact(e.dataObj as CActorEntity, e.dataObj_2 as CActorEntity);
		//}

		//private void OnImpact(CActorEntity owner, CActorEntity target){
		//	CEffectLaunchMissleMeta meta = _baseMeta as CEffectLaunchMissleMeta;
		//
		//		CEffect effect = CEffectHelper.CreateCEffect(meta.impactEffectKey, gameObject);
		//	if(effect != null)effect.Apply(owner, target);
		//}

		private void OnMissileImpact(Vector3 hitPos)
		{
			//if (m_meta.ImpactVFX.Valid) {
		//		GameObject explode = new GameObject(m_meta.ImpactVFX.Prefab);
		//		//UIUtil.SetAbilityEffect(explode.transform, m_meta.ImpactVFX);
			//	CDarkUtil.AddChild(CWorld.Instance.Layer.UnitLayer, explode.transform, hitPos);


			//	CAutoDestoryComp des = explode.AddComponent<CAutoDestoryComp>();
			//	des.Run(3f);
		//	}
		}

		protected override void OnDestroy()
		{
			//m_entity = null;
		}
	}
}
