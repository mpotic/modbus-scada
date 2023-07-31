using Common.DTO;
using MasterServices.Services;
using System.Threading.Tasks;

namespace MasterServices.ServiceProviders
{
    public class ConnectionServiceProvider : IConnectionServiceProvider
    {
        IConnectionService connectionService;

        internal ConnectionServiceProvider(IConnectionService connectionService) 
        {
            this.connectionService = connectionService;
        }
            
        public async Task<IResponse> StandardConnect(IConnectionParams connectionParams)
        {
            return await connectionService.StandardConnect(connectionParams);
        }

        public IResponse ModbusConnect(IConnectionParams connectionParams)
        {
            return connectionService.ModbusConnect(connectionParams);
        }

        public IResponse Disconnect()
        {
            return connectionService.Disconnect();
        }
    }
}
