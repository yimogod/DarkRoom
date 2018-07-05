using System;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 应用行为的效果. 一个很重要的作用就是添加buff
	/// </summary>
	public class CEffectRemoveBuff : CEffect
	{
		protected CEffectApplyBuffMeta m_meta {
			get { return MetaBase as CEffectApplyBuffMeta; }
		}

		public override void Apply(CAIController from, CAIController to)
		{
			base.Apply(from, to);
			//buff自己销毁自己
			CBuff beh = CBuff.Create(m_meta.Behavior, m_owner.gameObject);
			beh.Apply(from, to);
        }

		/// <summary>
		/// 如果我们添加buff的对象的是坐标, 那么我们认为buff是添加到了自己身上
		/// 
		/// 根据冲锋得到的, 未来可能会修改
		/// </summary>
		public override void Apply(CAIController from, Vector3 to) {
			if (m_meta.AttachTarget) {
				Debug.Log(m_meta.Id + " can not apply buff to pos with WhichUnit = Target");
				return;
			}

			Apply(from, from);
		}
	}
}

