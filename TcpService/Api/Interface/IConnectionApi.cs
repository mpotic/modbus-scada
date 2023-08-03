using Common.Callback;
using Common.DTO;
using System.Threading.Tasks;

namespace TcpService
{
	public interface IConnectionApi
	{
		Task<IResponse> Connect(IConnectionParams connectionParams);

		IResponse Disconnect();

		Task<IResponse> Listen(IConnectionParams connectionParams);

		void RegisterCallack(IConnectionStatusCallback callback);
	}
}
