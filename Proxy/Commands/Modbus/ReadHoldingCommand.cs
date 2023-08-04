using Common.Connection;
using Common.DTO;
using Common.Enums;
using Proxy.Connections;

namespace Proxy.Commands
{
	internal class ReadHoldingCommand : IModbusCommand
	{
		private ITcpSerializer serializer;

		private ITcpConnection proxy;

		private readonly IModbusConnection slave;

		public ReadHoldingCommand(IModbusConnection slave)
		{
			this.slave = slave;
		}

		public void SetParams(IConnection connection, ITcpSerializer serializer)
		{
			this.serializer = serializer;
			proxy = (ITcpConnection)connection;
		}

		public async void Execute()
        {
            byte slaveAddress = serializer.ReadSlaveAddressFromBody();
            ushort startAddress = serializer.ReadStartAddressFromBody();
            ushort numberOfPoints = serializer.ReadNumberOfPointsFromBody();
            IReadParams holdingParams = new ReadParams(slaveAddress, startAddress, numberOfPoints);
            IReadAnalogResponse response= await slave.Rtu.ReadHolding(holdingParams);

            SendResponse(response.Values);
        }

        private void SendResponse(ushort[] values)
        {
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.ProxyToMaster, FunctionCode.ReadHolding);
            serializer.AddBody(values);
            proxy.Communication.Send(serializer.Message);
        }
	}
}
