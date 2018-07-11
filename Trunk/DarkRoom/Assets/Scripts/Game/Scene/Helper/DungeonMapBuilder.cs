using System;
using System.Collections.Generic;
using DarkRoom.AI;
using DarkRoom.Game;
using DarkRoom.PCG;
using UnityEngine;

namespace Sword
{
    //需要记住, 我的地图terrain是已厘米为单位的. 也就是说unity的1 = 1cm. 1000 = 1m
    //这个跟2dtoolkit的2d机制有关系. 他认为最小单位1像素, 也就意味这1 = 1px = 1cm
    //这以为着3dmax做模型以米为单位的话, 导入的时候需要做缩放
    //在单位做运动的时候也需要记住以mi为单位
    public class DungeonMapBuilder
    {
        private DungeonMapHelper _helper;
        private TMap _world;

        private ActorGenerator _actorGen;
        //private TileMapBaseGenerator _mapGen;

        public DungeonMapBuilder(TMap world)
        {
            _world = world;
        }

        //随机生成地图及tile数据
        public void CreateMap(Terrain3DComp map)
        {
            //地图生成器生成地图
            /*TileMapPlainRandom3DGenerator gen = new TileMapPlainRandom3DGenerator();
            gen.treeCellularPercent = _mapMeta.treeCellularPercent;
            gen.decoBlockNum = _mapMeta.decoBlockNum;
            gen.decoDestroyNum = _mapMeta.decoDestroyNum;
            gen.Generate(map);

            _mapGen = gen;
            map.ForceBuild();*/
        }

        //讲生成器的通行数据拷贝到我们的tmap中
        public void CopyData()
        {
            //然后获取地图的各种数据
            var grid = _world.walkGrid;
            grid.Init(_mapMeta.rows, _mapMeta.cols, true);
            //grid.CopyUnWalkableFrom(_mapGen.walkGrid);
           // _world.typeGrid = typeGrid;
        }

        public void CreateOther()
        {
            //_helper = new DungeonMapHelper(_mapGen.walkGrid);
           // _actorGen = new ActorGenerator(_world.terrain);

            CreateTrigger();
            CreateUnit();
        }

        //创建单位, 英雄和怪物
        private void CreateUnit()
        {
            //1. BornHero
           // CreateHero(_mapGen.startTile);

            //2. 创建怪物
            CreateMonster(_mapMeta.monster_0, _mapMeta.monster_0_lv, _mapMeta.monster_0_ai, _mapMeta.monster_0_num);
            CreateMonster(_mapMeta.monster_1, _mapMeta.monster_1_lv, _mapMeta.monster_1_ai, _mapMeta.monster_1_num);
            CreateMonster(_mapMeta.monster_2, _mapMeta.monster_2_lv, _mapMeta.monster_2_ai, _mapMeta.monster_2_num);
            CreateMonster(_mapMeta.monster_3, _mapMeta.monster_3_lv, _mapMeta.monster_3_ai, _mapMeta.monster_3_num);
            CreateMonster(_mapMeta.boss_1, _mapMeta.boss_1_lv, _mapMeta.boss_1_ai, 1);


            //3. 创建boss, 在宝箱附近
            if (_mapMeta.boss_0 != -1)
            {
                //Vector3 pos = _helper.FindFreeTileNear(_mapGen.importTile_1);
                //UnitBornData bornData =
                //    UnitBornData.CreateUnitBornData(_mapMeta.boss_0, CUnitEntity.TeamSide.Blue, pos);
                //CreateMonsterAtPos(bornData, _mapMeta.boss_0_lv, _mapMeta.boss_0_ai);
            }
        }

        //创建英雄, 固定位置, 其实更好的设计应该是祭坛~~~~
        private void CreateHero(Vector3 pos)
        {
            UnitBornData bornData = UnitBornData.CreateUnitBornData(10001, CUnitEntity.TeamSide.Red, pos);

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
                UnitBornData bornData = _helper.CreateRandomUnitBorn(metaId, CUnitEntity.TeamSide.Blue);
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

        public void Clear()
        {
            _helper.Clear();
            _actorGen.Clear();
        }

        private MapMeta _mapMeta
        {
            get { return _world.meta; }
        }

        public CAssetGrid assetGrid
        {
            get
            {
                return null;
                //return _mapGen.assetGrid;
            }
        }

        public CAssetGrid typeGrid
        {
            //get { return _mapGen.typeGrid; }
            get { return null; }
        }
    }
}