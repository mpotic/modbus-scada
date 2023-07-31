using Common.DTO;
using System.Threading.Tasks;

namespace TcpService
{
	public interface ICommunicationApi
	{
		Task<ITcpReceiveResponse> Receive();

		IResponse Send(byte[] message);
	}
}
