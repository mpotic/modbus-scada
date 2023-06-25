using ModbusApi.Api;
using ModbusApi.ViewModel;
using ModbusServices.Services;

namespace ModbusApi
{
	public class ModbusApiHandler : IModbusApiHandler
	{
		public ModbusApiHandler(IReadResultsViewModel readResults, IMessageBoxCallback callback)
		{
			IServiceHandler serviceInitializer = new ServiceHandler();

			AnalogApi = new AnalogApi(serviceInitializer.AnalogProvider, readResults, callback);
			DiscreteApi = new DiscreteApi(serviceInitializer.DiscreteProvider, readResults, callback);
			ConnectionApi = new ConnectionApi(serviceInitializer.ConnectionProvider, callback);
		}

		public IAnalogApi AnalogApi { get; set; }

		public IDiscreteApi DiscreteApi { get; set; }

		public IConnectionApi ConnectionApi { get; set; }
	}
}
