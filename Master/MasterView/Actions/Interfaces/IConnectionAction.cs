using Common.DTO;

namespace MasterView.Actions
{
	internal interface IConnectionAction : IAction 
	{
		void SetParams(IConnectionParams connectionParams);
	}
}
