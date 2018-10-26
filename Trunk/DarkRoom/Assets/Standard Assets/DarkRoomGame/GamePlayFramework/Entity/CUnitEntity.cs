using System;
using UnityEngine;

namespace DarkRoom.Game
{
	/// <summary>
	/// 放置在场景中所有单位的基类, 等同于UE的actor
	/// Unit关联了很多组件. 比如移动和长啥样(也有可能没样子).
	/// 用更泛化抽象的概念来看，小到一个个地上的石头，大到整个世界的运行规则，都是Unit.
	/// 另外Unit会代理很多接口以方便直接调用
	/// 最后, 根据UE, 很多伤害和输入响应的代理都定义在里这里--其中有一些我根据自己的理解放入了Pawn中(不一定对)
	///  NOTE:所以不要直接用CUnitEntity
	/// </summary>
	[RequireComponent(typeof(CUnitSpacialComp))]
	public class CUnitEntity : MonoBehaviour
	{
		private static int UnitCounter = 0;

		//red我方, blue敌方, green为第三方中立友善

		public enum TeamSide
		{
			Red = 1,
			Blue = 2,
			Green = 3,
			Light = 4,
			Dark = 5
		}

		/// <summary>
		/// 我属于哪边?
		/// </summary>
		[HideInInspector, NonSerialized] public TeamSide Team;

		/// <summary>
		/// 我是否会收到伤害. true才会产生伤害事件 (比如 ReceiveDamage() 的调用)
		/// </summary>
		[HideInInspector, NonSerialized] public bool CanBeDamaged = true;

		//unit position info
		protected CUnitSpacialComp m_spacial;

		//该单位是否死亡
		protected bool m_dead = false;

		//该单位是否进入死亡态
		protected bool m_dying = false;

		//该单位是否暂时无效
		protected bool m_invalid = false;

		protected Transform m_tran;

		/// <summary>
		/// 客户端每个单位的唯一id, 也有可能由服务器传入
		/// </summary>
		public int CId { get; set; }

		public CUnitSpacialComp SpacialComp => m_spacial;

		/// <summary>
		/// 本单位的位置. 因为用的地方很多. 所以做了个引用
		/// </summary>
		public Vector3 LocalPosition => m_spacial.localPosition;

		/// <summary>
		/// 主体的半径, 读取的是空间组件的半径
		/// </summary>
		public float Radius => m_spacial.Radius;

		public virtual bool Dead
		{
			get { return m_dead; }
			set { m_dead = value; }
		}

		public virtual bool Dying
		{
			get { return m_dying; }
			set { m_dying = value; }
		}

		/// <summary>
		/// 是否暂时处于无效状态, 我们用这个变量来做一些循环conintue的判断
		/// 仅用来做标记位.
		/// </summary>
		public bool Invalid
		{
			get { return m_invalid; }
			set { m_invalid = value; }
		}

		/// <summary>
		/// 是否濒死或者已经死亡
		/// </summary>
		public bool DeadOrDying => m_dead || m_dying;

		/// <summary>
		/// 是否濒死或者已经死亡或者无效化
		/// </summary>
		public bool DeadOrDyingOrInvalid => m_dead || m_dying || m_invalid;

		//是否友好同盟, 包含自己和中立
		public bool IsFriendTeam(TeamSide value)
		{
			if (Team == TeamSide.Dark && value == TeamSide.Dark)
				return true;

			if (Team == TeamSide.Dark || value == TeamSide.Dark)
				return false;

			if (Team == TeamSide.Light || value == TeamSide.Light)
				return true;

			return Team == value;
		}

		public bool IsFriendTeam(CUnitEntity value)
		{
			return IsFriendTeam(value.Team);
		}


		public bool IsEnemyTeam(TeamSide value)
		{
			if (Team == TeamSide.Dark && value == TeamSide.Dark)
				return false;

			if (Team == TeamSide.Dark || value == TeamSide.Dark)
				return true;

			if (Team == TeamSide.Light || value == TeamSide.Light)
				return false;

			return Team != value;
		}

		public bool IsEnemyTeam(CUnitEntity value)
		{
			return IsEnemyTeam(value.Team);
		}

		protected virtual void Awake()
		{
			UnitCounter++;
			CId = UnitCounter;

			m_tran = transform;
			RegisterAllComponents();
			PostRegisterAllComponents();
		}

		protected virtual void Start()
		{
		}

		protected virtual void Update()
		{
		}

		/// <summary>
		/// 注册所有依赖的component, 在父类的awake中被调用
		/// </summary>
		protected virtual void RegisterAllComponents()
		{
			//初始化CUnitSpacialComp, 在awake中实现是为了防止时序问题
			//给别的类在start中做一些事情留有机会
			m_spacial = GetComponent<CUnitSpacialComp>();
			if (m_spacial == null)
			{
				m_spacial = gameObject.AddComponent<CUnitSpacialComp>();
			}
		}

		/// <summary>
		/// 在注册所有的component后面调用. 时期在Awake中
		/// </summary>
		protected virtual void PostRegisterAllComponents()
		{
		}

		/// <summary>
		/// 角色播放动作, 传入null代表暂停当前的动画播放
		/// </summary>
		public virtual void PlayAction(string action, float normalizedTime = 0)
		{
		}

		public virtual void Die()
		{
			m_dead = true;
		}

		protected virtual void OnDestroy()
		{
			m_spacial = null;
		}
	}
}