namespace Common.ModbusStandardMessage
{
    public interface IModbusStandardResponse
    {
        byte[] DiscreteValues { get; set; }

        ushort[] AnalogValues { get; set; }
    }
}
