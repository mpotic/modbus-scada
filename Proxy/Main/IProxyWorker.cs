using Proxy.Security;

namespace Proxy
{
	internal interface IProxyWorker
	{
		void Connect(string protocol, int localPort, int remotePort);

		void Disconnect(int port);

		void Listen(int port);

		void Receive(int receivePort, int sendPort);

		void ListAllConections();

		void ConfigureEncryption(EncryptionTypeCode encryptionCode);
	}
}
