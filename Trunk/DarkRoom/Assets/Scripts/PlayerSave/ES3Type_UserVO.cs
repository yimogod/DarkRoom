using System;
using UnityEngine;

namespace ES3Types
{
	[ES3PropertiesAttribute("CurrentCharacterName", "CharacterNameList")]
	public class ES3Type_UserVO : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3Type_UserVO() : base(typeof(Sword.UserVO)){ Instance = this; }

		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Sword.UserVO)obj;
			
			writer.WriteProperty("CurrentCharacterName", instance.CurrentCharacterName, ES3Type_string.Instance);
			writer.WriteProperty("CharacterNameList", instance.CharacterNameList);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Sword.UserVO)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "CurrentCharacterName":
						instance.CurrentCharacterName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "CharacterNameList":
						instance.CharacterNameList = reader.Read<System.Collections.Generic.List<System.String>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Sword.UserVO();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}

	public class ES3Type_UserVOArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_UserVOArray() : base(typeof(Sword.UserVO[]), ES3Type_UserVO.Instance)
		{
			Instance = this;
		}
	}
}