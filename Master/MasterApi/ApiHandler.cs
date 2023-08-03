using MasterApi.Api;
using MasterApi.ViewModel;
using MasterServices;

namespace MasterApi
{
	public class ApiHandler : IApiHandler
	{
		public ApiHandler(IReadResultsViewModel readResults, IMessageBoxCallback callback)
		{
			IServiceProviderHandler serviceHandler = new ServiceProviderHandler();
			WriteApi = new WriteApi(callback, serviceHandler);
			ReadApi = new ReadApi(readResults, callback, serviceHandler);
			ConnectionApi = new ConnectionApi(serviceHandler, callback);
		}

		public IWriteApi WriteApi { get; }

		public IReadApi ReadApi { get; }

		public IConnectionApi ConnectionApi { get; }
	}
}
