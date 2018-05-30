using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	public class CEffectDamage : CEffect{
		//private CDamageInfo _damage = null;

		public override void Apply(CAIController owner, CAIController target){
			CEffectDamageMeta meta = MetaBase as CEffectDamageMeta;

			//if (_damage == null)
				//_damage = new CDamageInfo(owner, target, meta);
			//else
				//_damage.target = target;

			//OnImpact(owner, target, _damage);
		}


		//命中目标后的回调
		//--1.激活atk带着的能力, 应用于emeny
		//2.计算伤害
		//3. 通知self在命中的时候, 身上挂着的status触发作用
		//4. 通知enemy在命中的时候, 身上挂着的status触发作用
		//5. 如果是miss, 播放miss特效
		//6. 如果命中
		//	1.推开敌人	
		//	2.播放毒药声音
		//	3.计算status effect 列表
		//	4.根据Affliction计算status effect 列表
		//	5.播放攻击着身上的特效和地面特效, 以及被攻击者身上的特效
		//	6.播放攻击者到受害者之间的特效
		//	7.攻击者武器恶化
		//	8.受害者的护甲和盾牌恶化
		//7.敌人的health对象响应伤害
		//8.如果确实受到伤害,调用自己的 触发伤害方法--比如消耗耐力, 另外该方法会
		//9.如果死亡, 调用外部传进来的死亡回调--OnKill
		//10. 在敌人身上播放流血--根据命中类型
		//11. 如果暴击, 播放暴击回调
		/*private void OnImpact(CController owner, CController target, CDamageInfo damage){
			damage.CalcDamage();
			HandleDamage(damage);
		}*/

		/*private void HandleDamage(CDamageInfo damage){
			CActorEntity atker = damage.owner;
			CActorEntity target = damage.target;
			//如果被攻击着是ai, 需要通知ai谁打他了.
			if(atker.type == RayConst.UNIT_TYPE_ACTOR) {
				//ActorAIComp ai = target.GetComponent<ActorAIComp>();
				//if(ai != null)ai.DamageBy(atker);
			}

			bool killed = damage.DamageTarget();
			if(killed) {
				//如果是你击杀怪物, 则通知proxy进行存储
				if(atker.igroup == CUnitEntity.GroupSide.Red){
					//ApplicationFacade.instance.SendNotification(CMD20003.NAME, target);
				}
			} else {//没有击杀
				//如果有buff, 添加buff
				//if(damage.buffList.Count > 0) {
				//	BuffSystemComp system = target.GetComponent<BuffSystemComp>();
				//	system.AddBuff(damage.buffList, atker.cid);
				//}
			}


		}*/

		//private void HandleDamage(List<CDamageInfo> damageList){
		//	foreach(CDamageInfo damage in damageList){
		//		HandleDamage(damage);
		//	}
		//}

		//1. 计算伤害. 填充damage对象

	}
}