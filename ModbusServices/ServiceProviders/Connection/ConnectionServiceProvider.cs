using Common.ActionDto;
using Common.ResponseDto;
using ModbusServices.Services;

namespace ModbusServices.ServiceProviders
{
    public class ConnectionServiceProvider : IConnectionServiceProvider
    {
        IConnectionService connectionService;

        internal ConnectionServiceProvider(IConnectionService connectionService) 
        {
            this.connectionService = connectionService;
        }
            
        public IOperationResponse StandardConnect(IConnectionParams connectionParams)
        {
            return connectionService.StandardConnect(connectionParams);
        }

        public IOperationResponse ModbusConnect(IConnectionParams connectionParams)
        {
            return connectionService.ModbusConnect(connectionParams);
        }
    }
}
