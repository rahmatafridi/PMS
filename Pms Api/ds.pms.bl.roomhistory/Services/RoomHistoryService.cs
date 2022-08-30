using ds.pms.apicommon.Models;
using ds.pms.bl.logger;
using ds.pms.bl.roomhistory.IServices;
using ds.pms.bl.roomhistory.Models;
using ds.pms.bl.roomshistory.Converters;
using ds.pms.dal.CustomRepository;
using ds.pms.dal.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace ds.pms.bl.roomhistory.Services
{
    public class RoomHistoryService : IRoomHistoryService
    {
        private RoomHistoryRepository roomRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public RoomHistoryService(string _databaseProvider, string _connectionString, ILogger<RoomHistoryService> roomServiceLogger)
        {
            roomRepository = new RoomHistoryRepository(_databaseProvider, _connectionString);
            logging = new Logging(roomServiceLogger);
            _className = this.GetType().Name;
        }

        public RoomHistoryService(string _databaseProvider, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds
            , ILogger<RoomHistoryService> roomServiceLogger)
        {
            roomRepository = new RoomHistoryRepository(_databaseProvider, _connectionString);
            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(roomServiceLogger);
            _className = this.GetType().Name;
        }

        public PaginatedList<RoomHistory> GetRoomHistoryList(int tenantId)
        {
            PaginatedList<RoomHistory> roomsPaginatedList = new PaginatedList<RoomHistory>();
            try
            {
                
               
                roomsPaginatedList = Pagination.ConvertDalToBlList(roomRepository.GetActiveRoomList(tenantId));
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return roomsPaginatedList;
        }

    }
}
