namespace Common.ModbusStandardMessage
{
    public interface IModbusStandardRequest
    {
        byte SlaveAddress { get; set; }

        ushort StartAddress { get; set; }  

        ushort NumberOfPoints { get; set; }

        bool[] CoilValues { get; set; }

        ushort[] HoldingValues { get; set; }
    }
}
