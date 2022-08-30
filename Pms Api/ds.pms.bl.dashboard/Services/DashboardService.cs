using ds.pms.apicommon.Models;
using ds.pms.bl.logger;
using ds.pms.bl.dashboard.IServices;
using ds.pms.dal.CustomModels;
using ds.pms.dal.CustomRepository;
using ds.pms.dal.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace ds.pms.bl.roomhistory.Services
{
    public class DashboardService : IDashboradService
    {
        private DashboardReporsitory dashboardRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public DashboardService(string _databaseProvider, string _connectionString, ILogger<DashboardService> dashboardServiceLogger)
        {
            dashboardRepository = new DashboardReporsitory(_databaseProvider, _connectionString);
            logging = new Logging(dashboardServiceLogger);
            _className = this.GetType().Name;
        }

        public DashboardService(string _databaseProvider, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds
            , ILogger<DashboardService> roomServiceLogger)
        {
            dashboardRepository = new DashboardReporsitory(_databaseProvider, _connectionString);
            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(roomServiceLogger);
            _className = this.GetType().Name;
        }

        public Dashboard LoadDashboardData(int clientId)
        {
            try
            {
                return dashboardRepository.GetDashboardData(clientId);
            }
            catch (Exception ex)
            {

                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }
    }
}
