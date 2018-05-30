using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.AI {
	/// <summary>
	/// 被脑子接收的ai消息, 会被感兴趣的CAIMessageObserver处理
	/// </summary>
	public struct CAIMessage
	{
		/// <summary>
		/// 消息id
		/// </summary>
		public int Id;

		/// <summary>
		/// 消息名称
		/// </summary>
		public string Name;

		/// <summary>
		/// 谁发送的这个消息
		/// </summary>
		public CController Sender;

		/// <summary>
		/// message携带的数据
		/// </summary>
		public Object Data;

		/// <summary>
		/// 标记
		/// </summary>
		public int Flags;

		/// <summary>
		/// Flags = flags
		/// </summary>
		public void SetFlags(int flags) { Flags = flags; }

		/// <summary>
		/// Flags |= flag
		/// </summary>
		public void SetFlag(int flag) { Flags |= flag; }

		/// <summary>
		/// Flags &= ~flag
		/// </summary>
		public void ClearFlag(int flag) { Flags &= ~flag; }

		/// <summary>
		/// (Flags & flag) != 0
		/// </summary>
		/// <returns></returns>
		public bool HasFlag(int flag) { return (Flags & flag) != 0; }

		/// <summary>
		/// 派发消息给target
		/// </summary>
		public static void Send(CAIController target, CAIMessage message)
		{
			target.BrainComp.HandleMessage(message);
		}

		public CAIMessage(int id, string name, CController sender)
		{
			Id = id;
			Name = name;
			Sender = sender;
			Flags = 0;
			Data = null;
		}

		public CAIMessage(int id, string name, CController sender, Object data) {
			Id = id;
			Name = name;
			Sender = sender;
			Flags = 0;
			Data = data;
		}
	}

	/// <summary>
	/// 消息处理器. 处理感兴趣的消息
	/// 感兴趣的规则有两个
	/// 一个是type-string
	/// 第二个可选的id-int
	/// </summary>
	public class CAIMessageObserver
	{
		/// <summary>
		/// 消息类型
		/// </summary>
		public string MessageType;

		private Action<CAIMessage> m_delegate;

		//是否过滤id
		private bool m_filterId = false;
		//处理器关心的id
		private int m_messageId;

		//用来标记是否有作用, 做延迟删除
		private bool m_dead = false;

		public bool Dead{
			get{ return m_dead; }
		}

		public CAIMessageObserver(string type, Action<CAIMessage> action)
		{
			MessageType = type;
			m_delegate = action;
		}

		public CAIMessageObserver(string type, Action<CAIMessage> action, int messageIdCareAbout) {
			MessageType = type;
			m_delegate = action;
			m_filterId = true;
			m_messageId = messageIdCareAbout;
		}

		/// <summary>
		/// 是否是同一个观测器
		/// </summary>
		public bool SameObserver(string type, Action<CAIMessage> action){
			if (!string.Equals(type, MessageType))return false;
			if(action != null && action != m_delegate)return false;
			return true;
		}

		public void OnMessage(CAIMessage Message)
		{
			if (!string.Equals(MessageType, Message.Name)) return;
			if (m_filterId && m_messageId != Message.Id) return;

			m_delegate(Message);
		}

		public void Die(){
			m_dead = true;
		}

		public void Dispose()
		{
			m_delegate = null;
		}
	}
}
