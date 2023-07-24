using Common.ParamsDto;
using Common.ResponseDto;
using ModbusServices.Services;
using System.Threading.Tasks;

namespace ModbusServices.ServiceProviders
{
    public class ConnectionServiceProvider : IConnectionServiceProvider
    {
        IConnectionService connectionService;

        internal ConnectionServiceProvider(IConnectionService connectionService) 
        {
            this.connectionService = connectionService;
        }
            
        public async Task<IOperationResponse> StandardConnect(IConnectionParams connectionParams)
        {
            return await connectionService.StandardConnect(connectionParams);
        }

        public IOperationResponse ModbusConnect(IConnectionParams connectionParams)
        {
            return connectionService.ModbusConnect(connectionParams);
        }

        public IOperationResponse Disconnect()
        {
            return connectionService.Disconnect();
        }
    }
}
