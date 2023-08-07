using Common.Callback;
using Common.DTO;
using Common.Enums;

namespace MasterApi.Api
{
	public interface IConnectionApi
	{
		void Connect(IConnectionParams connectionParams);

		void Disconnect(IConnectionParams connectionParams);

		void RegisterConnectionStatusCallback(IConnectionStatusCallback callback, ServiceTypeCode serviceType);
	}
}
