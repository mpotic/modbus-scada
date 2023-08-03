using Common.DTO;

namespace MasterView.Actions
{
	internal interface IWriteHoldingAction : IAction
 	{
		void SetParams(IWriteHoldingParams coilParamss);
	}
}
