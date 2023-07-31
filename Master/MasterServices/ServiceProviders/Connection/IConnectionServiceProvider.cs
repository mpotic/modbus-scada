using Common.DTO;
using System.Threading.Tasks;

namespace MasterServices.ServiceProviders
{
    public interface IConnectionServiceProvider
    {
		/// <summary>
		/// Initializes a connection using just the TCP protocol.
		/// </summary>
		Task<IResponse> StandardConnect(IConnectionParams connectionParams);

		/// <summary>
		/// Firstly initializes the Modbus service if needed. Initializes the connection to the Modbus slave using IModbusMaster.
		/// </summary>
		IResponse ModbusConnect(IConnectionParams connectionParams);

        /// <summary>
        /// Disconnects from any type of connection.
        /// </summary>
        IResponse Disconnect();
    }
}
