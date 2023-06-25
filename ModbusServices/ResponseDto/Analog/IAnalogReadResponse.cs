using Common.ResponseDto;

namespace ModbusServices.ResponseDto.Analog
{
	public interface IAnalogReadResponse : IOperationResponse
	{
		ushort[] Values { get; set; }
	}
}
