namespace Common.Util
{
    public interface IByteArrayConverter
    {
        bool[] ConvertToBoolArray(byte[] byteArray);

        byte[] ConvertToByteArray(bool[] boolArray);

        ushort[] ConvertToUshortArray(byte[] byteArray);

        byte[] ConvertToByteArray(ushort[] ushortArray);
    }
}
