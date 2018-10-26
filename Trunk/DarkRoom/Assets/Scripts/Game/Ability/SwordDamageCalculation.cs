using System;
using System.Collections.Generic;
using DarkRoom.GamePlayAbility;

namespace Sword
{
    public class SwordDamageCalculation : CAbilityAttributeExecutionCalculation
    {
		private Random m_rand = new Random();

        public override void Execute(IGameplayAbilityUnit instigator, IGameplayAbilityUnit victim)
        {
            base.Execute(instigator, victim);

			var sourceCtrl = victim as ActorController;
			var targetCtrl = instigator as ActorController;
			var source = sourceCtrl.Pawn as ActorEntity;
			var target = targetCtrl.Pawn as ActorEntity;
	        var srcAttr = source.AttributeSet;
	        var targAttr = target.AttributeSet;

			SwordDamagePacket damagePacket = new SwordDamagePacket();
	        //damagePacket.MinRawAttackPower

			damagePacket.TargetDodgeChance = targAttr.DodgeChance;


			//1. 计算无敌, 如果无敌就无伤

			//2. 计算是否miss
			//2.1 先计算是否有无视闪避--必定命中
			//2.2 产生随机数来判定是否miss
			float dodgeChance = m_rand.Next();
	        if (dodgeChance < damagePacket.TargetDodgeChance)
	        {
		        damagePacket.OutIsDodged = true;
		        SendDamageEvent();
				return;
	        }

			//3. 计算攻击属性包
        }

	    private void SendDamageEvent()
	    {
		    
	    }
    }
}
