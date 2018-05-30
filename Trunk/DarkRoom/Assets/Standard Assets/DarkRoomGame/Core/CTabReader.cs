using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.Game
{
	/// <summary>
	/// 读取二维表格的数据, 列数据直接用tab分开
	/// </summary>
	public class CTabReader {
		public static char[] RowSpliter = new char[] { '\r', '\n' };
		public static char[] ColSpliter = new char[] { '\t' };

		private List<string[]> m_tabValues = new List<string[]>();
		private TabRow m_tabRow = new TabRow();

		//数据的行数和列数
		private int m_row;

		public CTabReader() { }

		public int row {
			get { return m_row; }
		}

		/// <summary>
		/// 解析数据存储到类内部
		/// </summary>
		/// <param name="content">需要解析的内容的字符串</param>
		/// <returns></returns>
		public bool Parse(string content) {
			string[] lines = content.Split(RowSpliter);

			int lineCount = lines.Length;
			if (lineCount <= 4) {
				Debug.LogError("you tab file has no key or desc rows " + lines[0]);
				return false;
			}

			// 第一行是表头, 第二行是解释, 第三行才是开始的数据, 忽略空行和以#注释掉的行
			string line;
			for (int i = 4; i < lineCount; ++i) {
				line = lines[i].Trim();
				if (line.Length == 0) continue;
				m_tabValues.Add(ParseLine(lines[i]));
			}

			m_row = m_tabValues.Count;

			return true;
		}

		/// <summary>
		/// 标记开始读第row行
		/// </summary>
		/// <param name="row">标记的行数</param>
		public void MarkRow(int row) {
			m_tabRow.SetData(m_tabValues[row]);
		}

		public string ReadString() {
			return m_tabRow.ReadString();
		}

		public int ReadInt() {
			return m_tabRow.ReadInt();
		}

		public float ReadFloat() {
			return m_tabRow.ReadFloat();
		}

		public bool ReadBool() {
			return m_tabRow.ReadBool();
		}

		/// <summary>
		/// 清理reader的数据. 当读完后需要清理
		/// </summary>
		public void Close() {
			m_tabValues.Clear();
			m_tabRow.Close();

			m_tabValues = null;
			m_tabRow = null;
		}

		// Excel导出时，对内容中包含逗号时会对该字符串首尾加上双引号，
		// 这里我们需要进行处理，忽略掉多加上的双引号。
		private string[] ParseLine(string line) {
			string[] values = line.Split(ColSpliter);

			for (int i = 0, len = values.Length; i < len; ++i) {
				if (values[i].StartsWith("\"") && values[i].EndsWith("\"")) {
					values[i] = values[i].Substring(1, values[i].Length - 2);
				}
			}

			return values;
		}

	}

	/// <summary>
	/// 对于表格数据每行的抽象
	/// </summary>
	public class TabRow {
		private string[] m_value;
		private int m_index = 0;

		public TabRow() { }

		/// <summary>
		/// 填充tab row的数据. 
		/// </summary>
		/// <param name="value">string数组. 每行的数据由tab分开后形成的数组</param>
		public void SetData(string[] value) {
			m_value = value;
			m_index = 0;
		}

		public string ReadString() {
			string value = m_value[m_index];
			m_index += 1;
			return value;
		}

		public int ReadInt() {
			string value = ReadString();
			return int.Parse(value);
		}

		public float ReadFloat() {
			string value = ReadString();
			return float.Parse(value);
		}

		public bool ReadBool() {
			int value = ReadInt();
			return value > 0;
		}

		public void Close() {
			m_value = null;
		}
	}
}