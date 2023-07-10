using Common.ActionDto;
using Common.ResponseDto;
using ModbusServices.ResponseDto.Discrete;
using System.Threading.Tasks;

namespace ModbusServices.ServiceProviders
{
	public interface IDiscreteServiceProvider
	{
		Task<IDiscreteReadResponse> ReadInput(IModbusActionParams actionParams);

		Task<IDiscreteReadResponse> ReadCoil(IModbusActionParams actionParams);

		IOperationResponse WriteCoil(IModbusActionParams actionParams);
	}
}