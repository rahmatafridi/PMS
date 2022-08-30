using ds.pms.apicommon.Models;
using ds.pms.bl.roomhistory.Models;
using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ds.pms.bl.roomshistory.Converters
{
    public static class Pagination
    {
        public static PaginatedList<RoomHistory> ConvertDalToBl(PaginatedList<TblRoomsHistory> paginatedDbList)
        {
            PaginatedList<RoomHistory> paginatedList = new PaginatedList<RoomHistory>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlUserList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<RoomHistory> ConvertDalToBlUserList(List<TblRoomsHistory> dbList)
        {
            if (dbList != null && dbList.Any())
            {
                List<RoomHistory> blList = new List<RoomHistory>();
                dbList.ForEach(x => blList.Add(x));
                return blList;
            }
            return null;
        }

        public static PaginatedList<RoomHistory> ConvertDalToBlList(PaginatedList<RoomHistoryList> paginatedDbList)
        {
            PaginatedList<RoomHistory> paginatedList = new PaginatedList<RoomHistory>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlRoleList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<RoomHistory> ConvertDalToBlRoleList(List<RoomHistoryList> dbList)
        {
            List<RoomHistory> blList = new List<RoomHistory>();
            if (dbList != null && dbList.Any())
            {
                dbList.ForEach(x => blList.Add(x));
            }
            return blList;
        }
    }
}
