using Common.Connection;

namespace MasterServices.Connection
{
    internal class StandardConnection : IStandardConnection
	{
		public ITcpSocketHandler Connection { get; set; }
	}
}
