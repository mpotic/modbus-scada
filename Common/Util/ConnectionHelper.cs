using System.Net.Sockets;

namespace Common.Util
{
	public class ConnectionHelper : IConnectionHelper
	{
		public bool IsPortAvailable(int port)
		{
			using (var client = new TcpClient())
			{
				try
				{
					client.Connect("localhost", port);
					return false; 
				}
				catch (SocketException)
				{
					return true; 
				}
			}
		}
	}
}
