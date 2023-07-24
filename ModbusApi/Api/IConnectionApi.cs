using Common.ParamsDto;

namespace ModbusApi.Api
{
	public interface IConnectionApi
	{
		void ModbusConnect(IConnectionParams connectionParams);

		void StandardConnect(IConnectionParams connectionParams);

		void Disconnect();
	}
}
