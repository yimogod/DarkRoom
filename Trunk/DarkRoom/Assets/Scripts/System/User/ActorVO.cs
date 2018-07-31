using System;
using System.Collections.Generic;

namespace Sword{

	/// <summary>
	/// mvc中的数据来源
	/// 由controller根据各种情况改变各种数据
	/// </summary>
	public class ActorVO
	{
		/// <summary>
		/// 掌握的技能列表, 除了出生的数据, 也包含后天学习的
		/// </summary>
		public List<int> AbilityIdList = new List<int>();

		/// <summary>
		/// 掌握的技能的等级字典
		/// key是ability id, value是级别
		/// </summary>
		public Dictionary<int, int> AbilityLvDict = new Dictionary<int, int>();

		//角色元数据
		protected ActorMeta m_metaBase;
		protected int m_hp = 1000000;
		protected int m_maxHp = 1000000; //设置个默认最大值
		protected int m_mp;
		protected int m_maxMp;
		protected int m_lv;
		//物理攻击和防御
		protected int m_physicAtk;
		protected int m_physicDef;
		//法术伤害和防御
		protected int m_abilityAtk;
		protected int m_abilityDef;

		protected float m_speedBase;

		public ActorVO()
		{
		}

		public ActorVO(ActorMeta meta)
		{
			m_metaBase = meta;
		}

		/// <summary>
		/// 角色元数据基类
		/// </summary>
		public ActorMeta MetaBase{
			get{ return m_metaBase; }
		}

		/// <summary>
		/// 当期武器的攻击距离
		/// </summary>
		public virtual float AtkRange
		{
		    get { return 10.0f; }
		}

		/// <summary>
		/// 血量小于0就是死亡
		/// </summary>
		public bool Dead{
			get{ return HP <= 0; }
		}

		/// <summary>
		/// 获取和设置物理伤害
		/// </summary>
		public int ATK{
			get{ return m_physicAtk; }
			set { m_physicAtk = value; }
		}
		
		/// <summary>
		/// 获取和设置物理防御
		/// </summary>
		public int DEF{
			get { return m_physicDef; }
			set { m_physicDef = value; }
		}

		/// <summary>
		/// 法伤 ability power
		/// </summary>
		public int AP {
			get { return m_abilityAtk; }
			set { m_abilityAtk = value; }
		}

		/// <summary>
		/// 法防
		/// </summary>
		public int AD {
			get { return m_abilityDef; }
			set { m_abilityDef = value; }
		}

		public int CRIT_RATE{
			get { return 10; }
		}


		/// <summary>
		/// 人物的基础速度
		/// </summary>
		public float SpeedBase {
			get { return m_speedBase; }
			set{
				m_speedBase = value;
			}
		}

		public int HP{
			get{ return m_hp; }
			set{
				m_hp = value;
				if(m_hp < 0)m_hp = 0;
				if(m_hp > MaxHP)m_hp = MaxHP;
			}
		}

		/// <summary>
		/// 单位最大血量
		/// </summary>
		public int MaxHP{
			get
			{
				return m_maxHp;
			}
			set { m_maxHp = value; }
		}

		public bool HpFull{
			get{ return m_hp >= MaxHP; }
		}

		public void FullHp(){
			HP = MaxHP;
		}

		public void AddHp(int data){
			HP += data;
		}

		//data between 0~1
		public void AddHpPercent(float data){
			HP += (int)(MaxHP * data);
		}

		public int MP{
			get{ return m_mp; }
			set{
				m_mp = value;
				if (m_mp <= 0)m_mp = 0;
			}
		}

		public int MaxMP{
			get { return 100; }
		}

		public bool MpFull{
			get{ return m_mp >= MaxMP; }
		}

		public void FullMp(){
			MP = MaxMP;
		}

		public void AddMP(int data){
			MP += data;
		}

		//data between 0~1
		public void AddMpPercent(float data){
			HP += (int)(MaxMP * data);
		}

		public int LV{
			get { return m_lv; }
		}

	    public void SetMeta(ActorMeta meta)
	    {
	        m_metaBase = meta;
	    }
	}
}