namespace Proxy.Security.Certificate
{
	internal interface ICertWorker
	{
		void MakeCertificates();
		
		void LoadCertificates();

		byte[] GenerateSignature(byte[] dataToSign);

		bool IsSignatureValid(byte[] originalData, byte[] signature);
		
		int GetSignatureSize();
	}
}
