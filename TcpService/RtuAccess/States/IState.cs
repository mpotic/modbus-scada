using Common.DTO;
using System.Threading.Tasks;

namespace TcpService
{
	internal interface IState
	{
		Task<IResponse> Connect(IConnectionParams connectionParams);

		IResponse Disconnect();

		Task<IResponse> Listen(IConnectionParams connectionParams);

		Task<ITcpReceiveResponse> Receive();

		Task<ITcpReceiveResponse> ReceiveWithTimeout();

		IResponse Send(byte[] message);
	}
}
