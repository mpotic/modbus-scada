using System;

namespace Proxy.Security
{
	internal class Security : ISecurity
	{
		IEncryption encryption;

		ISignature signature;

		public void Secure(byte[] message)
		{
            Console.WriteLine("Secured");
        }

		public void Validate(byte[] message)
		{
            Console.WriteLine("Validated");
        }
	}
}
