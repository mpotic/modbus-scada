using Proxy.Security.Certificate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy.Security
{
	internal class SecurityHandler : ISecurityHandler
	{
		IEncryption encryptionHandle = null;

		ISignature signatureHandle = null;

		ICertWorker certWorker = new CertWorker();

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

			if (signatureHandle != null)
			{
				data = signatureHandle.SignData(data);
			}

			if (encryptionHandle != null)
			{
				data = encryptionHandle.Encrypt(data);
			}

			return data;
		}

		public byte[] Validate(byte[] message)
		{
			byte[] data = new byte[message.Length];
			Array.Copy(message, data, message.Length);

			if (encryptionHandle != null)
			{
				int encryptionLength = BitConverter.ToInt32(message, 0);
                Console.WriteLine("Ciphtertext: " + BitConverter.ToString(message, 4, encryptionLength));
                data = encryptionHandle.Decrypt(data);
			}

			if (signatureHandle != null)
			{
				byte[] originalData = signatureHandle.GetOriginalData(data);
				byte[] signature = signatureHandle.GetSignatureFromSignedData(data);

				Console.WriteLine("Original data: " + Encoding.UTF8.GetString(originalData));
				Console.WriteLine($@"Signature (hex): {BitConverter.ToString(signature)}");

				if (!signatureHandle.IsSignatureValid(originalData, signature))
				{
					throw new Exception("Invalid signature!");
				}

				data = originalData;
			}

			return data;
		}

		public void ConfigureEncryption(EncryptionTypeCode encryptionType)
		{
			encryptionHandle = encryptionTypes[encryptionType].Invoke();
            Console.WriteLine("Encryption mode: " + encryptionType.ToString());
        }

		public void ConfigureSigning(bool isSign)
		{
			if (isSign)
			{
				signatureHandle = new Signature(certWorker);
                Console.WriteLine("Signing messages using RSA certificates.");
            }
			else
			{
				signatureHandle = null;
                Console.WriteLine("Stopped signing.");
            }
		}

		public void GenerateCert()
		{
			certWorker.GenerateCertificate();
		}

		public void LoadCert()
		{
			certWorker.LoadCertificates();
		}
 	}
}
