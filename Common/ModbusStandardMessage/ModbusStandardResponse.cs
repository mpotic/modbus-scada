namespace Common.ModbusStandardMessage
{
    public class ModbusStandardResponse : IModbusStandardResponse
    {
        public byte[] DiscreteValues { get; set; }
     
        public ushort[] AnalogValues { get; set; }
    }
}
