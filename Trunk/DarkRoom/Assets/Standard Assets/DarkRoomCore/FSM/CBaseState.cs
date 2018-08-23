using System;

namespace DarkRoom.Core {
	public abstract class CBaseState {
		public string Name;

		/// <summary>
		/// 用于debug显示的name
		/// </summary>
	    public virtual string DebugName{
		    get { return Name; }
	    }

		protected object m_data;

		public CBaseState(string name) {
			Name = name;
		}

		public CBaseState(string name, object owner) {
			Name = name;
			m_data = owner;
		}

		public virtual void Initialize(object data) {
			m_data = data;
		}

		public virtual void Enter(CStateMachine sm) {

		}

		public virtual void Exit(CStateMachine sm) {

		}

		public virtual void Execute(CStateMachine sm) {

		}

		public virtual void Destroy()
		{
			m_data = null;
		}
	}
}