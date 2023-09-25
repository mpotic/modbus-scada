namespace Proxy
{
	internal interface IProxyWorker
	{
		void Connect(string protocol, int localPort, int remotePort);

		void Disconnect(int port);

		void Listen(int port);

		void ReceiveProxy(int receivePort, int sendPort);

		void ReceiveMaster(int receivePort, int sendPort);

		void ListAllConections();
	}
}
