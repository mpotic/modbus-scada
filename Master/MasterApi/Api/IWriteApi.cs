using Common.DTO;

namespace MasterApi.Api
{
	public interface IWriteApi
	{
		void WriteHolding(IWriteHoldingParams writeParams);

		void WriteCoil(IWriteCoilParams writeParams);
	}
}
