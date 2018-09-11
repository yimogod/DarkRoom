using System;
using System.Collections;
using System.Reflection;

namespace DarkRoom.Core
{

	/// <summary>
	/// Helper class for working with 'extended' enums using <see cref="StringValueAttribute"/> attributes.
	/// </summary>
	public class CStringEnum
	{
		private Type m_enumType;
		private static Hashtable m_stringValues = new Hashtable();

		/// <summary>
		/// Creates a new <see cref="CStringEnum"/> instance.
		/// </summary>
		/// <param name="enumType">Enum type.</param>
		public CStringEnum(Type enumType)
		{
			if (!enumType.IsEnum)
				throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", enumType.ToString()));

			m_enumType = enumType;
		}

		/// <summary>
		/// Gets the string value associated with the given enum value.
		/// </summary>
		/// <param name="valueName">Name of the enum value.</param>
		/// <returns>String Value</returns>
		public string GetStringValue(string valueName)
		{
			Enum enumType;
			string stringValue = null;
			try
			{
				enumType = (Enum)Enum.Parse(m_enumType, valueName);
				stringValue = GetStringValue(enumType);
			}
			catch (Exception) { }//Swallow!

			return stringValue;
		}

		/// <summary>
		/// Gets the string values associated with the enum.
		/// </summary>
		/// <returns>String value array</returns>
		public Array GetStringValues()
		{
			ArrayList values = new ArrayList();
			//Look for our string value associated with fields in this enum
			foreach (FieldInfo fi in m_enumType.GetFields())
			{
				//Check for our custom attribute
				StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
				if (attrs.Length > 0)
					values.Add(attrs[0].Value);

			}

			return values.ToArray();
		}

		/// <summary>
		/// Gets the values as a 'bindable' list datasource.
		/// </summary>
		/// <returns>IList for data binding</returns>
		public IList GetListValues()
		{
			Type underlyingType = Enum.GetUnderlyingType(m_enumType);
			ArrayList values = new ArrayList();
			//Look for our string value associated with fields in this enum
			foreach (FieldInfo fi in m_enumType.GetFields())
			{
				//Check for our custom attribute
				StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
				if (attrs.Length > 0)
					values.Add(new DictionaryEntry(Convert.ChangeType(Enum.Parse(m_enumType, fi.Name), underlyingType), attrs[0].Value));

			}

			return values;

		}

		/// <summary>
		/// Return the existence of the given string value within the enum.
		/// </summary>
		/// <param name="stringValue">String value.</param>
		/// <returns>Existence of the string value</returns>
		public bool IsStringDefined(string stringValue)
		{
			return Parse(m_enumType, stringValue) != null;
		}

		/// <summary>
		/// Return the existence of the given string value within the enum.
		/// </summary>
		/// <param name="stringValue">String value.</param>
		/// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
		/// <returns>Existence of the string value</returns>
		public bool IsStringDefined(string stringValue, bool ignoreCase)
		{
			return Parse(m_enumType, stringValue, ignoreCase) != null;
		}

		/// <summary>
		/// Gets the underlying enum type for this instance.
		/// </summary>
		/// <value></value>
		public Type EnumType
		{
			get { return m_enumType; }
		}

		/// <summary>
		/// Gets a string value for a particular enum value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>String Value associated via a <see cref="StringValueAttribute"/> attribute, or null if not found.</returns>
		public static string GetStringValue(Enum value)
		{
			string output = null;
			Type type = value.GetType();

			if (m_stringValues.ContainsKey(value))
				output = (m_stringValues[value] as StringValueAttribute).Value;
			else
			{
				//Look for our 'StringValueAttribute' in the field's custom attributes
				FieldInfo fi = type.GetField(value.ToString());
				StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
				if (attrs.Length > 0)
				{
					m_stringValues.Add(value, attrs[0]);
					output = attrs[0].Value;
				}

			}
			return output;

		}

		/// <summary>
		/// Parses the supplied enum and string value to find an associated enum value (case sensitive).
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="stringValue">String value.</param>
		/// <returns>Enum value associated with the string value, or null if not found.</returns>
		public static object Parse(Type type, string stringValue)
		{
			return Parse(type, stringValue, false);
		}

		/// <summary>
		/// Parses the supplied enum and string value to find an associated enum value.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="stringValue">String value.</param>
		/// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
		/// <returns>Enum value associated with the string value, or null if not found.</returns>
		public static object Parse(Type type, string stringValue, bool ignoreCase)
		{
			object output = null;
			string enumStringValue = null;

			if (!type.IsEnum)
				throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", type.ToString()));

			//Look for our string value associated with fields in this enum
			foreach (FieldInfo fi in type.GetFields())
			{
				//Check for our custom attribute
				StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
				if (attrs.Length > 0)
					enumStringValue = attrs[0].Value;

				//Check for equality then select actual enum value.
				if (string.Compare(enumStringValue, stringValue, ignoreCase) == 0)
				{
					output = Enum.Parse(type, fi.Name);
					break;
				}
			}

			return output;
		}

		/// <summary>
		/// Return the existence of the given string value within the enum.
		/// </summary>
		/// <param name="stringValue">String value.</param>
		/// <param name="enumType">Type of enum</param>
		/// <returns>Existence of the string value</returns>
		public static bool IsStringDefined(Type enumType, string stringValue)
		{
			return Parse(enumType, stringValue) != null;
		}

		/// <summary>
		/// Return the existence of the given string value within the enum.
		/// </summary>
		/// <param name="stringValue">String value.</param>
		/// <param name="enumType">Type of enum</param>
		/// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
		/// <returns>Existence of the string value</returns>
		public static bool IsStringDefined(Type enumType, string stringValue, bool ignoreCase)
		{
			return Parse(enumType, stringValue, ignoreCase) != null;
		}

	}

	/// <summary>
	/// Simple attribute class for storing String Values
	/// </summary>
	public class StringValueAttribute : Attribute
	{
		private string m_value;
		public string Value => m_value;

		public StringValueAttribute(string value)
		{
			m_value = value;
		}
	}
}