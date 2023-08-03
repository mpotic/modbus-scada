using ModbusService;
using TcpService;

namespace MasterServices
{
	public class ServiceProviderHandler : IServiceProviderHandler
	{
		public ServiceProviderHandler() 
		{ 
			ModbusServiceHandler modbusServiceHandler = new ModbusServiceHandler();
			TcpServiceHandler tcpServiceHandler = new TcpServiceHandler();
			ModbusServiceProvider = new ModbusServiceProvider(modbusServiceHandler);
			TcpServiceProvider = new TcpServiceProvider(tcpServiceHandler);
		}

		public IModbusServiceProvider ModbusServiceProvider { get; private set; }

		public ITcpServiceProvider TcpServiceProvider { get; private set; }
	}
}
