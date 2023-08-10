namespace Proxy.Security
{
	internal interface IEncryption
	{
		void Encrypt(ref byte[] plaintext);

		void Decrypt(ref byte[] ciphertext);
	}
}
