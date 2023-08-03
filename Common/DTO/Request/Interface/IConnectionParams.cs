namespace Common.DTO
{
	public interface IConnectionParams : IParams
	{
		int LocalPort { get; set; }

		int RemotePort { get; set; }
	}
}
