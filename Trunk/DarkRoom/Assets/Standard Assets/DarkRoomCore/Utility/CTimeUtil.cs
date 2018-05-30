using System;

namespace DarkRoom.Core
{
	public class CTimeUtil{
		//服务器与客户端之间时间差(s)
		private static long m_timeDiff = 0;

		/// <summary>
		/// 设置当前的服务器时间
		/// </summary>
		/// <param name="serverTimeStamp"></param>
		public static void SetServerTimeStamp(long serverTimeStamp){
			long clientNow = GetCurrentTimeStamp();
			m_timeDiff = serverTimeStamp - clientNow;
		}

		/// <summary>
		/// 客户端当前时间到1970年的秒数
		/// </summary>
		public static long GetCurrentTimeStamp(){
			TimeSpan span= DateTime.UtcNow.Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc));
			return (long)span.TotalSeconds;
		}

		/// <summary>
		/// 客户端当前时间到1970年的毫秒
		/// </summary>
		/// <returns></returns>
		public static long GetCurrentMillSecondStamp(){
			TimeSpan span= DateTime.UtcNow.Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc));
			return (long)span.TotalMilliseconds;
		}

		/// <summary>
		/// 根据服务器和客户端的时间差值, 计算出当前的服务器时间
		/// </summary>
		public static long GetServerTimeStampNow(){
			long clientNow = GetCurrentTimeStamp();
			return clientNow + m_timeDiff;
		}

		/// <summary>
		/// 客户端的时间是否到了desireTime
		/// </summary>
		public static bool HasClientArrivedTimeStampInMillSecond(long desireTime){
			long clientNow = GetCurrentMillSecondStamp();
			return clientNow >= desireTime;
		}
	}
}