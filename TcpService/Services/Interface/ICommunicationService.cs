using Common.DTO;
using System.Threading.Tasks;

namespace TcpService
{
	public interface ICommunicationService
	{
		Task<ITcpReceiveResponse> Receive();

		IResponse Send(byte[] message);
	}
}
