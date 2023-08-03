using Common.DTO;

namespace MasterView.Actions
{
	internal interface IWriteCoilAction : IAction
	{
		void SetParams(IWriteCoilParams holdingParams);
	}
}
