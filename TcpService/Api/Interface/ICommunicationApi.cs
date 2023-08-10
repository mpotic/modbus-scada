using Common.DTO;
using System.Threading.Tasks;

namespace TcpService
{
	public interface ICommunicationApi
	{
		Task<ITcpReceiveResponse> Receive();

		Task<ITcpReceiveResponse> ReceiveWithTimeout();

		IResponse Send(byte[] message);
	}
}
