using Common.Enums;

namespace ModbusService
{
	public class ModbusServiceHandler : IModbusServiceHandler
	{
		public ModbusServiceHandler() 
		{
			IConnectionHandle connectionHandle = new ConnectionHandle();
			IConnectionStatus connectionStatus = new ConnectionStatus();
			IRtuAccess rtuAccess = new RtuAccess(connectionStatus);
			IRtuService rtuService = new RtuService(connectionHandle);
			IConnectionService connectionService = new ConnectionService(connectionHandle);
			IStateFactory stateFactory = new StateFactory(rtuAccess, rtuService, connectionService);
			RtuServiceApi = new RtuServiceApi(rtuAccess);
			ConnectionServiceApi = new ConnectionServiceApi(rtuAccess, connectionStatus);

			// Set the initial state to be disconnected.
			rtuAccess.TransitionState(stateFactory.GetDisconnectedState(), ConnectionStatusCode.Disconnected);
		}

		public IRtuServiceApi RtuServiceApi { get; private set; }

		public IConnectionServiceApi ConnectionServiceApi { get; private set; }
	}
}
