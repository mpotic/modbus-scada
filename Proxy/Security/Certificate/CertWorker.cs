using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.IO;

namespace Proxy.Security.Certificate
{
	internal class CertWorker : ICertWorker
	{
		private readonly int keySizeInBits = 2048;

		private readonly string password = "password";

		private readonly string filepath = "Resources/Security/";

		private readonly string pfxName = "proxy_private_certificate.pfx";

		private readonly string cerName = "proxy_public_certificate.cer";

		private X509Certificate2 publicCert;

		private X509Certificate2 privateCert;

		internal CertWorker()
		{
			if(File.Exists(filepath + publicCert) && File.Exists(filepath + privateCert))
			{
				LoadCertificates();
			}
		}

		public void GenerateCertificate()
		{
			using (RSA rsa = RSA.Create(keySizeInBits))
			{
				var certReq = new CertificateRequest(
					$"CN=Proxy",
					rsa,
					HashAlgorithmName.SHA256,
					RSASignaturePadding.Pkcs1);

				DateTime now = DateTime.UtcNow;
				X509Certificate2 cert = certReq.CreateSelfSigned(now, now.AddYears(1));

				byte[] certBytes = cert.Export(X509ContentType.Pfx, password);
				string pfxFilePath = filepath + pfxName;
				File.WriteAllBytes(pfxFilePath, certBytes);

				byte[] publicCertBytes = cert.Export(X509ContentType.Cert);
				string cerFilePath = filepath + cerName;
				File.WriteAllBytes(cerFilePath, publicCertBytes);

				Console.WriteLine("Certificates generated and saved.");
			}
		}

		public void LoadCertificates()
		{
			try
			{
				privateCert = new X509Certificate2(filepath + pfxName, password);
				publicCert = new X509Certificate2(filepath + cerName);
			}
			catch (Exception e)
			{
				privateCert = null;
				publicCert = null;
				throw new Exception("Failed to load certificates! " + e.Message);
            }

			Console.WriteLine("Certificates loaded.");
		}

		public byte[] GenerateSignature(byte[] dataToSign)
		{
			using (RSA rsa = privateCert.GetRSAPrivateKey())
			{
				byte[] signature = rsa.SignData(dataToSign, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

				return signature;
			}
		}

		public bool IsSignatureValid(byte[] originalData, byte[] signature)
		{
			using (RSA rsa = publicCert.GetRSAPublicKey())
			{
				bool isValid = rsa.VerifyData(originalData, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

				return isValid;
			}
		}

		public int GetSignatureSize()
		{
			return keySizeInBits / 8;
		}
	}
}
