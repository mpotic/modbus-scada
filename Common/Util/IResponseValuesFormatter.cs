using System;

namespace Common.Util
{
	public interface IResponseValuesFormatter
	{
		string FormatArray(Array array, ushort startAddress);
	}
}
