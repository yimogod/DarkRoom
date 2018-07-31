using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.Game;
using UnityEngine;

namespace Sword
{
    public class ActorGenerator : MonoBehaviour
    {
        private MapMeta m_mapMeta;
        private DungeonMapPlaceholder m_tilesData;
        private ActorEntityCreator m_creator;

        public void Generate(MapMeta meta, CAssetGrid assetGrid)
        {
            m_tilesData = new DungeonMapPlaceholder(assetGrid);
            m_mapMeta = meta;
        }

        //创建单位, 英雄和怪物
        private void CreateUnit()
        {
            //1. BornHero
            CreateHero(10001, Vector2Int.down);

            //2. 创建怪物
            foreach (var m in m_mapMeta.Monsters)
            {
                CreateMonster(m.x, m.y, m.z);
            }
            
            //CreateMonster(m_mapMeta.boss_1, m_mapMeta.boss_1_lv, m_mapMeta.boss_1_ai, 1);


            //3. 创建boss, 在宝箱附近
            //if (m_mapMeta.boss_0 != -1)
           // {
                //Vector3 pos = _helper.FindFreeTileNear(_mapGen.importTile_1);
                //UnitBornData bornData =
                //    UnitBornData.CreateUnitBornData(_mapMeta.boss_0, CUnitEntity.TeamSide.Blue, pos);
                //CreateMonsterAtPos(bornData, _mapMeta.boss_0_lv, _mapMeta.boss_0_ai);
          //  }
        }

        //创建英雄, 固定位置, 其实更好的设计应该是祭坛~~~~
        private void CreateHero(int metaId, Vector2Int tile)
        {
            var meta = ActorMetaManager.GetMeta(metaId);
            var vo = ProxyPool.UserProxy.Hero;
            vo.SetMeta(meta);

            var pos = CMapUtil.GetTileCenterPosByColRow(tile);
            var entity = CWorld.Instance.SpawnUnit<HeroEntity>("Hero_" + metaId, pos);
            m_creator.CreateActor(vo, entity);
            m_creator.CreateHeroAddon(entity);
            m_creator.CreateAttachGO(entity);

            m_tilesData.AddUnitToDict(entity.LocalPosition, 5);
        }

        //根据meta配置数据创建单个魔物
        private void CreateMonster(int metaId, int lv, int num)
        {
            if (metaId <= 0 || num == 0) return;

            for (int i = 0; i < num; ++i)
            {
                CreateMonsterAtPos(metaId, lv, Vector3.zero);
            }
        }

        //在固定位置创建怪物
        private void CreateMonsterAtPos(int metaId, int lv, Vector3 pos)
        {
            var meta = ActorMetaManager.GetMeta(metaId);
            ActorVO vo = new ActorVO(null);
            //vo.ai = AIMetaManager.GetMeta(ai);
            //vo.Init(lv);

            var entity = CWorld.Instance.SpawnUnit<BotEntity>("Bot_" + metaId, pos);
            m_creator.CreateMonsterAddon(entity);
            m_creator.CreateAttachGO(entity);

            m_tilesData.AddUnitToDict(entity.LocalPosition);
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