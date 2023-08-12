namespace Proxy.Security
{
	internal interface ISignature
	{
		byte[] Sign(byte[] data);

		bool IsValid(byte[] data);

		byte[] RemoveSignature(byte[] data);
	}
}
