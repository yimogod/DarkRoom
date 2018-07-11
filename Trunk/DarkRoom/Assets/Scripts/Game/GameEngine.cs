using System.Collections.Generic;
using DarkRoom.Game;
using DarkRoom.Utility;
using UnityEngine;
using Rayman;

namespace Sword
{
    //针对手机只能单机操作的方式的gameplay
    public class GameEngine
    {
        private static GameEngine _ins = new GameEngine();
        private bool _enabled = false;

        //目前玩家操纵的角色
        private CUnitEntity _mainActor = null;
        private CUnitEntity _overActor = null;

        private GameEngine()
        {
        }

        public static GameEngine Instance
        {
            get { return _ins; }
        }

        public void Init()
        {
        }

        public void Start()
        {
            _enabled = true;
        }

        public void Stop()
        {
            _enabled = false;
        }

        /*public void Update()
        {
            if (!_enabled) return;
            MouseController.Instance.Update();
            if (_mainActor == null)
            {
                _mainActor = TMap.Instance.hero;
            }


            //如果玩家点击了场景
            if (MouseController.Instance.hasDown)
            {
                Transform target = MouseController.Instance.hitUnit;

                bool hasClickActor = false;
                CUnitEntity actor = null;
                if (target != null)
                {
                    actor = target.GetComponent<CUnitEntity>();
                    if (actor != null)
                        hasClickActor = true;
                }

                //如果点击到了某个单位, target也可能不是actor. 这里先假设是
                //这里还有其他逻辑.
                //比如如果是敌人的话, 走过去攻击.
                if (hasClickActor)
                {
                    if (_mainActor.inTargetSelectCommand)
                    {
                        _mainActor.selectedActor = actor;
                    }
                    else
                    {
                        if (_mainActor.IsFriendGroup(actor.igroup))
                        {
                            //选择的是友人, 就切换主角
                            _mainActor = actor;
                            _mainActor.selectedActor = null;
                        }
                        else
                        {
                            //如果不是友人, 就是A过去
                            _mainActor.selectedActor = actor;
                            _mainActor.SelectAbility(0);
                        }
                    }

                    //如果有目标. 就用当前actor选择的技能affect
                    if (_mainActor.selectedActor != null)
                    {
                        //然后选中的角色走过去
                        ActorBaseAIComp ai = _mainActor.GetComponent<ActorBaseAIComp>();
                        ai.UseAbilityAffectTo(_mainActor.selectedActor);

                        GameCursor.Instance.PlayActorSelectedEffectOnGround(_mainActor.selectedActor.transform);
                    }

                }
                else
                {
                    //如果点击到了地面. 播放特效
                    Vector3 pos = MouseController.Instance.mouseDownWorldPosition;
                    Vector3 tile = MapUtil.GetTileByPos(pos);
                    if (TMap.Instance.walkable(tile))
                    {
                        GameCursor.Instance.PlayClickEffectOnGround(pos);
                        //然后选中的角色走过去
                        ActorBaseAIComp ai = _mainActor.GetComponent<ActorBaseAIComp>();
                        ai.ForceGotoPosition(pos);
                    }
                }
            }

            //探测鼠标经过的单位然后高亮之
            if (MouseController.Instance.overUnit != null)
            {
                Transform target = MouseController.Instance.overUnit;
                CUnitEntity actor = target.GetComponent<CUnitEntity>();
                if (actor != null && actor != TMap.Instance.hero)
                {
                    GameCursor.Instance.DesiredCursor = CursorType.Attack;

                    if (actor != _overActor)
                    {
                        if (_overActor != null)
                        {
                            _overActor.GetComponent<UnitViewComp>().UnGlow();
                        }

                        _overActor = actor;
                        _overActor.GetComponent<UnitViewComp>().Glow();
                    }
                }
            }
            else if (_overActor != null)
            {
                _overActor.GetComponent<UnitViewComp>().UnGlow();
                _overActor = null;
            }


            //探测鼠标经过不可通行区域, 更改cursor
            Vector3 overPos = MouseController.Instance.mouseOverWorldPosition;
            if (!RayUtil.IsInvalidVec3(overPos))
            {
                overPos = MapUtil.GetTileByPos(overPos);
                bool walkable = TMap.Instance.walkable(overPos);
                if (!walkable)
                {
                    GameCursor.Instance.DesiredCursor = CursorType.NoWalk;
                }
            }


            if (TMap.Instance.hero.dead)
            {
                ApplicationFacade.instance.SendNotification(NotiConst.GAME_LOSE);
                Stop();
            }
            
        }*/

        public void Clean()
        {
            _enabled = false;
            TMap.Instance.Clean();
        }
    }
}