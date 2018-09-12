using System;
using UnityEngine;

namespace ES3Types
{
	[ES3PropertiesAttribute("AttributePoint", "SkillPoint", "Strength", "Dexterity", "Constitution", "Magic", "Willpower", "Cunning", "Luck", "Name", "Level", "Class", "Race", "Address")]
	public class ES3Type_HeroVO : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3Type_HeroVO() : base(typeof(Sword.HeroVO)){ Instance = this; }

		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Sword.HeroVO)obj;
			
			writer.WriteProperty("AttributePoint", instance.AttributePoint, ES3Type_int.Instance);
			writer.WriteProperty("SkillPoint", instance.SkillPoint, ES3Type_int.Instance);
			writer.WriteProperty("Strength", instance.Strength, ES3Type_int.Instance);
			writer.WriteProperty("Dexterity", instance.Dexterity, ES3Type_int.Instance);
			writer.WriteProperty("Constitution", instance.Constitution, ES3Type_int.Instance);
			writer.WriteProperty("Magic", instance.Magic, ES3Type_int.Instance);
			writer.WriteProperty("Willpower", instance.Willpower, ES3Type_int.Instance);
			writer.WriteProperty("Cunning", instance.Cunning, ES3Type_int.Instance);
			writer.WriteProperty("Luck", instance.Luck, ES3Type_int.Instance);
			writer.WriteProperty("Name", instance.Name, ES3Type_string.Instance);
			writer.WriteProperty("Level", instance.Level, ES3Type_int.Instance);
			writer.WriteProperty("Class", instance.Class, ES3Type_int.Instance);
			writer.WriteProperty("Race", instance.Race, ES3Type_int.Instance);
			writer.WriteProperty("Address", instance.Address, ES3Type_string.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Sword.HeroVO)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "AttributePoint":
						instance.AttributePoint = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "SkillPoint":
						instance.SkillPoint = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Strength":
						instance.Strength = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Dexterity":
						instance.Dexterity = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Constitution":
						instance.Constitution = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Magic":
						instance.Magic = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Willpower":
						instance.Willpower = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Cunning":
						instance.Cunning = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Luck":
						instance.Luck = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
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
					case "Address":
						instance.Address = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Sword.HeroVO();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}

	public class ES3Type_HeroVOArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_HeroVOArray() : base(typeof(Sword.HeroVO[]), ES3Type_HeroVO.Instance)
		{
			Instance = this;
		}
	}
}