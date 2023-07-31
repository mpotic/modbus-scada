namespace TcpService
{
	public class TcpServiceHandler
	{
		public TcpServiceHandler()
		{
			IConnectionStatus connectionStatus = new ConnectionStatus();
			ISocketAccess socketAccess = new SocketAccess(connectionStatus);
			CommunicationApi = new CommunicationApi(socketAccess);
			ConnectionApi = new ConnectionApi(socketAccess, connectionStatus);
			IConnectionHandle connectionHandle = new ConnectionHandle();
			IConnectionService connectionService = new ConnectionService(connectionHandle);
			ICommunicationService communicationService = new CommunicationService(connectionHandle);
			IStateFactory stateFactory = new StateFactory(socketAccess, communicationService, connectionService);

			socketAccess.TransitionState(stateFactory.GetDisconnectedState(), ConnectionStatusCode.Disconnected);
		}

		public ICommunicationApi CommunicationApi { get; private set; }

		public IConnectionApi ConnectionApi { get; private set; }	
	}
}
