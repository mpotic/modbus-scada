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
			byte[] signedData;
			try
			{
				byte[] signature = certWorker.GenerateSignature(data);
				signedData = data.Concat(signature).ToArray();
			}
			catch(Exception e)
			{
				throw new Exception("Exception occured while signing data!\n" + e.Message);
			}

			return signedData;
		}

		public bool IsSignatureValid(byte[] originalData, byte[] signature)
		{
			bool isValid;
			try
			{
				isValid = certWorker.IsSignatureValid(originalData, signature);
			}
			catch(Exception e)
			{
				throw new Exception("Exception occured while validating signature!\n" + e.Message);
			}

			return isValid;
		}

		public byte[] GetOriginalData(byte[] signedData)
		{
			byte[] originalData;
			try
			{
				string signedDataStr = Encoding.UTF8.GetString(signedData);
				if (!int.TryParse(signedDataStr.Split(';')[0], out int originalDataSize))
				{
					throw new Exception("Couldn't extract the size from the signed message!");
				}
				originalData = new byte[originalDataSize];
				Array.Copy(signedData, 0, originalData, 0, originalDataSize);
			}
			catch(Exception e) 
			{
				throw new Exception("Exception while extracting data from signed message!\n" + e.Message);
			}

			return originalData;
		}

		public byte[] GetSignatureFromSignedData(byte[] signedData)
		{
			byte[] signature;
			try
			{
				string signedDataStr = Encoding.UTF8.GetString(signedData);
				if (!int.TryParse(signedDataStr.Split(';')[0], out int originalDataSize))
				{
					throw new Exception("Couldn't extract the size from the signed message!");
				}
				int signatureSize = certWorker.GetSignatureSize();
				signature = new byte[signatureSize];
				Array.Copy(signedData, originalDataSize, signature, 0, signatureSize);
			}
			catch(Exception e)
			{
				throw new Exception("Exception occured while extracting signature from signed message!\n" + e.Message);
			}

			return signature;
		}

	}
}
