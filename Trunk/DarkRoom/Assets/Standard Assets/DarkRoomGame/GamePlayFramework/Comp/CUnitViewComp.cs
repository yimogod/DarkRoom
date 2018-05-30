using System;
using UnityEngine;

namespace DarkRoom.Game
{
	/// <summary>
	/// 单位的视图. 可以播放动画和执行回调
	/// </summary>
	public class CUnitViewComp : MonoBehaviour
	{
		/// <summary>
		/// TODO 是否删除再实际应用中看
		/// </summary>
		public const string ACTOR_VIEW_ROOT = "view_root";

		/// <summary>
		/// 本单位播放动画涉及到的clip名
		/// 全部是静态变量. 为了适应不同的项目. 这里设置不设为常量.
		/// 可以在外部进行改变
		/// </summary>
		public static string CLIP_IDLE = "idle";

		/// <summary>
		/// 走
		/// </summary>
		public static string CLIP_WALK = "walk";
		/// <summary>
		/// 跑
		/// </summary>
		public static string CLIP_RUN = "run";
		/// <summary>
		/// 冲刺
		/// </summary>
		public static string CLIP_SPRINT = "sprint";
		/// <summary>
		/// 默认普通动作
		/// </summary>
		public static string CLIP_ATK = "attack";
		/// <summary>
		/// 没有武器时的拳击或者脚踢
		/// </summary>
		public static string CLIP_KICK = "kick";
		/// <summary>
		/// 技能前摇
		/// </summary>
		public static string CLIP_CAST = "cast";
		/// <summary>
		/// 技能中的动作, 比如位移技能边冲刺边挥刀
		/// </summary>
		public static string CLIP_IN_ABILITY = "in_ability";
		/// <summary>
		/// 被击飞
		/// </summary>
		public static string CLIP_HIT_FLY = "hit_fly";
		/// <summary>
		/// 死亡动画
		/// </summary>
		public static string CLIP_DIE = "die";

		private Action m_onComplete = null;
		private Action m_onTrigger = null;

		private ICanPlayClip m_anim = null;

		/// <summary>
		/// 是否正在播放一次性动画, 非loop
		/// </summary>
		public bool PlayingOnceClip {
			get { return m_anim.PlayingOnceClip; }
		}

		/// <summary>
		/// 当前正在播放的clip
		/// </summary>
		public string AnimClip{
			get { return m_anim.CurrClip; }
		}

		/// <summary>
		/// 注册播放动画的实际代理
		/// </summary>
		public void RegisterClipPlayComp(ICanPlayClip anim){
			m_anim = anim;
		}

		/// <summary>
		/// 仅仅播放动画
		/// </summary>
		public void PlayClip(string clip, float normalizedTime = 0)
		{
			if (m_anim == null)return;
			//如果动画一样, 且是循环播放的动画, 就不与处理
			if (string.Equals(m_anim.CurrClip, clip) && !PlayingOnceClip) {
				return;
			}

			m_anim.PlayClip(clip, normalizedTime);
        }

		/// <summary>
		/// 当前是否正在播放clip
		/// </summary>
		public bool IsPlayClip(string clip){
            return string.Equals(m_anim.CurrClip, clip);
		}

		/// <summary>
		/// 暂停动画播放
		/// </summary>
		public void Pause()
		{
			if (m_anim != null) m_anim.Pause();
		}

		/// <summary>
		/// 恢复播放现场
		/// </summary>
		public void Resume()
		{
			if (m_anim != null)m_anim.Resume();
		}

		private void InvokeTrigger()
		{
			if (m_onTrigger == null) return;

			m_onTrigger();
			m_onTrigger = null;
		}

		private void InvokeComplete()
		{
			if (m_onComplete == null) return;

			m_onComplete();
			m_onComplete = null;
		}

		void OnDestroy()
		{
			m_anim = null;

			m_onTrigger = null;
			m_onComplete = null;
		}
	}
}