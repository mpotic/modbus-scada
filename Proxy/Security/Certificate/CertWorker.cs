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

		private readonly string pfxPath = "Resources/Security/proxy_private_certificate.pfx";

		private readonly string cerPath = "Resources/Security/proxy_public_certificate.cer";

		private readonly string publicCertSubjectName = "MilosProxyBsc";

		private X509Certificate2 publicCert;

		private X509Certificate2 privateCert;

		public void MakeCertificates()
		{
			using (RSA rsa = RSA.Create(keySizeInBits))
			{
				var certRequest = new CertificateRequest(
					$"CN={publicCertSubjectName}",
					rsa,
					HashAlgorithmName.SHA256,
					RSASignaturePadding.Pkcs1);

				DateTime now = DateTime.UtcNow;
				X509Certificate2 cert = certRequest.CreateSelfSigned(now, now.AddYears(1));

				byte[] pftBytes = cert.Export(X509ContentType.Pfx, password);
				string pfxFilePath = pfxPath;
				File.WriteAllBytes(pfxFilePath, pftBytes);

				byte[] cerBytes = cert.Export(X509ContentType.Cert);
				string cerFilePath = cerPath;
				File.WriteAllBytes(cerFilePath, cerBytes);

				Console.WriteLine("Certificates created and saved on a file system.");

				StorePublicCert();
			}
		}

		private void StorePublicCert()
		{
			X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			if (store == null)
			{
				throw new Exception("Failed to open the store!");
			}

			store.Open(OpenFlags.ReadWrite);
			var certificates = store.Certificates.Find(X509FindType.FindBySubjectName, "MilosProxyBsc", false);
			if (certificates.Count > 1)
			{
				throw new Exception("More than one certificate is already stored!");
			}
			else if(certificates.Count == 1)
			{
				store.Remove(certificates[0]);
				Console.WriteLine("Existing certificate removed from store.");
			}

			var cert = new X509Certificate2(cerPath);
			store.Add(cert);

            Console.WriteLine("Public certificate stored in current users Personal certificates.");
        }

		public void LoadCertificates()
		{
			privateCert = new X509Certificate2(pfxPath, password);
			LoadPublicCertFromStore();

			Console.WriteLine("Certificates loaded.");
		}

		private void LoadPublicCertFromStore()
		{
			X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			if (store == null)
			{
				throw new Exception("Failed to open the store!");
			}

			store.Open(OpenFlags.ReadOnly);
			var certCollection = store.Certificates.Find(X509FindType.FindBySubjectName, "MilosProxyBsc", false);

			if (certCollection == null || certCollection.Count == 0)
			{
				throw new Exception("Failed to find the certificate: " + publicCertSubjectName);
			}

			publicCert = certCollection[0];
			store.Close();
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
