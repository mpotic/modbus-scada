using MasterApi.Api;
using MasterApi.ViewModel;
using MasterServices.Services;

namespace MasterApi
{
	public class ApiHandler : IApiHandler
	{
		public ApiHandler(IReadResultsViewModel readResults, IMessageBoxCallback callback)
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
