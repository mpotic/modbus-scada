using ModbusServices.ServiceProviders;

namespace ModbusServices.Services
{
	public class ServiceHandler : IServiceHandler
	{
		public ServiceHandler()
		{
			ConnectionProvider = new ConnectionProvider(this);
			DiscreteProvider = new DiscreteServiceProvider();
			AnalogProvider = new AnalogServiceProvider();
		}

		internal void SetupStandardServices()
		{
			IStandardService service = new StandardService(((ConnectionProvider)ConnectionProvider).StandardConnection);

			((DiscreteServiceProvider)DiscreteProvider).SetService(service);
			((AnalogServiceProvider)AnalogProvider).SetService(service);
		}

		internal void SetupModbusServices()
		{
			IModbusService service = new ModbusService(((ConnectionProvider)ConnectionProvider).ModbusConnection);

			((DiscreteServiceProvider)DiscreteProvider).SetService(service);
			((AnalogServiceProvider)AnalogProvider).SetService(service);
		}

		public IConnectionProvider ConnectionProvider { get; private set; }

		public IDiscreteServiceProvider DiscreteProvider { get; private set; }

		public IAnalogServiceProvider AnalogProvider { get; private set; }
	}
}
