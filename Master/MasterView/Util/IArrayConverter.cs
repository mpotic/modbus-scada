namespace MasterView.Util
{
	interface IArrayConverter
	{
		ushort[] ConvertStringToUshortArray(string numbers);
		
		byte[] ConvertStringToByteArray(string numbers);
	}
}
