namespace Proxy.Security
{
	internal interface ISignature
	{
		void Sign(ref byte[] data);

		bool Validate(byte[] data);

		void RemoveSignature(ref byte[] data);
	}
}
