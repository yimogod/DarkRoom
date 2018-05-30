using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 根据SC编辑器的数据. 效果是技能系统真正起作用的部分. 具体的效果类型有
	///		传送, 持续性效果, 创造单位, 发射导弹物, 造成伤害,
	///		搜索区域, 移除动能, 应用行为, 修改单位等
	/// 
	/// 根据UE4, Effect是个数据对象. 会修改属性和标签. 是一个专门的buff类.
	/// 这跟星际的不一样
	/// 
	/// IMPORTANT!! 目前我们约定, effect apply之后会删除自己
	/// 
	/// 记住! ceffect的which unit是谁, 那么这个effect就挂在谁身上. 也意味着owner和target是谁
	/// </summary>
	public class CEffect : MonoBehaviour
	{
		/// <summary>
		/// 效果的名称, 要跟meta名称对应
		/// </summary>
		public string EffectName;

		/// <summary>
		/// TODO 对应编辑器的锚点索引
		/// </summary>
		public int Index;

		/// <summary>
		/// 效果配置信息
		/// </summary>
		public CEffectMeta MetaBase {
			get{
				if (string.IsNullOrEmpty(EffectName)) {
					Debug.LogError("Notice EffectName is null");
					return null;
				}
				return CEffectMetaManager.GetMeta(EffectName);
			}
		}

		//该效果是挂在谁身上的, 也可以称之为目标
		//TODO 放在地上的效果再说
		protected CAIController m_owner;

		//效果来源于谁
		protected CAIController m_from;
		//效果目标是谁
		protected CAIController m_to;
		protected Vector3 m_toPos;

		protected virtual void Awake(){
			m_owner = gameObject.GetComponent<CAIController>();
		}
		protected virtual void Start(){}
		protected virtual void Update(){}
		protected virtual void OnDestroy(){}

		/// <summary>
		/// 效果作用于某个坐标
		/// </summary>
		public virtual void Apply(CAIController from, Vector3 to) {
			m_from = from;
			m_toPos = to;

			if (MetaBase.WhichUnit == CAbilityEnum.Location.CasterUnit) {
				m_owner = from;
			} else {
				m_owner = null;
			}

			//不是己方给与的效果, 那么我们就人物是伤害了
			//TODO 可能有坑
			if (m_owner != null && !m_owner.SameTeam(m_from)) {
				m_owner.Pawn.Instigator = m_from;
			}
		}

		public virtual void Apply(CAIController from, CAIController to)
		{
			m_from = from;
			m_to = to;

			//效果自己放给自己
			if (MetaBase.WhichUnit == CAbilityEnum.Location.CasterUnit) {
				m_owner = from;
			} else {
				m_owner = to;
			}

			//不是己方给与的效果, 那么我们就人物是伤害了
			//TODO 可能有坑
			if (m_owner != null && !m_owner.SameTeam(m_from)) {
				m_owner.Pawn.Instigator = m_from;
			}
		}

		/// <summary>
		/// 效果结束, 销毁自己
		/// </summary>
		public virtual void JobDown(){
			Destroy(this);
		}

		public static CEffect Create(string meta, GameObject from, GameObject to)
		{
			CEffect eff = null;
			CEffectMeta emeta = CEffectMetaManager.GetMeta(meta);
			GameObject go = emeta.DectectGameObjectOwner(from, to);
			if (go == null) {
				Debug.LogError("We can not get GameObject which effect attach on " + meta);
				return null;
			}

			switch (emeta.Type) {
				case CEffectMeta.EffectType.LaunchMissle:
					eff = go.AddComponent<CEffectLaunchMissile>();
					eff.EffectName = meta;
                    break;
				case CEffectMeta.EffectType.Damage:
					eff = go.AddComponent<CEffectDamage>();
					eff.EffectName = meta;
					break;
				case CEffectMeta.EffectType.ApplyBehaviour:
					eff = go.AddComponent<CEffectApplyBehavior>();
					eff.EffectName = meta;
					break;
				case CEffectMeta.EffectType.IssueOrder:
					eff = go.AddComponent<CEffectIssueOrder>();
					eff.EffectName = meta;
					break;
				case CEffectMeta.EffectType.EnumArea:
					eff = go.AddComponent<CEffectEnumArea>();
					eff.EffectName = meta;
					break;
				case CEffectMeta.EffectType.EffectSet:
					eff = go.AddComponent<CEffectSet>();
					eff.EffectName = meta;
					break;
			}

			return eff;
		}

		public static CEffect DefaultCreateAndApply(string meta, CAIController from, CAIController to)
		{
			CEffect eff = CEffect.Create(meta, from.gameObject, to.gameObject);
			eff.Apply(from, to);
			eff.JobDown();
			return eff;
		}

		public static CEffect DefaultCreateAndApply(string meta, CAIController from, Vector3 to) {
			CEffect eff = CEffect.Create(meta, from.gameObject, null);
			eff.Apply(from, to);
			eff.JobDown();
			return eff;
		}
	}
}