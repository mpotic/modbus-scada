namespace Proxy.MessageHandling
{
    internal interface IMessageWorker
	{
		void ListenAcceptAndStartReceiving();

        void ConnectAndStartReceiving(int remotePort);

        void AutoConnectAndStartReceiving();
        
        void Disconnect();
    }
}
