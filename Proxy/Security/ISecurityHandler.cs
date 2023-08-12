namespace Proxy.Security
{
	internal interface ISecurityHandler
	{
		byte[] Secure(byte[] message);

		byte[] Validate(byte[] message);

		void ConfigureEncryption(EncryptionTypeCode encryptionType);
	}
}
