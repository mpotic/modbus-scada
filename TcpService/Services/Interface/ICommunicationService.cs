using Common.DTO;
using System.Threading.Tasks;

namespace TcpService
{
	public interface ICommunicationService
	{
		Task<ITcpReceiveResponse> Receive();

		Task<ITcpReceiveResponse> ReceiveWithTimeout();

		IResponse Send(byte[] message);

		IResponse ClearReceiveBuffer();
	}
}
