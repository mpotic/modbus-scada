namespace TcpService
{
	internal interface IConnectionStatus
	{
		ConnectionStatusCode StatusCode { get; set; }

		IConnectionStatusCallback Callback { get; set; }
	}
}
