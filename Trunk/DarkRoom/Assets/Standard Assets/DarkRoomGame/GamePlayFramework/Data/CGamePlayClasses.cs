using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkRoom.Game
{
	/// <summary>
	/// 移动方式, 是mover的直接移动， 还是由rvo驱动的移动
	/// 对于Unknow(默认值), 的意思是外部操控
	/// </summary>
	public enum MoveType
	{
		Unknow, //外部操控
		Direct, //直接给速度, 然后移动
	}

	/// <summary>
	/// 跟随路径行走完毕的结果
	/// </summary>
	public enum FinishPathResultType
	{
		/** nothing was happened */
		Default,

		/** Reached destination */
		Success,

		/** Movement was blocked */
		Blocked,

		/** Agent is not on path */
		OffPath,

		/** Aborted and stopped (failure) */
		Aborted,

		/** Request was invalid */
		Invalid,
	}

	/// <summary>
	/// 请求跟随路径的结果
	/// </summary>
	public enum RequestPathResultType
	{
		Failed,
		AlreadyAtGoal,
		RequestSuccessful
	}

	public enum PathFollowingStatus
	{
		/** No requests */
		Idle,

		/** Request with incomplete path, will start after UpdateMove() */
		Waiting,

		/** Request paused, will continue after ResumeMove() */
		Paused,

		/** Following path */
		Moving,
	}

}
