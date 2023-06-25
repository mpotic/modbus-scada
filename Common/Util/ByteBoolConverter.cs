using System.Linq;

namespace Common.Util
{
	public class ByteBoolConverter
	{
		public bool[] ByteArrayToBoolArray(byte[] byteArray)
		{
			bool[] boolArray = byteArray.Select(x => x == 0 ? false : true).ToArray();

			return boolArray;
		}

		public byte[] BoolArrayToByteArray(bool[] boolArray)
		{
			byte[] byteArray = boolArray.Select(x => x ? (byte)1 : (byte)0).ToArray();

			return byteArray;
		}
	}
}
