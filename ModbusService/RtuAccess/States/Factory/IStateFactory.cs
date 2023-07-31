namespace ModbusService
{
	internal interface IStateFactory
	{
		IState GetDisconnectedState();

		IState GetConnectingState();

		IState GetConnectedState();
	}
}
