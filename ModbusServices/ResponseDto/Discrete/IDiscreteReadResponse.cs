using Common.ResponseDto;

namespace ModbusServices.ResponseDto.Discrete
{
	public interface IDiscreteReadResponse : IOperationResponse
	{
		byte[] Values { get; set; }
	}
}
