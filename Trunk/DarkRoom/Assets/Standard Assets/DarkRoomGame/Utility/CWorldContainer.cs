using System;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.Game {
	/// <summary>
	/// 对战斗场景的分层. 这个单例mono类, 且负责特效的回归需要重新设计
	/// TODO 目前这个类是独立个体. 需要重新设计让其他模块来管理他, 并且不要技能mono和用单例
	/// </summary>
	public class CWorldContainer : MonoBehaviour{
		public const string LAYER_NAME_TERRAIN = "Terrain";
		public const string LAYER_NAME_TERRAIN_02 = "Terrain_02";
		public const string LAYER_NAME_LAKE = "Lake";

		/// <summary>
		/// 贴花. 这个层需要再观察是否需要
		/// </summary>
		public const string LAYER_NAME_DECAL = "Decal";

		/// <summary>
		/// 类似于魔兽争霸地编的 trigger 区域. 没有视图.
		/// </summary>
		public const string LAYER_NAME_TRIGGER = "Trigger";

		/// <summary>
		/// 主要单位层. 
		/// 比如3d, 包含英雄, npc, 障碍物, 掉落物
		/// </summary>
		public const string LAYER_NAME_UNIT = "Unit";
		public const string LAYER_NAME_WALKABLE = "Walkable";
		public const string LAYER_NAME_GLOW = "Glow";

		public enum LayerIndex {
			TERRIAN = 0,
			TERRIAN_02, //这个代表terraincomp的第二种地形, 索引为1, 不能删除
			ROAD, //lake用这层
			DECAL,
			UNIT,//TRIGGER, Deco
			LOW_SKY,
			HIGH_SKY,
			WALKABLE
		}


		private static CWorldContainer _ins = null;

		public static CWorldContainer Instance{
			get{
				if (_ins == null)
				{
					GameObject go = new GameObject("WorldContrainer");
					go.transform.localPosition = Vector3.zero;
					_ins = go.AddComponent<CWorldContainer>();
				}
				return _ins; 
			}
		}

		public GameObject Root;
		[NonSerialized]
		public Transform EffLayer;
		[NonSerialized]
		public Transform UnitLayer;//存放所有的可以交互的单位, 英雄,npc,掉落物品等等
		[NonSerialized]
		public Transform TriggerLayer;
		[NonSerialized]
		public Transform DecoLayer;//装饰物品
		[NonSerialized]
		public Transform PoolLayer;
		[NonSerialized]
		public Transform ItemPoolLayer; //一些英雄的装备

		//正在运行的task
		private List<EffectTask> _validTasks = new List<EffectTask>();
		//已经休眠的task
		private Queue<EffectTask> _invalidTask = new Queue<EffectTask>();

		private void Awake(){
			_ins = this;

			Root = gameObject;

			GameObject go = null;
			UnitLayer = Root.transform.Find("unit_layer");
			if(UnitLayer == null){
				UnitLayer = new GameObject("unit_layer").transform;
				UnitLayer.parent = Root.transform;
				UnitLayer.localPosition = Vector3.zero;
				return;
			}

			//4f是terrain的半高,
			go = UnitLayer.gameObject;
			go.layer = LayerMask.NameToLayer("Unit");

			go = CreateGO("trigger_layer");
			TriggerLayer = go.transform;
			TriggerLayer.localPosition = UnitLayer.localPosition;

			go = CreateGO("deco_layer");
			DecoLayer = go.transform;
			DecoLayer.localPosition = UnitLayer.localPosition;

			go = CreateGO("pool_layer");
			PoolLayer = go.transform;

			go = CreateGO("effect_layer");
			EffLayer = go.transform;
			EffLayer.localPosition = UnitLayer.localPosition;
		}

		//获取pool layer缓存的特效
		//这些特效之前已经被set active false
		public GameObject FindEffectInPool(string name){
			Transform tran = PoolLayer.Find(name);
			if (tran == null)return null;
			return tran.gameObject;
		}

		//获取effect layer正在播放的效果
		public GameObject FindEffectInLayer(string name){
			Transform tran = EffLayer.Find(name);
			if (tran == null)return null;
			return tran.gameObject;
		}

		//delay base on second
		public void PutBackToEffectPool(GameObject go, float delay = -1f){
			if (delay <= 0){
				CDarkUtil.AddChild(PoolLayer, go.transform);
				go.SetActive(false);
				return;
			}

			EffectTask task = null;
			if (_invalidTask.Count == 0){
				task = new EffectTask();
			} else{
				task = _invalidTask.Dequeue();
			}
			task.Activity(go, delay);
			_validTasks.Add(task);
		}

		void Update(){
			GameObject go = null;

			for (int i = _validTasks.Count - 1; i >= 0; i--){
				EffectTask task = _validTasks[i];
				bool v = task.ValidateTimeToGo();
				if (v){
					//go放入effectlayer
					PutBackToEffectPool(task.data, 0);
					task.Disable();
				}

				//从可用任务列表中移除, 然后添加到休眠列表中
				if (task.enabled)continue;
				_validTasks.RemoveAt(i);
				_invalidTask.Enqueue(task);

			}
		}

		private GameObject CreateGO(string name){
			GameObject go = new GameObject(name);
			CDarkUtil.AddChild(Root.transform, go.transform);
			return go;
		}

		void OnDestroy(){
			_ins = null;

			Root = null;
			EffLayer = null;
			DecoLayer = null;
			UnitLayer = null;
			PoolLayer = null;

			_validTasks.Clear();
			_validTasks = null;
			_invalidTask.Clear();
			_invalidTask = null;
		}


		public class EffectTask{
			public bool enabled = false;
			public long actionTime;

			public GameObject data;

			public void Activity(GameObject go, float duationInSecond){
				actionTime = CTimeUtil.GetCurrentMillSecondStamp() + (long)(duationInSecond * 1000);

				data = go;
				enabled = true;
			}

			//是否到了时间点, 然后执行某种行为
			public bool ValidateTimeToGo(){
				return CTimeUtil.HasClientArrivedTimeStampInMillSecond(actionTime);
			}

			public void Disable(){
				data = null;
				enabled = false;
			}
		}
	}

}