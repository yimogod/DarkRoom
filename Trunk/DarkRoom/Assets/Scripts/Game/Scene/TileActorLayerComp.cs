using System.Collections;
using DarkRoom.Core;
using DarkRoom.Game;
using UnityEngine;

namespace Sword
{
	public class TileActorLayerComp : MonoBehaviour
	{
		private CAssetGrid m_assetGrid;
		private Transform m_parent;

		private ActorEntityCreator m_creator = new ActorEntityCreator();

		public void SetAssetGrid(CAssetGrid grid)
		{
			m_assetGrid = grid;
		}

		public void BuildHero(Vector2Int pos)
		{
			Vector3 localPos = CMapUtil.GetTileCenterPosByColRow(pos);
			localPos.y = GameConst.DEFAULT_TERRAIN_HEIGHT + 0.1f;
			ProxyPool.HeroProxy.CreateHeroEntity(localPos);
		}

		public void Build()
		{
			if (m_assetGrid == null)
			{
				Debug.LogError("AssetGrid Must Not Null");
				return;
			}

			m_parent = CWorld.Instance.Layer.UnitLayer;
			//StartCoroutine("CoroutineBuild");
		}

		private IEnumerator CoroutineBuild()
		{
			for (int row = 0; row < m_assetGrid.NumRows; row++)
			{
				yield return new WaitForEndOfFrame();
				for (int col = 0; col < m_assetGrid.NumCols; col++)
				{
					var mid = m_assetGrid.GetNodeType(col, row);
					if (mid <= 0) continue;

					var subType = m_assetGrid.GetNodeSubType(col, row);
					if(subType <= 0)continue;

					var pos = CMapUtil.GetTileCenterPosByColRow(col, row);
					pos.y = GameConst.DEFAULT_TERRAIN_HEIGHT + 0.1f;


				}
			}
		}

		protected void LoadAndCreateModel(string prefab, Vector3 pos)
		{
			AssetManager.LoadTilePrefab("map_forest", prefab, m_parent, pos);
		}






		//创建英雄, 固定位置, 其实更好的设计应该是祭坛~~~~
		/*private void CreateHero(Vector2Int tile)
		{
			var entity = ProxyPool.HeroProxy.CreateHeroEntity();


			var meta = ActorMetaManager.GetMeta(metaId);
			HeroVO vo = new HeroVO();
			//var vo = ProxyPool.UserProxy.Hero;

			var pos = CMapUtil.GetTileCenterPosByColRow(tile);
			var entity = CWorld.Instance.SpawnUnit<HeroEntity>("Hero_" + metaId, pos);
			m_creator.CreateActor(vo, entity);
			m_creator.CreateAttachGO(entity);

			m_tilesData.AddUnitToDict(entity.LocalPosition, 5);
		}*/

		//在固定位置创建怪物
		private void CreateMonsterAtPos(int metaId, int lv, Vector3 pos)
		{
			var meta = ActorMetaManager.GetMeta(metaId);
			ActorVO vo = new ActorVO();
			//vo.ai = AIMetaManager.GetMeta(ai);
			//vo.Init(lv);

			var entity = CWorld.Instance.SpawnUnit<BotEntity>("Bot_" + metaId, pos);
			m_creator.CreateMonsterAddon(entity);
			m_creator.CreateAttachGO(entity);
		}


		//创建各种触发器
		private void CreateTrigger()
		{
			//生成通关传送门
			/*Vector3 pos = _helper.FindFreeTileNear(_mapGen.endTile);
			_triggerGen.BornSucessPortal(pos);
			_helper.AddUnitToDict(pos, 2);

			for (int i = 0; i < _mapMeta.trigger_0_num; ++i)
			{
			    pos = _helper.FindFreeTile();
			    _triggerGen.CreateOneTrigger(_mapMeta.trigger_0, pos);
			    _helper.AddUnitToDict(pos, 0);
			}

			for (int i = 0; i < _mapMeta.trigger_1_num; ++i)
			{
			    pos = _helper.FindFreeTile();
			    _triggerGen.CreateOneTrigger(_mapMeta.trigger_1, pos);
			    _helper.AddUnitToDict(pos, 0);
			}

			TriggerMeta meta = TriggerMetaManager.GetMeta(_mapMeta.chest_2);
			_triggerGen.CreateTrigger_Chest(meta, _mapGen.importTile_1);*/
		}
	}
}