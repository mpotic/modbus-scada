using Newtonsoft.Json.Linq;
using Proxy.Security.Certificate;
using System;
using System.Linq;
using System.Text;

namespace Proxy.Security
{
	internal class Signature : ISignature
	{
		private readonly ICertWorker certWorker;

		public Signature(ICertWorker worker)
		{
			certWorker = worker;
		}

		public byte[] SignData(byte[] data)
		{
			byte[] signature = certWorker.GenerateSignature(data);
			int originalDataSize = data.Length;
			byte[] sizeBytes = BitConverter.GetBytes(originalDataSize);
			
			byte[] signedData = sizeBytes.Concat(data).Concat(signature).ToArray();

			return signedData;
		}

		public bool IsSignatureValid(byte[] originalData, byte[] signature)
		{
			bool isValid = certWorker.IsSignatureValid(originalData, signature);

			return isValid;
		}

		public byte[] GetOriginalData(byte[] signedData)
		{
			int originalDataSize = BitConverter.ToInt32(signedData, 0);
			byte[] originalData = new byte[originalDataSize];
			Array.Copy(signedData, 4, originalData, 0, originalDataSize);

			return originalData;
		}

		public byte[] GetSignatureFromSignedData(byte[] signedData)
		{
			int originalDataSize = BitConverter.ToInt32(signedData, 0);
			int signatureSize = certWorker.GetSignatureSize();
			byte[] signature = new byte[signatureSize];
			Array.Copy(signedData, 4 + originalDataSize, signature, 0, signatureSize);

			return signature;
		}

	}
}
