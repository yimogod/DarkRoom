namespace DarkRoom.GamePlayAbility {
	public class CAbilEffectTarget : CAbility
	{
		/*protected AbilityObject m_abilityObject = null;
		protected  Dictionary<string, AbilityObject> m_abilityObjectOhHit = null;

		private CAbilEffectTargetMeta m_meta{
			get{ return MetaBase as CAbilEffectTargetMeta; }
		}

		protected override void Start()
		{
			if (NeedEditorShow && !string.IsNullOrEmpty(m_meta.AbilityShowConfPath)) {
				var actor = m_owner as ActorController;
				if (actor != null) {
					string prefab = actor.Data.Meta.Prefab;
					string path = string.Empty;
					if (actor.Entity.IsHero) {
						path = string.Format( "Assets/Arts/Config/Unit/Hero/{0}/{1}.asset", prefab, m_meta.AbilityShowConfPath);
					} else {
						path = string.Format( "Assets/Arts/Config/Unit/Soldier/{0}/{1}.asset", prefab, m_meta.AbilityShowConfPath);
					}

					LoadAssetCallbacks cb = new LoadAssetCallbacks(BTCallback);
					//Debug.Log ("ability path is " + path);
					Facade.Resource.LoadAssetAsync(path, cb);
				}
			}
		}

		//加载技能配置文件回调
		private void BTCallback(string assetName, object asset, float duration, object userData){
			m_abilityObject = ScriptableObject.Instantiate ( asset as AbilityObject);
			m_abilityObjectOhHit = new Dictionary<string, AbilityObject> ();

			//加载依赖文件
			var actor = m_owner as ActorController;
			string prefab = actor.Data.Meta.Prefab;
			foreach (string item in m_abilityObject.Dependencies) {
				LoadAssetCallbacks cb = new LoadAssetCallbacks(BTCallback2);

				string path = string.Empty;
				if (actor.Entity.IsHero) {
					path = string.Format( "Assets/Arts/Config/Unit/Hero/{0}/{1}.asset", prefab, item);
				} else {
					path = string.Format( "Assets/Arts/Config/Unit/Soldier/{0}/{1}.asset", prefab, item);
				}
				Facade.Resource.LoadAssetAsync(path, cb);
			}
		}

		private void BTCallback2(string assetName, object asset, float duration, object userData){
			var ori = asset as AbilityObject;
			var obj = ScriptableObject.Instantiate (ori);
			m_abilityObjectOhHit[ori.name] = obj;
		}

		public override AffectDectectResult Activate() {
			Debug.LogError("CAbilEffectTarget need a target who is not selfs");
			return AffectDectectResult.TargetInvalid;
		}

		public override AffectDectectResult Activate(CAIController target)
		{
			AffectDectectResult result = base.Activate(target);
			if (result == AffectDectectResult.Success) {

				//编辑器内容初始化
				if (m_abilityObject != null) {
					Facade.Event.Subscribe (AbilityEventType.HitOn.GetStringValue(), OnHit);
					Facade.Event.Subscribe (AbilityEventType.AnimationFinish.GetStringValue(), OnFinish);
					m_owner.ObserveMessage("NotiyEffectCallBack", OnNotifyBack);

					m_abilityObject.EventSystem = BattleScene.AbilityEventSystem;
					(m_owner as ActorController).PlayAbilityObject(m_abilityObject, null);
				}

				m_effect = CEffect.Create(m_meta.Effect, m_ownerGO, m_targetGO);
				m_effect.Apply(m_owner, target);
				m_effect.JobDown();
			}

			return result;
		}

		private AbilityHitOnEventArgs m_currHitArgs;
		private void OnHit(object sender, BaseEventArgs args){
			m_currHitArgs = args as AbilityHitOnEventArgs;
			if (m_currHitArgs.ActorId != m_owner.Pawn.cid)return;

			CAIMessage m = new CAIMessage(0, "NotiyEffect", m_owner, m_currHitArgs.Index);
			CAIMessage.Send(m_owner, m);
		}

		private void OnNotifyBack(CAIMessage message){
			ActorController actor = message.Data as ActorController;
			if (actor == null)return;
			if (m_currHitArgs == null)return;
			if (m_currHitArgs.TargetActingObjects == null)return;

			//地面特效
			string str = m_currHitArgs.TargetActingObjects [(int)AbilityActingTargetType.Point];
			if (!String.IsNullOrEmpty (str)) {
				Debug.Log("play ground effect");
				var go = new GameObject("whatever");
				go.transform.position = actor.transform.position;

				var updater = go.AddComponent<AbilityPlayerUpdater>();
				var obj = ScriptableObject.Instantiate<AbilityObject>(m_abilityObjectOhHit [str]);
				updater.Play (obj);
			}


			if (!actor.Entity.DeadOrDying) {
				if (actor.Entity.IsHero) {
					//英雄特效
					str = m_currHitArgs.TargetActingObjects [(int)AbilityActingTargetType.General];
					if (!String.IsNullOrEmpty (str)) {
						var obj = ScriptableObject.Instantiate<AbilityObject>(m_abilityObjectOhHit [str]);
						actor.AbilityObjToBePlaying = obj;
						CAIMessage m = new CAIMessage (0, ActorAIAgent.UNIT_OUT_OF_CTRL, m_owner);
						CAIMessage.Send (actor, m);

						//Debug.Log("play hero effect " + str + "   " + actor.name);
					}
				}else{
					//小兵特效
					str = m_currHitArgs.TargetActingObjects [(int)AbilityActingTargetType.Pawn];
					if (!String.IsNullOrEmpty (str)) {
						var obj = ScriptableObject.Instantiate<AbilityObject>(m_abilityObjectOhHit [str]);
						actor.AbilityObjToBePlaying = obj;
						CAIMessage m = new CAIMessage (0, ActorAIAgent.UNIT_OUT_OF_CTRL, m_owner);
						CAIMessage.Send (actor, m);

						//Debug.Log("play unit effect " + str);
					}
				}
			}
		}

		private void OnFinish(object sender, BaseEventArgs args){
			var finishArgs = args as AbilityAnimationFinishEventArgs;
			if (finishArgs.ActorId != m_owner.Pawn.cid)return;

			Debug.Log ("OnFinish");
			m_owner.UnobserveMessage("NotiyEffectCallBack", OnNotifyBack);
			Facade.Event.Unsubscribe (AbilityEventType.HitOn.GetStringValue(), OnHit);
			Facade.Event.Unsubscribe (AbilityEventType.AnimationFinish.GetStringValue(), OnFinish);
		}

		public override AffectDectectResult Activate(Vector3 target) {
			AffectDectectResult result = base.Activate(target);
			m_effect = CEffect.Create(m_meta.Effect, m_ownerGO, m_targetGO);
			if (result == AffectDectectResult.Success) {
				m_effect.Apply(m_owner, target);
				m_effect.JobDown();
			}

			return result;
		}

		public override AffectDectectResult CanAffectOnTarget(CAIController target){
			AffectDectectResult result = base.CanAffectOnTarget(target);
			if (result != AffectDectectResult.Success)return result;

			if (m_meta.Range > 0) {
				float dist = m_owner.GetSquaredXZDistanceTo_NoRadius(target);
				if (dist > (m_meta.Range * m_meta.Range))
					return AffectDectectResult.OutOfRange;
			}

			return AffectDectectResult.Success;
		}

		public override AffectDectectResult CanAffectOnTarget(Vector3 target) {
			AffectDectectResult result = base.CanAffectOnTarget(target);
			if (result != AffectDectectResult.Success) return result;

			if (m_meta.Range > 0) {
				//技能不考虑自己和对方的半径, 否则在AI阶段如果是以
				//CanAffectOnTarget(CController target)为考量, 这样考虑到了对方的半径
				//但具体技能是以地点为目标, 使用的是CanAffectOnTarget(Vector3 target)
				//也就是说没有考虑对方的半径. 这样会造成结果不统一
				float dist = m_owner.GetSquaredXZDistanceTo_NoRadius(target);
				if (dist > m_meta.Range * m_meta.Range)
					return AffectDectectResult.OutOfRange;
			}

			return AffectDectectResult.Success;
		}*/
	}
}