using Common.Connection;
using Common.DTO;
using Common.Enums;
using Proxy.Connections;

namespace Proxy.Commands
{
	internal class ReadCoilCommand : IModbusCommand
	{
		private ITcpSerializer serializer;

		private ITcpConnection proxy;

		private readonly IModbusConnection slave;

		public ReadCoilCommand(IModbusConnection slave)
        {
            this.slave = slave;
        }

		public void SetParams(IConnection connection, ITcpSerializer serializer)
		{
            proxy = (ITcpConnection)connection;
            this.serializer = serializer;
		}

		public async void Execute()
        {
            byte slaveAddress = serializer.ReadSlaveAddressFromBody();
            ushort startAddress = serializer.ReadStartAddressFromBody();
            ushort numberOfPoints = serializer.ReadNumberOfPointsFromBody();
            IReadParams readParams = new ReadParams(slaveAddress, startAddress, numberOfPoints);
            IReadDiscreteResponse response = await slave.Rtu.ReadCoil(readParams);

            SendResponse(response.BoolValues);
        }

        private void SendResponse(bool[] values)
        {
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.ProxyToMaster, FunctionCode.ReadCoils);
            serializer.AddBody(values);
            proxy.Communication.Send(serializer.Message);
        }
	}
}
