using Common.DTO;
using System.Threading.Tasks;

namespace MasterServices.ServiceProviders
{
	public interface IDiscreteServiceProvider
	{
		Task<IReadDiscreteResponse> ReadInput(IModbusActionParams actionParams);

		Task<IReadDiscreteResponse> ReadCoil(IModbusActionParams actionParams);

		IResponse WriteCoil(IModbusActionParams actionParams);
	}
}