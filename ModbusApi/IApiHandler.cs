using ModbusApi.Api;

namespace ModbusApi
{
	public interface IApiHandler
	{
		IAnalogApi AnalogApi { get; set; }

		IDiscreteApi DiscreteApi { get; set; }

		IConnectionApi ConnectionApi { get; set; }
	}
}
