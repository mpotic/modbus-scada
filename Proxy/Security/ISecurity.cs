namespace Proxy.Security
{
	internal interface ISecurity
	{
		void Secure(byte[] message);

		void Validate(byte[] message);
	}
}
