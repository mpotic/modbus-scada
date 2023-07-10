namespace Proxy.MessageHandling
{
    internal interface IMessageWorker
	{
		void AcceptAndStartReceiving();

        void ConnectAndStartReceiving(int remotePort);
        
        void Disconnect();
    }
}
