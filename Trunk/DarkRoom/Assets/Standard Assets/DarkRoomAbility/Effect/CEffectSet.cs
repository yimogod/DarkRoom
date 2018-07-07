using System;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	public class CEffectSet : CEffect{
		private CEffectSetMeta m_meta {
			get { return MetaBase as CEffectSetMeta; }
		}

		//目前调试的, 此effect作用于自己是ok的
		// TODO 我们没有处理此effect作用于target身上的情况, 根据具体技能来处理
		/*public override void Apply(CAIController from, CAIController to) {
			int i = 0;
			foreach (var mid in m_meta.EffectList) {
				var eff = CEffect.DefaultCreateAndApply(mid, from, to);
				eff.Index = i;
				i++;
			}
		}*/

		/*public override void Apply(CAIController from, Vector3 to) {
			int i = 0;
			foreach (var mid in m_meta.EffectList) {
				var eff = CEffect.DefaultCreateAndApply(mid, from, to);
				eff.Index = i;
				i++;
			}
		}*/
	}
}