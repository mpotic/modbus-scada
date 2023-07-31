namespace ModbusService
{
	public interface IModbusServiceHandler
	{
		IRtuServiceApi RtuServiceApi { get; }

		IConnectionServiceApi ConnectionServiceApi { get; }
	}
}
