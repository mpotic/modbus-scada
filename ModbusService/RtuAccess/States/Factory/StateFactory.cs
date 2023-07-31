namespace ModbusService
{
	internal sealed class StateFactory : IStateFactory
	{
		private IRtuAccess rtuAccess;

		private IRtuService rtuService;

		private IConnectionService connectionService;

		public StateFactory(IRtuAccess rtuAccess, IRtuService rtuService, IConnectionService connectionService)
		{
			this.rtuAccess = rtuAccess;
			this.rtuService = rtuService;
			this.connectionService = connectionService;
		}

		public IState GetDisconnectedState()
		{
			DisconnectedState state = new DisconnectedState(rtuAccess, connectionService, this);

			return state;
		}

		public IState GetConnectingState()
		{
			ConnectingState state = new ConnectingState();

			return state;
		}

		public IState GetConnectedState()
		{
			ConnectedState state = new ConnectedState(rtuAccess, connectionService, rtuService, this);

			return state;
		}
	}
}
