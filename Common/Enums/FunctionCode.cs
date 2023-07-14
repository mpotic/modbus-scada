namespace Common.Enums
{
    public enum FunctionCode
    {
        ReadHolding = 0x03,
        ReadCoils = 0x01,
        ReadDiscreteInputs = 0x02,
        ReadAnalogInputs = 0x04,
        WriteHolding = 0x10,
        WriteCoils = 0x0F
    }
}
