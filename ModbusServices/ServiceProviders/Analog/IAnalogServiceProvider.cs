using Common.ActionDto;
using Common.ResponseDto;
using ModbusServices.ResponseDto.Analog;
using System.Threading.Tasks;

namespace ModbusServices.ServiceProviders
{
	public interface IAnalogServiceProvider
	{
		Task<IAnalogReadResponse> ReadInput(IModbusActionParams actionParams);

		Task<IAnalogReadResponse> ReadHolding(IModbusActionParams actionParams);

		IOperationResponse WriteHolding(IModbusActionParams actionParams);
	}
}
