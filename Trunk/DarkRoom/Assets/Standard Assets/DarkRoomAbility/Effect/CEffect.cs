using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 根据SC编辑器的数据. 效果是技能系统真正起作用的部分. 具体的效果类型有
	///		传送, 持续性效果, 创造单位, 发射导弹物, 造成伤害,
	///		搜索区域, 移除动能, 应用Buff, 修改单位等
	/// 
	/// 我们的Effect都是别人应用的effect到我们身上, 也就是说挂载在我们身上的Effect都是被动接受的
	/// 没有主动发起的. 当然自己对自己施加Effect也是自己被自己被动了
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
        /// 本效果是否在执行中
        /// </summary>
	    private bool m_running = false;



		//该效果是挂在谁身上的, 就是主人
		protected IGameplayAbilityActor m_owner;
        //效果由谁引发的
	    protected IGameplayAbilityActor m_instigator;
        //效果作用的坐标
	    protected Vector3 m_targetLocalPos = Vector3.negativeInfinity;

        /// <summary>
        /// 效果配置信息
        /// </summary>
        public CEffectMeta MetaBase
        {
            get
            {
                if (string.IsNullOrEmpty(EffectName))
                {
                    Debug.LogError("Notice EffectName is null");
                    return null;
                }
                return CEffectMetaManager.GetMeta(EffectName);
            }
        }

        protected virtual void Awake(){}
		protected virtual void Start(){}
		protected virtual void Update(){}
		protected virtual void OnDestroy(){}

	    public void InitAbilityActorInfo(IGameplayAbilityActor actor)
	    {
	        m_owner = actor;
	    }

        /// <summary>
        /// 效果作用于某个坐标
        /// </summary>
        public virtual void ApplyToPosition(Vector3 localPosition)
        {
            m_instigator = m_owner;
            m_targetLocalPos = localPosition;
        }

		public virtual void AppliedFrom(IGameplayAbilityActor instigator)
		{
		    m_instigator = instigator;
		}

		/// <summary>
		/// 效果结束
		/// </summary>
		public virtual void JobDown()
		{
		    m_running = false;
		}

        /// <summary>
        /// 给owner创建一个Effect组件, 并挂在Effect Child Layer身上
        /// </summary>
		public static CEffect Create(string meta, IGameplayAbilityActor owner)
		{
			CEffect eff = null;
			CEffectMeta emeta = CEffectMetaManager.GetMeta(meta);
			if (owner == null) {
				Debug.LogError("We can not get GameObject which effect attach on " + meta);
				return null;
			}

		    GameObject go = owner.EffectLayer;

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
					eff = go.AddComponent<CEffectApplyBuff>();
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
	}
}