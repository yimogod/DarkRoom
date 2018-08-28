using System;
using UnityEngine;

namespace ES3Types
{
	[ES3PropertiesAttribute("Name", "Level", "Class", "Race")]
	public class ES3Type_CharacterVO : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3Type_CharacterVO() : base(typeof(Sword.CharacterVO)){ Instance = this; }

		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Sword.CharacterVO)obj;
			
			writer.WriteProperty("Name", instance.Name, ES3Type_string.Instance);
			writer.WriteProperty("Level", instance.Level, ES3Type_int.Instance);
			writer.WriteProperty("Class", instance.Class, ES3Type_int.Instance);
			writer.WriteProperty("Race", instance.Race, ES3Type_int.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Sword.CharacterVO)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Name":
						instance.Name = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "Level":
						instance.Level = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Class":
						instance.Class = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Race":
						instance.Race = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Sword.CharacterVO();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}

	public class ES3Type_CharacterVOArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_CharacterVOArray() : base(typeof(Sword.CharacterVO[]), ES3Type_CharacterVO.Instance)
		{
			Instance = this;
		}
	}
}