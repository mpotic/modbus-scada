using Common.ActionDto;
using Common.ResponseDto;
using ModbusServices.ResponseDto.Analog;

namespace ModbusServices.ServiceProviders
{
	public interface IAnalogServiceProvider
	{
		IAnalogReadResponse ReadInput(IModbusActionParams actionParams);

		IAnalogReadResponse ReadHolding(IModbusActionParams actionParams);

		IOperationResponse WriteHolding(IModbusActionParams actionParams);
	}
}
