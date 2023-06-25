using ModbusServices.ServiceProviders;

namespace ModbusServices.Services
{
	public interface IServiceHandler
	{
		IConnectionProvider ConnectionProvider { get; }

		IDiscreteServiceProvider DiscreteProvider { get; }

		IAnalogServiceProvider AnalogProvider { get; }
	}
}
