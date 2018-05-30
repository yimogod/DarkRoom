using System;
namespace DarkRoom.Core{
	/// <summary>
	/// when data source is big endian, use this instead of BitConverter
	/// cause c# is little endian
	/// </summary>
	public class CBitConvertHelper{
		public byte[] ShortBuff = new byte[2];
		public byte[] IntBuff = new byte[4];
		public byte[] LongBuff = new byte[8];

		public byte[] FloatBuff = new byte[4];
		public byte[] DoubleBuff = new byte[8];

		public short ToInt16(byte[] bytes, int startIndex){
			for(int i = 0; i < CBitStream.SHORT16_LEN; i++){
				ShortBuff[i] = bytes[startIndex + i];
			}
			Array.Reverse(ShortBuff);

			return BitConverter.ToInt16(ShortBuff, 0);
		}

		public int ToInt32(byte[] bytes, int startIndex){
			for(int i = 0; i < CBitStream.INT32_LEN; i++){
				IntBuff[i] = bytes[startIndex + i];
			}
			Array.Reverse(IntBuff);

			return BitConverter.ToInt32(IntBuff, 0);
		}

		public long ToInt64(byte[] bytes, int startIndex){
			for(int i = 0; i < CBitStream.LONG_LEN; i++){
				LongBuff[i] = bytes[startIndex + i];
			}
			Array.Reverse(LongBuff);
			
			return BitConverter.ToInt64(LongBuff, 0);
		}

		public float ToSingle(byte[] bytes, int startIndex){
			for(int i = 0; i < CBitStream.FLOAT_LEN; i++){
				FloatBuff[i] = bytes[startIndex + i];
			}
			Array.Reverse(FloatBuff);
			
			return BitConverter.ToSingle(FloatBuff, 0);
		}

		public double ToDouble(byte[] bytes, int startIndex){
			for(int i = 0; i < CBitStream.DOUBLE_LEN; i++){
				DoubleBuff[i] = bytes[startIndex + i];
			}
			Array.Reverse(DoubleBuff);
			
			return BitConverter.ToDouble(DoubleBuff, 0);
		}

	}
}