using System;
using System.Collections.Generic;
using PureMVC.Patterns;
using PureMVC.Interfaces;

namespace Sword
{
	public class UserProxy : Proxy, IProxy
	{
		public const string NAME = "UserProxy";

		public static string GetCharacterSlot(string name)
		{
			return $"char_{name}";
		}

		public UserVO User = null;

		/// <summary>
		/// 玩家当前或者说上一局玩的角色
		/// </summary>
		public CharacterVO Character = null;

		/// <summary>
		/// 玩家创建所有的角色
		/// </summary>
		public List<CharacterVO> CharacterList = new List<CharacterVO>();

		public UserProxy() : base(NAME)
		{
		}

		/// <summary>
		/// 保存角色到数据库
		/// </summary>
		public void SaveCharacter()
		{
			var slot = GetCharacterSlot(Character.Name);
			ES3.Save<CharacterVO>(slot, Character);
		}

		/// <summary>
		/// 读取角色
		/// </summary>
		public static CharacterVO Load(string name)
		{
			//如果name不合法, 那就取一个
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}


			var slot = GetCharacterSlot(name);
			CharacterVO vo = null;

			if (ES3.KeyExists(slot))
			{
				vo = ES3.Load<CharacterVO>(slot);
			}

			return vo;
		}

		public void CreateCharacter(string name, int metaClass, int race)
		{
			CharacterVO vo = new CharacterVO
			{
				Name = name,
				Class = metaClass,
				Race = race,
				Level = 1,
			};
			Character = vo;
			CharacterList.Add(vo);
			SaveCharacter();

			User.CurrentCharacterName = vo.Name;
			User.CharacterNameList.Add(vo.Name);
			User.Save();
		}

		public bool HasEnoughCoin(int coin)
		{
			return User.coin >= coin;
		}

		public bool HasEnoughGold(int gold)
		{
			return User.gold >= gold;
		}

		public void AddCoin(int value)
		{
			User.coin += value;
		}

		//消费
		public void ConsumeCoin(int value)
		{
			User.coin -= value;
			if (User.coin < 0) User.coin = 0;
		}
	}
}