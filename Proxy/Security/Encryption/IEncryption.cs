namespace Proxy.Security
{
	internal interface IEncryption
	{
		/// <summary>
		/// Encrypts the plaintext and returns a byte[] with the encrypted text.
		/// </summary>
		byte[] Encrypt(byte[] plaintext);

		/// <summary>
		/// Decrypts ciphertext and returns the byte[] with the decrypted text.
		/// </summary>
		byte[] Decrypt(byte[] ciphertext);
	}
}
