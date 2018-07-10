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
		/// 速度修改通知其他依赖的对象. 比如移动组件
		/// </summary>
		public Action OnNotiSpeedChange;

		/// <summary>
		/// 属性字段
		/// </summary>
		public enum Property
		{
			SpeedBonus, //速度
			SpeedMultiplier, //速度
			HPBonus, //血
			MaxHPBonus,
			MPBonus, //蓝
			MaxMPBonus,
			ATK, //攻击
			DEF, //防守
			CRIT_RATE, //暴击率
		}

		/// <summary>
		/// buff修改单位的状态值
		/// </summary>
		public enum State {
			Stun, //击退然后昏迷
			Daze, //晕眩 TODO 看两者的区别
			HitFly, //击飞
			SuppressItemUsage, //禁止使用物品
			Cloak, //隐身
			SuppressCollision, //无视碰撞
			SuppressDamageVisibilityAttacker, //禁止非隐身单位的攻击 ??????
			Undetectable, //不可侦测
			Summoned, //成为召唤单位/属性
			Silence, //沉默
			Uncommandable, //无法给于指令
			SuppressAttack, //禁止攻击
			Unstoppable, //不可停止 ???
			Invulnerable, //无敌
			Benign, // 仁慈的；温和的；良性
			Stasis, //停滞
			Untargetable, //不可成为目标
		}

		//主武器
		public int PrimaryWeaponId = 0;

		/// <summary>
		/// 掌握的技能列表, 除了出生的数据, 也包含后天学习的
		/// </summary>
		public List<int> AbilityIdList = new List<int>();

		/// <summary>
		/// 掌握的技能的等级字典
		/// key是ability id, value是级别
		/// </summary>
		public Dictionary<int, int> AbilityLvDict = new Dictionary<int, int>();

		/// <summary>
		/// 因为buff造成的各种属性的修改, 每帧都会重新取值
		/// 字典的key代表buff自己的cid
		/// KeyValuePair key代表 buff的property, value代表 buff修改的值
		/// </summary>
		public Dictionary<int, Dictionary<Property, float>> PropertyModifier = 
			new Dictionary<int, Dictionary<Property, float>>();

		/// <summary>
		/// 技能对状态造成的修改
		/// value是state结束的时间, 毫秒数和state的buff 的cid
		/// </summary>
		public Dictionary<State, KeyValuePair<int, long>> StateModifier =
			new Dictionary<State, KeyValuePair<int, long>>();

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
			PrimaryWeaponId = meta.InitWeapon;
		}

		/// <summary>
		/// 角色元数据基类
		/// </summary>
		public ActorMeta MetaBase{
			get{ return m_metaBase; }
		}

		/// <summary>
		/// 我的视野角度
		/// </summary>
		public virtual float ViewAngle{
			get { return m_metaBase.ViewAngle; }
		}

		/// <summary>
		/// 视野距离范围
		/// </summary>
		public virtual float ViewRange
		{
			get { return m_metaBase.ViewRange; }
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
		/// 行走速度. 其他组件依赖于这个速度值
		/// 作为数据的存储, 移动组件会从这里读取速度
		/// </summary>
		public float Speed{
			get{
				if (PropertyModifier.Count == 0)return SpeedBase;

				float b = 0;
				float m = 0;
				int mulNum = 0;
				foreach (var buff in PropertyModifier) {
					foreach (var kv in buff.Value) {
						if (kv.Key == Property.SpeedBonus) {
							b += kv.Value;
						}

						if (kv.Key == Property.SpeedMultiplier) {
							m += kv.Value;
							mulNum++;
						}
					}
				}

				if (mulNum == 0) {
					m = 1f;
				} else {
					m /= mulNum;
				}

				return b  + SpeedBase * m;
			}
		}

		/// <summary>
		/// 人物的基础速度
		/// </summary>
		public float SpeedBase {
			get { return m_speedBase; }
			set{
				m_speedBase = value;
				NotiSpeedChange();
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

		public void NotiSpeedChange()
		{
			if (OnNotiSpeedChange != null) {
				OnNotiSpeedChange();
			}
		}
	}

}