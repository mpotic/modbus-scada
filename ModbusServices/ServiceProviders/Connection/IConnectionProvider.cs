using Common.ActionDto;
using Common.ResponseDto;

namespace ModbusServices.ServiceProviders
{
	public interface IConnectionProvider
	{
		IOperationResponse StandardConnect(IConnectionParams connectionParams);

		IOperationResponse ModbusConnect(IConnectionParams connectionParams);
	}
}
