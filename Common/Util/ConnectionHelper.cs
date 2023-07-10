using System.Net;
using System.Net.Sockets;

namespace Common.Util
{
    public class ConnectionHelper : IConnectionHelper
    {
        /// <summary>
        /// Check if a port is available by attempting to connect to it. Failing to connect means that no app is listening on that port.
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool IsPortAvailable(int port)
        {
            var listener = new TcpListener(IPAddress.Loopback, port);

            try
            {
                listener.Start();
                listener.Stop();

                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }

        public int GetAvailablePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}
