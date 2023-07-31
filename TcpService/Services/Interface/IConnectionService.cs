using Common.DTO;
using System.Threading.Tasks;

namespace TcpService
{
	internal interface IConnectionService
	{
		bool IsConnected { get; }

		Task<IResponse> Connect(IConnectionParams connectionParams);

		IResponse Disconnect();

		Task<IResponse> Listen(IConnectionParams connectionParams);
	}
}
