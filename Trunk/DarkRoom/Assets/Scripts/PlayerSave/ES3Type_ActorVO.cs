using System;
using UnityEngine;

namespace ES3Types
{
	[ES3PropertiesAttribute("ATK")]
	public class ES3Type_ActorVO : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3Type_ActorVO() : base(typeof(Sword.ActorVO)){ Instance = this; }

		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Sword.ActorVO)obj;
			
			writer.WriteProperty("ATK", instance.ATK, ES3Type_int.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Sword.ActorVO)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "ATK":
						instance.ATK = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Sword.ActorVO();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}

	public class ES3Type_ActorVOArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_ActorVOArray() : base(typeof(Sword.ActorVO[]), ES3Type_ActorVO.Instance)
		{
			Instance = this;
		}
	}
}