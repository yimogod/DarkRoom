using UnityEngine;
using System.Collections.Generic;
using DarkRoom.AI;
using DarkRoom.Core;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	public class CEffectEnumArea : CEffect
	{
	    private List<IGameplayAbilityUnit> m_searchResult = new List<IGameplayAbilityUnit>();

		protected CEffectEnumAreaMeta m_meta
		{
			get { return MetaBase as CEffectEnumAreaMeta; }
		}

	    public override void AppliedFrom(IGameplayAbilityUnit instigator)
	    {
	        base.AppliedFrom(instigator);
	        ApplyToPosition(m_owner.LocalPosition);
	    }

	    public override void ApplyToPosition(Vector3 localPosition)
	    {
            base.ApplyToPosition(localPosition);

	        m_searchResult.Clear();
            m_owner.SearchUnitsWithQuery(m_searchResult);

	        foreach (var item in m_searchResult)
	        {
	            if (item.InValid) continue;
	            //CEffect.DefaultCreateAndApply(m_meta.Area.Effect, owner, item as CAIController);
	        }
        }

		private void SearchAndApply()
		{
//			List<CController> list = new List<CController>();
			/*switch (m_meta.SearchMethod) {
					case AbilitySearchTargetMethod.TeamAll:
					list = target.SearchUnits();
                    break;
			case AbilitySearchTargetMethod.RandomInRange:
				list = target.SearchUnits ();
				if (list.Count > 1) {
					int i = CDarkRandom.Next (0, list.Count - 1);
					m_interested = list [i];
					list = owner.SearchUnits (m_interested.LocalPosition, m_meta.Area.Radius);
				}
					
					break;
					case AbilitySearchTargetMethod.ImpactRange:
					list = owner.SearchUnits(center, m_meta.Area.Radius);
					break;
			}*/
		}
	}
}
