using System;

namespace Common.Util
{
	public class ResponseValuesFormatter : IResponseValuesFormatter
	{
		public string FormatArray(Array array, ushort startAddress)
		{
			string formatedText = "";

			for(int i = 0; i < array.Length; i++)
			{
				formatedText += $"Register[{startAddress + i}]  =  {array.GetValue(i)}\n";
			}

			return formatedText;
		}
	}
}
