using Common.DTO;

namespace MasterView.Actions
{
	internal interface IReadAction : IAction
	{
		void SetParams(IReadParams readParams);
	}
}
