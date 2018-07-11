using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.Game;
using UnityEngine;
using Sword;

namespace Sword
{

//创建地图的辅助类,
//1. 一些实用方法
//2. 创建单位的实用方法
    public class ActorGenerator
    {
        private Transform _unitLayer;
        private TerrainComp _terrain;

        public ActorGenerator(TerrainComp terrain)
        {
            _unitLayer = CWorld.Instance.Layer.UnitLayer;
            _terrain = terrain;
        }

        /*创建角色*/
        public CUnitEntity CreateActor(ActorVO vo, UnitBornData bornData, GameObject gameObject = null)
        {
            bool hasActorInit = true;
            if (gameObject == null)
            {
                gameObject = null;//AssetManager.LoadActorPrefab(vo.MetaBase.prefab);
                hasActorInit = false;
            }

            CUnitSpacialComp posComp = gameObject.AddComponent<CUnitSpacialComp>();
            CUnitEntity entity = gameObject.AddComponent<CUnitEntity>();
            //entity.RestoreForm(vo);

            CUnitViewComp view = gameObject.AddComponent<CUnitViewComp>();
            //gameObject.AddComponent<CAbilAttack>();
            //gameObject.AddComponent<ActorDebugComp>();
            //gameObject.AddComponent<CMover>();


            //如果go是外面传进来的, 说明位置已经定好了
            if (!hasActorInit)
            {
                Rigidbody rig = gameObject.GetComponent<Rigidbody>();
                if (rig == null)
                {
                    rig = gameObject.AddComponent<Rigidbody>();
                    rig.constraints = RigidbodyConstraints.FreezeRotation |
                                      RigidbodyConstraints.FreezePositionX |
                                      RigidbodyConstraints.FreezePositionZ;
                }

                //设置actor的坐标和位置
                CDarkUtil.AddChild(_unitLayer, gameObject.transform);
                Vector3 pos = CMapUtil.GetTileCenterPosByColRow(bornData.col, bornData.row);
                //多了0.1高度是因为角色放的炸弹啊, 地面的滩涂啊. 要在角色的脚下
                pos.y = _terrain.GetWorldY(bornData.col, bornData.row) + 0.1f;
                //posComp.SetPos(pos, bornData.direction);
                //posComp.Pause();
               // posComp.speed = vo.meta.speed;
            }

            return entity;
        }

        public void CreateHeroAddon(CUnitEntity entity)
        {
            entity.Team = CUnitEntity.TeamSide.Red;
            GameObject gameObject = entity.gameObject;
            //目前我们仅仅英雄会有不同的武器
            //gameObject.AddComponent<HeroFSMComp>();
            //gameObject.AddComponent<HeroControlKeyboard>();
        }

        public void CreateMonsterAddon(CUnitEntity entity)
        {
            entity.Team = CUnitEntity.TeamSide.Blue;
            GameObject gameObject = entity.gameObject;
            //gameObject.AddComponent<ActorFSMComp>();
            //gameObject.AddComponent<ActorAIComp>();
        }

        public void CreateAttachGO(CUnitEntity entity)
        {
            GameObject attachRoot = new GameObject();
            attachRoot.name = "go";//RayConst.GO_NAME_ATTACH_ROOT;
            Transform rootTran = attachRoot.transform;
            CDarkUtil.AddChild(entity.transform, rootTran);

            GameObject healthGO = AssetManager.LoadAndCreatePrefab("Prefabs/Hud/Health_Display_Preb");
            Transform trans = healthGO.transform;
            CDarkUtil.AddChild(rootTran, trans);
            //trans.localPosition = new Vector3(0, entity.height * 1.4f, 0);

            //头顶冒血
            GameObject hpGO = AssetManager.LoadAndCreatePrefab("Prefabs/Hud/HP_Change_Preb");
            trans = hpGO.transform;
            CDarkUtil.AddChild(rootTran, trans);
        }

        public void Clear()
        {
            _unitLayer = null;
        }
    }
}