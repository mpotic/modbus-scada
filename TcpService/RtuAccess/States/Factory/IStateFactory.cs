namespace TcpService
{
	internal interface IStateFactory
	{
		IState GetDisconnectedState();

		IState GetConnectingState();

		IState GetConnectedState();

		IState GetListeningState();
	}
}
