namespace Common.Util
{
	public interface IConnectionHelper
	{
		bool IsPortAvailable(int port);

		int GetAvailablePort();
	}
}
