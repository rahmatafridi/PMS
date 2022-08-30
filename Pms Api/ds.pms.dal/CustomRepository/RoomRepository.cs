using ds.pms.apicommon.Models;
using ds.pms.dal.CustomModels;
using ds.pms.dal.GenericRepository;
using ds.pms.dal.Models;
using ds.pms.dal.SortFields;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ds.pms.dal.CustomRepository
{
   public class RoomRepository : BaseCustomRepository
    {
        private GenericRepository<TblRoom> roomGenericRepository;
        private GenericRepository<TblRoomsHistory> roomHistoryGenericRepository;

        public RoomRepository(string databaseProvider, string connectionString) : base(databaseProvider, connectionString)
        {
            roomGenericRepository = new GenericRepository<TblRoom>(databaseProvider, connectionString);
            roomHistoryGenericRepository = new GenericRepository<TblRoomsHistory>(databaseProvider, connectionString);

        }

        public PaginatedList<RoomList> GetActiveRoomList(int proId,string search, int limit = 10, int page = 1, string sortBy = "", string sortDir = "")
        {
            PaginatedList<RoomList> paginatedRooms = new PaginatedList<RoomList>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                RoomSortFields sortField = sortBy.GetRoomField();
                SortDirection sortDirection = sortDir.GetSortDirectionByName();

                //Select Query
               // var query = dataContext.TblRooms.Where(p => !p.IsDeleted && p.PropertyId== proId);

                var query = (from room in dataContext.TblRooms
                             from tenant in dataContext.TblTenants.LeftJoin(c => c.Id == room.TenantId)
                             where !room.IsDeleted && room.PropertyId== proId
                             select new RoomList
                             {
                                Tenant= tenant.FirstName +" "+tenant.LastName,
                                Id= room.Id,
                                CoreRent= room.CoreRent,
                                PersonalCharge = room.PersonalCharge,
                                TenancyStartDate= room.TenancyStartDate,
                                RoomName= room.RoomName,
                                RoomNo= room.RoomNo,
                                ServiceCharge= room.ServiceCharge,
                                 

                             });

                // Search - go deeper
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(p => (p.RoomName != null && p.RoomName.ToLower().Contains(search))
                                        );
                }

                // Sorting
                //if (sortField != RoomSortFields.None && sortDirection != SortDirection.None)
                //{
                //    if (sortDirection == SortDirection.Asc)
                //        query = query.OrderBy(sortField.GetColumn());
                //    else if (sortDirection == SortDirection.Desc)
                //        query = query.OrderByDescending(sortField.GetColumn());
                //}

                // Pagination
                paginatedRooms.TotalCount = query.LongCount();
                paginatedRooms.PageSize = limit;
                paginatedRooms.CurrentPage = page;
                query = query.Skip((page - 1) * limit).Take(limit);

                paginatedRooms.Items = query.ToList();

                return paginatedRooms;
            }
        }

        public TblRoom GetRoomById(int roomId)
        {
            return roomGenericRepository.GetById(roomId);
        }

        public TblRoom Add(TblRoom addRoom)
        {
            return roomGenericRepository.Insert(addRoom);
        }
        public TblRoomsHistory AddHistory(TblRoomsHistory addRoom)
        {
            return roomHistoryGenericRepository.Insert(addRoom);
        }
        public TblRoom Update(TblRoom updateRoom)
        {
            return roomGenericRepository.Update(updateRoom);
        }

        public bool Delete(int roomId)
        {
            return roomGenericRepository.DeleteById(roomId);
        }
    }
}
