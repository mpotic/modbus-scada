using Common.DTO;
using System.Threading.Tasks;

namespace MasterServices.ServiceProviders
{
	public interface IAnalogServiceProvider
	{
		Task<IReadAnalogResponse> ReadInput(IModbusActionParams actionParams);

		Task<IReadAnalogResponse> ReadHolding(IModbusActionParams actionParams);

		IResponse WriteHolding(IModbusActionParams actionParams);
	}
}
