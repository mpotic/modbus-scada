using Common.DTO;

namespace MasterApi.Api
{
	public interface IReadApi
	{
		void ReadHolding(IReadParams readParams);

		void ReadAnalogInput(IReadParams readParams);

		void ReadCoil(IReadParams readParams);

		void ReadDiscreteInput(IReadParams readParams);
		
		void ClearResults();
	}
}
