using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	public class CBuffStatusMeta : CBuffMeta {
		/// <summary>
		/// 效果产生作用的前置条件
		/// </summary>
		public List<string> Requirements;

		/// <summary>
		/// 该buff可以在同一个单位身上存在几个实例
		/// 比如英雄联盟的血瓶, 可以吃多个, 加快回血速度
		/// </summary>
		public int MaxStackCount;

		/// <summary>
		/// buff时长
		/// 小于0表明永远有效果, 除非角色死亡
		/// 等于0表明立刻销毁
		/// </summary>
		public float Duration = 0f;

		/// <summary>
		/// buff产生作用的时间间隔.
		/// 如果是0.就持续产生作用(每帧都起作用)
		/// 如果是小于0, 则表明没有效果产生. 可能仅仅是修改state
		/// 否则就是间隔产生效果. 比如没1s造成伤害
		/// 跟PeriodicEffect成对出现
		/// </summary>
		public float Period = -1;

		/// <summary>
		/// 间隔产生的具体效果. 比如没1s增加10%的血量
		/// 跟Period成对出现
		/// </summary>
		public string PeriodicEffect;


		/// <summary>
		/// buff初始化时的效果
		/// </summary>
		public string InitialEffect;

		/// <summary>
		/// buff完成时的效果
		/// </summary>
		public string FinalEffect;

		/// <summary>
		/// buff失效时产生的效果
		/// </summary>
		public string ExpireEffect;

		/// <summary>
		/// 伤害反映
		/// </summary>
		public DamageResponse OnDamage;

		/// <summary>
		/// 本buff会修改的flag值
		/// 星际2编辑器内置的flag有
		/// 暴漏, 被侦查, 调整移动, 共享视野, 关闭技能, 禁用武器, 开启攻击, 开启移动
		/// 可折跃, 抑制移动, 更改ui, 指令不可打断
		/// TODO 回头用枚举来替代, 并且弄明白在星际里的作用
		/// </summary>
		public Dictionary<string, int> ModifyFlags = new Dictionary<string, int>();

		/// <summary>
		/// 更改单位自身的时间流速
		/// </summary>
		public float TimeScale = 1f;

		/// <summary>
		/// 状态标记. 没明白为何星际2分开了不同的flag
		/// 内置的状态state有
		/// 暴漏进程, 被动, 不可拖动, 不可选择, 不可阻止, 不可作为目标, 沉默
		/// 单位属性修改--轻甲, 重甲, 生物, 机械, 灵能, 建筑, 英雄
		/// 不让攻击, 不让隐形, 不让碰撞, 不让战斗, 不让侦测, 不让提供/使用人口
		/// 不让乘客, 不让有视野. 
		/// </summary>
		public Dictionary<CPawnVO.State, int> StateFlags = new Dictionary<CPawnVO.State, int>();

		/// <summary>
		/// 禁用技能类别, 比如目标技能, 普通攻击, 采集, 合体, 变形, 建造
		/// </summary>
		public List<CAbilityMeta.AbilityType> AbilClassDisableList = 
			new List<CAbilityMeta.AbilityType>();

		/// <summary>
		/// buff修改的单位属性
		/// </summary>
		public CAbilityEnum.BuffModification ModifyProperty = 
			new CAbilityEnum.BuffModification();

		public CBuffStatusMeta(string idKey) : base(idKey) {}

		/// <summary>
		/// 控制有该buff的单位受到伤害时会如何反映的参数
		/// 比如无敌, 最小伤害就为0
		/// </summary>
		public class DamageResponse {
			/// <summary>
			/// response 起作用的几率
			/// 0代表完全不会起作用
			/// 1代表完全起作用
			/// </summary>
			public float Chance = 1f;


			public float ModifyFraction = 0f;

			/// <summary>
			/// 最小伤害
			/// </summary>
			public float ModifyMinimumDamage = 1f;

			/// <summary>
			/// 是否闪避
			/// </summary>
			public bool Evade = false;

			/// <summary>
			/// 给攻击者的效果
			/// 比如荆棘护甲的反弹伤害
			/// </summary>
			public string EffectToInstigator;
		}
	}
}