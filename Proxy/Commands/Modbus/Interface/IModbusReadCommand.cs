using System.Threading.Tasks;

namespace Proxy.Commands
{
	internal interface IModbusReadCommand : IModbusCommand
	{
		Task Execute();
	}
}
