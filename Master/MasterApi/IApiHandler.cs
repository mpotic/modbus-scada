using MasterApi.Api;

namespace MasterApi
{
	public interface IApiHandler
	{
		IAnalogApi AnalogApi { get; set; }

		IDiscreteApi DiscreteApi { get; set; }

		IConnectionApi ConnectionApi { get; set; }
	}
}
