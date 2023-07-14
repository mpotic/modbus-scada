using Common.ActionDto;
using Common.ResponseDto;
using ModbusServices.Connection;

namespace ModbusServices.Services
{
	internal interface IConnectionService
	{
		IStandardConnection StandardConnection { get; }

		IModbusConnection ModbusConnection { get; }	

		IOperationResponse StandardConnect(IConnectionParams connectionParams);

        IOperationResponse ModbusConnect(IConnectionParams connectionParams);
	}
}
