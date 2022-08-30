using ds.pms.apicommon.Models;
using ds.pms.dal.CustomModels;
using ds.pms.dal.GenericRepository;
using ds.pms.dal.Models;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ds.pms.dal.CustomRepository
{
    public class RoomHistoryRepository : BaseCustomRepository
    {
        private GenericRepository<TblRoomsHistory> roomHistoryGenericRepository;
        public RoomHistoryRepository(string databaseProvider, string connectionString) : base(databaseProvider, connectionString)
        {
            roomHistoryGenericRepository = new GenericRepository<TblRoomsHistory>(databaseProvider, connectionString);

        }
        public PaginatedList<RoomHistoryList> GetActiveRoomList(int tenantId)
        {
            PaginatedList<RoomHistoryList> paginatedRooms = new PaginatedList<RoomHistoryList>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {


                //Select Query
                // var query = dataContext.TblRooms.Where(p => !p.IsDeleted && p.PropertyId== proId);
                var query = (from room in dataContext.TblRoomsHistories
                             from prop in  dataContext.TblProperties.LeftJoin(c=> c.Id== room.PropertyId)
                             where room.TenantId== tenantId
                             select new RoomHistoryList
                             {
                                 Id = room.Id,
                                 IsTenantIsMoving = room.IsTenantIsMoving,
                                 IsTenantLeaving= room.IsTenantLeaving,
                                 Property= prop.Address1,
                                 TenancyEndDate= room.TenancyEndDate,
                                 TenancyStartDate = room.TenancyStartDate,
                                 RoomName = room.RoomName,
                                 RoomNo = room.RoomNo,


                             });
                paginatedRooms.Items = query.ToList();

                return paginatedRooms;
            }
        }

    }
}
