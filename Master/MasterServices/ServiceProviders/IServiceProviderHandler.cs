namespace MasterServices
{
	public interface IServiceProviderHandler
	{
		IModbusServiceProvider ModbusServiceProvider { get; }

		ITcpServiceProvider TcpServiceProvider { get; }
	}
}
