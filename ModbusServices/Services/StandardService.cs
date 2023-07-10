using Common.Connection;
using Common.Enums;
using Common.Util;
using ModbusServices.Connection;
using System.Threading.Tasks;

namespace ModbusServices.ServiceProviders
{
    internal class StandardService : IStandardService
	{
		private readonly IStandardConnection connection;

		private readonly ITcpSerializer serializer = new TcpSerializer();

		private readonly IByteArrayConverter byteArrayConverter = new ByteArrayConverter();

		public StandardService(IStandardConnection connection)
		{
			this.connection = connection;
		}

		public async Task<ushort[]> ReadAnalogInput(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.Master, ModbusRequestCode.ReadAnalogInput);
            serializer.AddBody(slaveAddress, startAddress, numberOfPoints);
			connection.Connection.Send(serializer.Message);

			byte[] response = await connection.Connection.ReceiveAsync();
            serializer.InitMessage(response);
            ushort[] retVal = serializer.ReadAnalogReadValuesFromBody();

			return retVal;
        }

        public async Task<ushort[]> ReadHolding(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.Master, ModbusRequestCode.ReadHolding);
            serializer.AddBody(slaveAddress, startAddress, numberOfPoints);
            connection.Connection.Send(serializer.Message);

            byte[] response = await connection.Connection.ReceiveAsync();
            serializer.InitMessage(response);
            ushort[] retVal = serializer.ReadAnalogReadValuesFromBody();

            return retVal;
        }

        public async Task<bool[]> ReadCoil(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {

            serializer.InitMessage();
            serializer.AddHeader(SenderCode.Master, ModbusRequestCode.ReadCoil);
            serializer.AddBody(slaveAddress, startAddress, numberOfPoints);
            connection.Connection.Send(serializer.Message);

            byte[] response = await connection.Connection.ReceiveAsync();
            serializer.InitMessage(response);
            bool[] retVal = serializer.ReadDiscreteReadValuesFromBody();

            return retVal;
        }

        public async Task<bool[]> ReadDiscreteInput(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.Master, ModbusRequestCode.ReadDiscreteInput);
            serializer.AddBody(slaveAddress, startAddress, numberOfPoints);
            connection.Connection.Send(serializer.Message);

            byte[] response = await connection.Connection.ReceiveAsync();
            serializer.InitMessage(response);
            bool[] retVal = serializer.ReadDiscreteReadValuesFromBody();

            return retVal;
        }

		public void WriteCoil(byte slaveAddress, ushort startAddress, bool[] writeValues)
        {
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.Master, ModbusRequestCode.WriteCoil);
            serializer.AddBody(slaveAddress, startAddress, writeValues);
            connection.Connection.Send(serializer.Message);
        }

		public void WriteHolding(byte slaveAddress, ushort startAddress, ushort[] writeValues)
        {
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.Master, ModbusRequestCode.WriteHolding);
            serializer.AddBody(slaveAddress, startAddress, writeValues);
            connection.Connection.Send(serializer.Message);
        }
    }
}
