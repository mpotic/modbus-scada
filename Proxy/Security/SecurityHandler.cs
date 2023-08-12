using System;
using System.Collections.Generic;

namespace Proxy.Security
{
	internal class SecurityHandler : ISecurityHandler
	{
		IEncryption encryption = null;

		ISignature signature = null;

		private IDictionary<EncryptionTypeCode, Func<IEncryption>> encryptionTypes = new
			Dictionary<EncryptionTypeCode, Func<IEncryption>>()
		{
			{ EncryptionTypeCode.None, () => { return null; } },
			{ EncryptionTypeCode.AES, () => { return new AesEncryption(); } }
		};

		public byte[] Secure(byte[] message)
		{
			byte[] data = new byte[message.Length];
			Array.Copy(message, data, message.Length);

			if (signature != null)
			{
				data = signature.Sign(data);
			}

			if (encryption != null)
			{
				data = encryption.Encrypt(data);
			}

			return data;
		}

		public byte[] Validate(byte[] message)
		{
			byte[] data = new byte[message.Length];
			Array.Copy(message, data, message.Length);

			if (encryption != null)
			{
				data = encryption.Decrypt(data);
			}

			if (signature != null)
			{
				if (!signature.IsValid(data))
				{
					throw new Exception("Invalid signature!");
				}

				data = signature.RemoveSignature(data);
			}

			return data;
		}

		public void ConfigureEncryption(EncryptionTypeCode encryptionType)
		{
			encryption = encryptionTypes[encryptionType].Invoke();
            Console.WriteLine("Encryption mode: " + encryptionType.ToString());
        }
	}
}
