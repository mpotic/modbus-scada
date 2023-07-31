using Common.DTO;
using System.Threading.Tasks;

namespace ModbusService
{
	internal interface IConnectionService
	{
		bool IsConnected { get; }

		Task<IResponse> Connect(IConnectionParams connectionParams);

		IResponse Disconnect();
	}
}
