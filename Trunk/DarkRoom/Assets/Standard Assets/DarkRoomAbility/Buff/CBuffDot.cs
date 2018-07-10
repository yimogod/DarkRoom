using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Core;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 这个buff放在谁身上, 是由effect里面配置的
	/// </summary>
	public class CBuffDot : CBuff{
		private static int BuffCounter = 0;

		private int m_cid = 0;

		//buff生命周期的计数器
		private CTimeRegulator m_durationReg;

		//dot数据的计数器
		private CTimeRegulator m_dotReg;

		private CBuffDotMeta MBuffMeta {
			get { return MetaBase as CBuffDotMeta; }
		}

		protected override void Awake()
		{
			base.Awake();
			BuffCounter++;

			m_cid = BuffCounter;
		}

		protected override void Update(){
			base.Update();

			//buff的持续周期
			if (MBuffMeta.Duration > 0) {
				bool b = m_durationReg.Update();
				if (b)OnFinal();
			}


			if (MBuffMeta.Period > 0) {
				bool b = m_dotReg.Update();
				if (b) OnPeriod();
			}else if (CMathUtil.IsZero(MBuffMeta.Period)) {
				OnPeriod();
			}
		}

		public override void Apply(CAIController from, CAIController to) {
			//Debug.Log("buff apply to target");
			base.Apply(from, to);

			//不是己方给与的效果, 那么我们就人物是伤害了
			//TODO 可能有坑
			if (m_owner != null && !m_owner.SameTeam(m_from)) {
				m_owner.Pawn.Instigator = m_from;
			}

			if (MBuffMeta.Duration > 0) {
				m_durationReg = new CTimeRegulator(MBuffMeta.Duration);
				//Debug.Log("create duration time is " + m_meta.Duration);
            }

			if (MBuffMeta.Period > 0) {
				m_dotReg = new CTimeRegulator(MBuffMeta.Period);
				OnPeriod();
			}

			ApplyModification();
			ApplyState();
		}

		//添加属性修改

		private void ApplyModification(){
			/*var target = m_owner.BaseData.PropertyModifier;
			if (!target.ContainsKey(m_cid)) {
				target[m_cid] = new Dictionary<CPawnVO.Property, float>();
			}

			var m = MBuffMeta.ModifyProperty;

			//速度增量
			if (m.MoveSpeedBonus > 0) {
				target[m_cid].Add(CPawnVO.Property.SpeedBonus, m.MoveSpeedBonus);
				m_owner.BaseData.NotiSpeedChange();
			}

			//速度系数
			if (!CMathUtil.IsOne(m.MoveSpeedMultiplier)) {
				//Debug.Log("add speed multiplier to me " + m.MoveSpeedMultiplier);
				target[m_cid].Add(CPawnVO.Property.SpeedMultiplier, m.MoveSpeedMultiplier);
				m_owner.BaseData.NotiSpeedChange();
			}*/
		}

		//移除属性修改
		private void RemoveModification()
		{
			/*var target = m_owner.BaseData.PropertyModifier;
			if (target.ContainsKey(m_cid)) {
				target.Remove(m_cid);

				m_owner.BaseData.NotiSpeedChange();
			}*/
		}

		//添加状态修改
		//目前会有潜在问题. 比如同一个技能对一个对象多次产生buff
		//比如在冲锋的过程中, period effect会不断产生同一类buff,
		//这是恰巧应用于同一个目标单位, 那么属性和状态就会被累加
		//所以 TODO 如果buff的来源是同一个技能的话, 那我们就不接着apply了
		private void ApplyState(){
            //if (MBuffMeta.StateFlags.Count == 0)return;

		    /*var target = m_owner.BaseData.StateModifier;
            foreach (var kv in MBuffMeta.StateFlags) {
                //只要是指令移除, 那么就相当于驱散, 不管来源
                if (kv.Value == 0 &&
                    target.ContainsKey(kv.Key)) {
                    target.Remove(kv.Key);
                    continue;
                }

                if (kv.Value != 0) {
                    //计算时间, 给与更长的时间
                    long newTime = CTimeUtil.GetCurrentMillSecondStamp() +
                        (long)(MBuffMeta.Duration * 1000f);

                    //如果有其他人给的状态, 那么更新他的值
                    if (target.ContainsKey(kv.Key)) {
                        if (newTime > target[kv.Key].Value) {
                            target[kv.Key] = new KeyValuePair<int, long>(m_cid, newTime);
                        }
                    } else {
                        //否则赋予自己
                        target[kv.Key] = new KeyValuePair<int, long>(m_cid, newTime);
                        //Debug.Log("ApplyState");
                    }
                }
            }*/

        }

        //移除状态修改
        //单指时间到的正常移除
        private void RemoveState() {
            //if (MBuffMeta.StateFlags.Count == 0) return;
            //Debug.Log("remove state");

            //移除之前添加的, 是我赋予的buff
            /*var target = m_owner.BaseData.StateModifier;
			foreach (var kv in MBuffMeta.StateFlags) {
				if (kv.Value == 1 &&
					target.ContainsKey(kv.Key) &&
					target[kv.Key].Key == m_cid) {
					target.Remove(kv.Key);
				}
			}*/
        }

        //dot的执行
        private void OnPeriod()
		{
			//正常结束时施加的效果
			if (string.IsNullOrEmpty(MBuffMeta.PeriodicEffect))return;

			//var eff = CEffect.Create(MBuffMeta.PeriodicEffect, m_owner.gameObject, m_to.gameObject);
			//eff.Apply(m_owner, m_to);
			//eff.JobDown();
		}

		//buff生命周期正常结束
		private void OnFinal()
		{
			//正常结束时施加的效果
			if (!string.IsNullOrEmpty(MBuffMeta.FinalEffect)) {
				
			}

			RemoveModification();
			RemoveState();
			JobDown();
		}

		private void JobDown()
		{
			GameObject.Destroy(this);
		}

		protected override void OnDestroy(){
			base.OnDestroy();
			m_durationReg = null;
			m_dotReg = null;
		}
	}
}