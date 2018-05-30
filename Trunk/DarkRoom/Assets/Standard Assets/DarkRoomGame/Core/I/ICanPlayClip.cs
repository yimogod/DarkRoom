using System;

namespace DarkRoom.Game
{
	/// <summary>
	/// 播放动画组件的接口
	/// </summary>
	public interface ICanPlayClip {
		Action<String> OnTrigger { get; set; }
		Action<String> OnComplete { get; set; }

		string CurrClip { get; }

		bool PlayingOnceClip { get; }

		/// <summary>
		/// 暂停动画播放
		/// </summary>
		void Pause();

		/// <summary>
		/// 恢复播放现场
		/// </summary>
		void Resume();

		/// <summary>
		/// 播放动画
		/// </summary>
		/// <param name="clip"></param>
		void PlayClip(string clip);

		/// <summary>
		/// 从第frame帧开始播放动画
		/// </summary>
		/// <param name="clip"></param>
		void PlayClip(string clip, float normalizedTime);
	}
}