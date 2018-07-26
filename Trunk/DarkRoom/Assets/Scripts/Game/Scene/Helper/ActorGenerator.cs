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

        public void Generate(MapMeta meta, CAssetGrid assetGrid)
        {
            m_tilesData = new DungeonMapPlaceholder(assetGrid);
            m_mapMeta = meta;
        }

        //创建单位, 英雄和怪物
        private void CreateUnit()
        {
            //1. BornHero
            //CreateHero(_mapGen.startTile);

            //2. 创建怪物
            CreateMonster(m_mapMeta.monster_0, m_mapMeta.monster_0_lv, m_mapMeta.monster_0_ai, m_mapMeta.monster_0_num);
            CreateMonster(m_mapMeta.monster_1, m_mapMeta.monster_1_lv, m_mapMeta.monster_1_ai, m_mapMeta.monster_1_num);
            CreateMonster(m_mapMeta.monster_2, m_mapMeta.monster_2_lv, m_mapMeta.monster_2_ai, m_mapMeta.monster_2_num);
            CreateMonster(m_mapMeta.monster_3, m_mapMeta.monster_3_lv, m_mapMeta.monster_3_ai, m_mapMeta.monster_3_num);
            CreateMonster(m_mapMeta.boss_1, m_mapMeta.boss_1_lv, m_mapMeta.boss_1_ai, 1);


            //3. 创建boss, 在宝箱附近
            if (m_mapMeta.boss_0 != -1)
            {
                //Vector3 pos = _helper.FindFreeTileNear(_mapGen.importTile_1);
                //UnitBornData bornData =
                //    UnitBornData.CreateUnitBornData(_mapMeta.boss_0, CUnitEntity.TeamSide.Blue, pos);
                //CreateMonsterAtPos(bornData, _mapMeta.boss_0_lv, _mapMeta.boss_0_ai);
            }
        }

        //创建英雄, 固定位置, 其实更好的设计应该是祭坛~~~~
        private void CreateHero(Vector2Int pos)
        {
            //UnitBornData bornData = UnitBornData.CreateUnitBornData(10001, CUnitEntity.TeamSide.Red, pos);

            /*CActorVO vo = ProxyPool.userProxy.hero;
            if (vo == null)
            {
                vo = new CActorVO(bornData.metaId);
                vo.Init(1);
            }
            else
            {
                (vo as CActorVO).Reborn();
            }

            CUnitEntity entity = _actorGen.CreateActor(vo, bornData);
            _actorGen.CreateHeroAddon(entity);
            _actorGen.CreateAttachGO(entity);

            _world.AddUnit(entity);
            _helper.AddUnitToDict(entity.posColRow, 5);*/
        }

        //根据meta配置数据创建单个魔物
        private void CreateMonster(int metaId, int lv, int ai, int num)
        {
            if (metaId == -1 || num == 0) return;

            for (int i = 0; i < num; ++i)
            {
                UnitBornData bornData = m_tilesData.CreateRandomUnitBorn(metaId, CUnitEntity.TeamSide.Blue);
                if (bornData.invalid) continue;
                CreateMonsterAtPos(bornData, lv, ai);
            }
        }

        //在固定位置创建怪物
        private void CreateMonsterAtPos(UnitBornData bornData, int lv, int ai)
        {
            /*CActorVO vo = new CActorVO(bornData.metaId);
            vo.ai = AIMetaManager.GetMeta(ai);
            vo.Init(lv);

            CUnitEntity entity = _actorGen.CreateActor(vo, bornData);
            _actorGen.CreateMonsterAddon(entity);
            _actorGen.CreateAttachGO(entity);

            _world.AddUnit(entity);
            _helper.AddUnitToDict(entity.posColRow);*/
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