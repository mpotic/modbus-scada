namespace TcpService
{
	public interface ITcpServiceHandler
	{
		ICommunicationApi CommunicationApi { get; }

		IConnectionApi ConnectionApi { get; }
	}
}
