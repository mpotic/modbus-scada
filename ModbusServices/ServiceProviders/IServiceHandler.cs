using ModbusServices.ServiceProviders;

namespace ModbusServices.Services
{
	/// <summary>
	/// Used for dependency injection and service setup.
	/// </summary>
	public interface IServiceHandler
	{
		IConnectionServiceProvider ConnectionProvider { get; }

		IDiscreteServiceProvider DiscreteProvider { get; }

		IAnalogServiceProvider AnalogProvider { get; }
	}
}
