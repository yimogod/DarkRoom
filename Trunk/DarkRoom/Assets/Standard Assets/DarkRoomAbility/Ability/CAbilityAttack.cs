using System;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	//挂在角色身上, 角色的默认攻击技能
	//其本质是用角色身上的武器进行攻击
	//从而调用武器身上带有的effect

	//进行攻击的目标我们是有AI/鼠标进行选定的，除非取消这个技能， 否则会进行攻击判定
	//即使target已经逃出攻击范围
	public class CAbilityAttack : CAbility
	{
		/*public CWeaponEntity primaryWeapon
		{
			get{
				//如果没有武器， 就赤手空拳打
				if (_owner.primaryWeapon == null){
					GameObject go = new GameObject("weapon_fist");
					CDarkUtil.AddChild(_owner.transform, go.transform);

					CWeaponEntity w = go.AddComponent<CWeaponEntity>();
					w.SetMeta(3);
					_owner.EquipWeapon(w);
				}

				return _owner.primaryWeapon;
			}
		}*/

		//普攻, 那只能对敌人使用
		public override  AffectDectectResult CanAffectOnTarget(IGameplayAbilityUnit target){
			m_target = target;

			AffectDectectResult result = base.CanAffectOnTarget(m_target);
			if (result != AffectDectectResult.Success)return result;


			//if (_owner.IsFriendGroup(_target.igroup))
			//	return AffectDectectResult.TargetGroupNotMatch;

			/*float range = primaryWeapon.meta.range;
			Bounds src = _owner.bounds;
			src.size = new Vector3(range, range, range);
			bool intersect = src.Intersects(_target.bounds);
			if (!intersect)return AffectDectectResult.OutOfRange;*/

			return AffectDectectResult.Success;
		}
	}
}