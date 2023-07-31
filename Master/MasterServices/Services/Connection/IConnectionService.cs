using Common.DTO;
using MasterServices.Connection;
using System.Threading.Tasks;

namespace MasterServices.Services
{
	internal interface IConnectionService
	{
		IStandardConnection StandardConnection { get; }

		IModbusConnection ModbusConnection { get; }

		Task<IResponse> StandardConnect(IConnectionParams connectionParams);


		IResponse ModbusConnect(IConnectionParams connectionParams);

		IResponse Disconnect();
	}
}
