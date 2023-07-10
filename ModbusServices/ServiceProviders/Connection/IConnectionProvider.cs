using Common.ActionDto;
using Common.ResponseDto;

namespace ModbusServices.ServiceProviders
{
	public interface IConnectionProvider
	{
		IOperationResponse StandardConnect(IConnectionParams connectionParams);

        /// <summary>
        /// Firstly initializes the Modbus service if needed. Initialize the connection to the Modbus slave using IModbusMaster.
        /// </summary>
        /// <param name="connectionParams">Specifies clients port.</param>
        /// <returns></returns>
        IOperationResponse ModbusConnect(IConnectionParams connectionParams);
	}
}
