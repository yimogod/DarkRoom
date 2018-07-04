using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
    /// <summary>
    /// 以period的间断来修改某个Attribute
    /// 一般比如中毒或者喝血
    /// </summary>
    public class CBuffDotMeta : CBuffMeta
    {
        /// <summary>
        /// buff产生作用的时间间隔.
        /// 如果是0.就持续产生作用(每帧都起作用)
        /// 否则就是间隔产生效果. 比如每1s造成伤害
        /// 跟PeriodicEffect成对出现
        /// </summary>
        public float Period = -1;

        /// <summary>
        /// 间隔产生的具体效果. 比如没1s增加10%的血量
        /// 跟Period成对出现
        /// </summary>
        public string PeriodicEffect;

        /// <summary>
        /// 刚附着本buff时是否执行PeriodicEffect
        /// </summary>
        public bool ExecutePeriodicEffectOnApply = true;

        /// <summary>
        /// 伤害反映
        /// </summary>
        public CBuffStatusMeta.DamageResponse OnDamage;

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


        public CBuffDotMeta(string idKey) : base(idKey)
        {
        }
    }
}