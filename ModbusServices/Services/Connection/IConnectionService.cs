using Common.ParamsDto;
using Common.ResponseDto;
using ModbusServices.Connection;
using System.Threading.Tasks;

namespace ModbusServices.Services
{
	internal interface IConnectionService
	{
		IStandardConnection StandardConnection { get; }

		IModbusConnection ModbusConnection { get; }

		Task<IOperationResponse> StandardConnect(IConnectionParams connectionParams);


		IOperationResponse ModbusConnect(IConnectionParams connectionParams);

		IOperationResponse Disconnect();
	}
}
