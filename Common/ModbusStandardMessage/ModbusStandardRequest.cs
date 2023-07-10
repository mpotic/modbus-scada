namespace Common.ModbusStandardMessage
{
    public class ModbusStandardRequest : IModbusStandardRequest
    {
        public byte SlaveAddress { get; set; }

        public ushort StartAddress { get; set; }
        
        public ushort NumberOfPoints { get; set; }
        
        public bool[] CoilValues { get; set; }
        
        public ushort[] HoldingValues { get; set; }
    }
}
