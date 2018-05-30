using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Core;
using DarkRoom.Game;

namespace DarkRoom.AI {
	/// <summary>
	/// 如果之前看见过对手, 就记住这个人, 未来后期会看到人立刻通知同伴
	/// </summary>
	public class CAISense_Sight : CAISense {
		private CTimeRegulator m_regulator;

		/*get the unit which the owner know it*/
		private List<CAIController> m_recentOpponents = new List<CAIController>();

		public CAISense_Sight() : base() {
			m_regulator = new CTimeRegulator(0.1f);
		}

		public override void Update(List<CAIController> stimusSrcList) {
			base.Update(stimusSrcList);

			bool isReady = m_regulator.Update();
			if (!isReady) return;

			for (int i = 0; i < stimusSrcList.Count; ++i) {
				CAIController enemy = stimusSrcList[i];
				if (enemy.Pawn.DeadOrDyingOrInvalid) continue;
				bool isEnemy = Owner.Pawn.IsEnemyTeam(enemy.Pawn);
				if (!isEnemy) continue;

				bool canSee = Owner.LineOfSightTo(enemy, Vector3.zero);
                if (!canSee)continue;

				//如果能够看见
				CAIStimulus stimu = new CAIStimulus();
				stimu.Sense = this;
				stimu.Source = Owner;
				stimu.Expired = false;
				stimu.Source = enemy;
				stimu.TimeBecameVisible = Time.realtimeSinceStartup;
				stimu.TimeLastSensed = Time.realtimeSinceStartup;
				stimu.StimulusLocation = enemy.LocalPosition;
				stimu.ReceiverLocation = Owner.LocalPosition;

				AddOrUpdateStimulu(stimu);
				return;
			}

		}


		/// <summary>
		/// 留在记忆中的人, 看见时间太长, 在原地的概率就很小了, 我们就不予考虑了
		/// </summary>
		/// <returns></returns>
		public List<CAIController> GetListOfRecentlySensedOpponents() {
			m_recentOpponents.Clear();

			foreach (var item in m_stimuDict) {
				CAIStimulus record = item.Value;
				if (record.Invalid) continue;
				m_recentOpponents.Add(record.Source);
			}

			return m_recentOpponents;
		}

	}
}
