namespace Common.ActionDto
{
	public interface IConnectionParams : IModbusParams
	{
		int ServerPort { get; set; }

		int ClientPort { get; set; }
	}
}
