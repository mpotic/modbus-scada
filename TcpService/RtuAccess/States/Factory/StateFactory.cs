namespace TcpService
{
	internal sealed class StateFactory : IStateFactory
	{
		private ISocketAccess socketAccess;

		private ICommunicationService communicationService;

		private IConnectionService connectionService;

		public StateFactory(ISocketAccess socketAccess, ICommunicationService communicationService, IConnectionService connectionService)
		{
			this.socketAccess = socketAccess;
			this.communicationService = communicationService;
			this.connectionService = connectionService;
		}

		public IState GetDisconnectedState()
		{
			DisconnectedState state = new DisconnectedState(connectionService, socketAccess, this);

			return state;
		}

		public IState GetConnectingState()
		{
			ConnectingState state = new ConnectingState();

			return state;
		}

		public IState GetConnectedState()
		{
			ConnectedState state = new ConnectedState(socketAccess, this, communicationService, connectionService);

			return state;
		}

		public IState GetListeningState()
		{
			ListeningState state = new ListeningState(connectionService, socketAccess, this);

			return state;
		}
	}
}
