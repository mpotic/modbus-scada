using Common.Callback;
using Common.DTO;
using System.Threading.Tasks;

namespace ModbusService
{
	public interface IConnectionServiceApi
	{
		Task<IResponse> Connect(IConnectionParams connectionParams);

		IResponse Disconnect();

		void RegisterCallack(IConnectionStatusCallback callback);
	}
}
