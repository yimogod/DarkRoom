using System;
using System.Text;
using UnityEngine;

namespace DarkRoom.Core {
	public class CBitStream{
		public const int BYTE_LEN = 1;
		public const int SHORT16_LEN = 2;
		public const int INT32_LEN = 4;
		public const int FLOAT_LEN = 4;
		public const int LONG_LEN = 8;
		public const int DOUBLE_LEN = 8;

		//二进制的原始数据
		private byte[] m_bytes = null;

		//游标的位置
		private short m_index = 0;

		/* total length without tail 0 */
		private int m_length = 0;

		private CBitConvertHelper m_bitHelper;

		/// <summary>
		/// 二进制流的读取
		/// </summary>
		/// <param name="mBytes">二进制的原始数据</param>
		/// <param name="littleEndian">是否小端</param>
		public CBitStream(byte[] mBytes, bool littleEndian){
			m_index = 0;
			m_bytes = mBytes;
			m_length = m_bytes.Length;

			m_bitHelper = new CBitConvertHelper();
		}

		public byte[] Bytes{
			get{ return m_bytes; }
			set{
				m_bytes = value;
				m_length = m_bytes.Length;
			}
		}

		/* content length for client, 2 bigger then server*/
		public int Length{
			get { return m_length; }
			set { m_length = value; }
		}

		public short Index{
			get { return m_index;}
		}

		public void Reset(){
			m_index = 0;
		}

		public void WriteByte(byte bt){
			if (m_index + BYTE_LEN > m_length){
				Debug.LogError("NetBitStream <WriteByte> out of range");
				return;
			}
	    
			m_bytes[m_index] = bt;
			m_index += BYTE_LEN;
		}

		public void WriteBool(bool flag){
			if (m_index + BYTE_LEN > m_length){
				Debug.LogError("NetBitStream <WriteBool> out of range");
				return;
			}

			byte b = (byte)0;
			if (flag)b = (byte)1;

			m_bytes[m_index] = b;
			m_index += BYTE_LEN;
		}

		public void WriteInt(int number){
			if (m_index + INT32_LEN > m_length){
				Debug.LogError("NetBitStream <WriteInt> out of range");
				return;
			}

			byte[] bs = BitConverter.GetBytes(number);
			Array.Reverse(bs);

			bs.CopyTo(m_bytes, m_index);
			m_index += INT32_LEN;
		}

		public void WriteLong(long number){
			if (m_index + LONG_LEN > m_length){
				Debug.LogError("NetBitStream <WriteLong> out of range");
				return;
			}
		
			byte[] bs = BitConverter.GetBytes(number);
			Array.Reverse(bs);

			bs.CopyTo(m_bytes, m_index);
			m_index += LONG_LEN;
		}

		public void WriteShort(short number){
			if (m_index + SHORT16_LEN > m_length){
				Debug.LogError("NetBitStream <WriteShort> out of range");
				return;
			}

			byte[] bs = BitConverter.GetBytes(number);
			Array.Reverse(bs);

			bs.CopyTo(m_bytes, m_index);
			m_index += SHORT16_LEN;
		}

		public void WriteFloat(float number){
			if (m_index + FLOAT_LEN > m_length){
				Debug.LogError("NetBitStream <WriteFloat> out of range");
				return;
			}

			byte[] bs = BitConverter.GetBytes(number);
			Array.Reverse(bs);

			bs.CopyTo(m_bytes, m_index);
			m_index += FLOAT_LEN;
		}

		public void WriteDouble(double number){
			if (m_index + DOUBLE_LEN > m_length){
				Debug.LogError("NetBitStream <WriteDouble> out of range");
				return;
			}
		
			byte[] bs = BitConverter.GetBytes(number);
			Array.Reverse(bs);

			bs.CopyTo(m_bytes, m_index);
			m_index += DOUBLE_LEN;
		}

		public void WriteString(string str){
			short len = (short)Encoding.UTF8.GetByteCount(str);
			WriteShort(len);

			if (m_index + len > m_length){
				Debug.LogError("NetBitStream <WriteString> out of range");
				return;
			}

			Encoding.UTF8.GetBytes(str, 0, str.Length, m_bytes, m_index);
			m_index += len;
		}

		public byte ReadByte(){
			if (m_index + BYTE_LEN > m_length){
				Debug.LogError("NetBitStream <ReadByte> out of range");
				return 0;
			}

			byte bt = m_bytes[m_index];
			m_index += BYTE_LEN;

			return bt;
		}

		public bool ReadBool(){
			if (m_index + BYTE_LEN > m_length){
				Debug.LogError("NetBitStream <ReadBool> out of range");
				return false;
			}

			byte bt = m_bytes[m_index];
			m_index += BYTE_LEN;

			bool flag = false;
			if( bt == (byte)1)flag = true;
			return flag;
		}


		public int ReadInt(){
			if (m_index + INT32_LEN > m_length){
				Debug.LogError("NetBitStream <ReadInt> out of range with index " + m_index);
				return 0;
			}

			int number = m_bitHelper.ToInt32(m_bytes, m_index);
			m_index += INT32_LEN;

			return number;
		}

		public short ReadShort(){
			if (m_index + SHORT16_LEN > m_length){
				Debug.LogError("NetBitStream <ReadShort> out of range with index " + m_index);
				return 0;
			}

			short number = m_bitHelper.ToInt16(m_bytes, m_index);
			m_index += SHORT16_LEN;

			return number;
		}

		public long ReadLong(){
			if (m_index + LONG_LEN > m_length){
				Debug.LogError("NetBitStream <ReadLong> out of range");
				return 0;
			}

			long number = m_bitHelper.ToInt64(m_bytes, m_index);
			m_index += LONG_LEN;
		
			return number;
		}

		public float ReadFloat(){
			if (m_index + FLOAT_LEN > m_length){
				Debug.LogError("NetBitStream <ReadFloat> out of range");
				return 0;
			}

			float number = m_bitHelper.ToSingle(m_bytes, m_index);
			m_index += FLOAT_LEN;

			return number;
		}

		public double ReadDouble(){
			if (m_index + DOUBLE_LEN > m_length){
				Debug.LogError("NetBitStream <ReadDouble> out of range");
				return 0;
			}

			double number = m_bitHelper.ToDouble(m_bytes, m_index);
			m_index += DOUBLE_LEN;
		
			return number;
		}

		public string ReadString(){
			short len = ReadShort();

			if (m_index + len > m_length){
				Debug.LogError("NetBitStream <ReadString> out of range");
				return "";
			}

			string str = Encoding.UTF8.GetString(m_bytes, m_index, (int)len);
			m_index += len;

			return str;
		}

		public void Dispose(){
			m_bytes = null;
		}
	}

}