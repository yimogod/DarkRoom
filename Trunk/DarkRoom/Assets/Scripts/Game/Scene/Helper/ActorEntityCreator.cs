using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.Game;
using UnityEngine;

namespace Sword
{
	/// <summary>
	/// 根据数据创建实际的UnitEntity, 回头看下是否放在entity类里面的一个实体方法
	/// </summary>
	public class ActorEntityCreator
	{
		public ActorEntityCreator()
		{
		}

		/*创建角色*/
		public void CreateActor(ActorVO vo, ActorEntity entity)
		{
			//var meta = vo.MetaBase;
			var attr = entity.AttributeSet;

			attr.InitLevel(1);
			//attr.InitAttribute(meta.MetaClass, meta.InitHealth, meta.InitMana, 0, 0, meta.InitDamage, meta.InitDef);
			attr.InitHealthAndMana();

			/*Rigidbody rig = entity.GetComponent<Rigidbody>();
			if (rig == null)
			{
			    rig = entity.gameObject.AddComponent<Rigidbody>();
			    rig.constraints = RigidbodyConstraints.FreezeRotation |
			                        RigidbodyConstraints.FreezePositionX |
			                        RigidbodyConstraints.FreezePositionZ;
			}*/

			//设置actor的坐标和位置
			//多了0.1高度是因为角色放的炸弹啊, 地面的滩涂啊. 要在角色的脚下
			//posComp.SetPos(pos, bornData.direction);
			//posComp.Pause();
			// posComp.speed = vo.meta.speed;
		}

		public void CreateMonsterAddon(CUnitEntity entity)
		{
			entity.Team = CUnitEntity.TeamSide.Blue;
			//GameObject gameObject = entity.gameObject;
			//gameObject.AddComponent<ActorFSMComp>();
			//gameObject.AddComponent<ActorAIComp>();
		}

		public void CreateAttachGO(CUnitEntity entity)
		{
			GameObject attachRoot = new GameObject();
			attachRoot.name = "go"; //RayConst.GO_NAME_ATTACH_ROOT;
			Transform rootTran = attachRoot.transform;
			CDarkUtil.AddChild(entity.transform, rootTran);

			//GameObject healthGO = AssetManager.LoadAndCreatePrefab("Prefabs/Hud/Health_Display_Preb");
			//Transform trans = healthGO.transform;
			//CDarkUtil.AddChild(rootTran, trans);
			//trans.localPosition = new Vector3(0, entity.height * 1.4f, 0);

			//头顶冒血
			//GameObject hpGO = AssetManager.LoadAndCreatePrefab("Prefabs/Hud/HP_Change_Preb");
			//trans = hpGO.transform;
			//CDarkUtil.AddChild(rootTran, trans);
		}

		public void Clear()
		{
		}
	}
}