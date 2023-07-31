using Common.DTO;

namespace MasterApi.Api
{
	public interface IConnectionApi
	{
		void ModbusConnect(IConnectionParams connectionParams);

		void StandardConnect(IConnectionParams connectionParams);

		void Disconnect();
	}
}
