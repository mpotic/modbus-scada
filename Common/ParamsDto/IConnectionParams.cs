namespace Common.ParamsDto
{
	public interface IConnectionParams : IModbusParams
	{
		int LocalPort { get; set; }

		int RemotePort { get; set; }
	}
}
