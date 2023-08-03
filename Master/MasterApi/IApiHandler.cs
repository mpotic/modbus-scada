using MasterApi.Api;

namespace MasterApi
{
	public interface IApiHandler
	{
		IWriteApi WriteApi { get; }

		IReadApi ReadApi { get; }

		IConnectionApi ConnectionApi { get; }
	}
}
