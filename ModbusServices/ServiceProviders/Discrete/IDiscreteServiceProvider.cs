using Common.ActionDto;
using Common.ResponseDto;
using ModbusServices.ResponseDto.Discrete;

namespace ModbusServices.ServiceProviders
{
	public interface IDiscreteServiceProvider
	{
		IDiscreteReadResponse ReadInput(IModbusActionParams actionParams);

		IDiscreteReadResponse ReadCoil(IModbusActionParams actionParams);

		IOperationResponse WriteCoil(IModbusActionParams actionParams);
	}
}