using Common.ParamsDto;
using Common.ResponseDto;
using System.Threading.Tasks;

namespace ModbusServices.ServiceProviders
{
    public interface IConnectionServiceProvider
    {
		/// <summary>
		/// Initializes a connection using just the TCP protocol.
		/// </summary>
		Task<IOperationResponse> StandardConnect(IConnectionParams connectionParams);

		/// <summary>
		/// Firstly initializes the Modbus service if needed. Initializes the connection to the Modbus slave using IModbusMaster.
		/// </summary>
		IOperationResponse ModbusConnect(IConnectionParams connectionParams);

        /// <summary>
        /// Disconnects from any type of connection.
        /// </summary>
        IOperationResponse Disconnect();
    }
}
