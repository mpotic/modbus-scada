namespace Proxy.Security
{
	internal interface ISignature
	{
		byte[] SignData(byte[] data);

		bool IsSignatureValid(byte[] originalData, byte[] signature);

		byte[] GetOriginalData(byte[] data);

		byte[] GetSignatureFromSignedData(byte[] data);
	}
}
