namespace ModbusView.Util
{
	class ArrayConverter : IArrayConverter
	{
		public ushort[] ConvertStringToUshortArray(string numbers)
		{
			string[] numberStrings = numbers.Split(',');
			ushort[] result = new ushort[numberStrings.Length];

			for (int i = 0; i < numberStrings.Length; i++)
			{
				result[i] = ushort.Parse(numberStrings[i]);
			}

			return result;
		}

		public byte[] ConvertStringToByteArray(string numbers)
		{
			string[] numberStrings = numbers.Split(',');
			byte[] result = new byte[numberStrings.Length];

			for (int i = 0; i < numberStrings.Length; i++)
			{
				result[i] = byte.Parse(numberStrings[i]);
			}

			return result;
		}
	}
}
