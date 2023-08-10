namespace Proxy.Commands
{
	internal interface IModbusWriteCommand : IModbusCommand
	{
		void Execute();
	}
}
