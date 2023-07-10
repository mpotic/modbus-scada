using System;
using System.Linq;

namespace Common.Util
{
	public class ByteArrayConverter : IByteArrayConverter
	{
		public bool[] ConvertToBoolArray(byte[] byteArray)
		{
			bool[] boolArray = byteArray.Select(x => x == 0 ? false : true).ToArray();

			return boolArray;
		}

		public byte[] ConvertToByteArray(bool[] boolArray)
		{
			byte[] byteArray = boolArray.Select(x => x ? (byte)1 : (byte)0).ToArray();

			return byteArray;
		}

        public ushort[] ConvertToUshortArray(byte[] byteArray)
        {
            int ushortCount = byteArray.Length / 2;
            ushort[] ushortArray = new ushort[ushortCount];

            for (int i = 0; i < ushortCount; i++)
            {
                ushortArray[i] = BitConverter.ToUInt16(byteArray, i * 2);
            }

            return ushortArray;
        }

		public byte[] ConvertToByteArray(ushort[] ushortArray)
		{
            byte[] byteArray = new byte[ushortArray.Length * 2];
            Buffer.BlockCopy(ushortArray, 0, byteArray, 0, byteArray.Length);

            return byteArray;
        }
    }
}
