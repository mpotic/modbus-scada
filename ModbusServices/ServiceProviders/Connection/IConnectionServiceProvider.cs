using Common.ActionDto;
using Common.ResponseDto;

namespace ModbusServices.ServiceProviders
{
    public interface IConnectionServiceProvider
    {
        /// <summary>
        /// Initializes a connection using just the TCP protocol.
        /// </summary>
        IOperationResponse StandardConnect(IConnectionParams connectionParams);

        /// <summary>
        /// Firstly initializes the Modbus service if needed. Initializes the connection to the Modbus slave using IModbusMaster.
        /// </summary>
        IOperationResponse ModbusConnect(IConnectionParams connectionParams);
    }
}
