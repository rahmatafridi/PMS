using ds.pms.apicommon.Models;
using ds.pms.bl.rooms.Models;
using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ds.pms.bl.rooms.Converters
{
    public static class Pagination
    {
        public static PaginatedList<Room> ConvertDalToBl(PaginatedList<TblRoom> paginatedDbList)
        {
            PaginatedList<Room> paginatedList = new PaginatedList<Room>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlUserList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<Room> ConvertDalToBlUserList(List<TblRoom> dbList)
        {
            if (dbList != null && dbList.Any())
            {
                List<Room> blList = new List<Room>();
                dbList.ForEach(x => blList.Add(x));
                return blList;
            }
            return null;
        }

        public static PaginatedList<Room> ConvertDalToBlList(PaginatedList<RoomList> paginatedDbList)
        {
            PaginatedList<Room> paginatedList = new PaginatedList<Room>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlRoleList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<Room> ConvertDalToBlRoleList(List<RoomList> dbList)
        {
            List<Room> blList = new List<Room>();
            if (dbList != null && dbList.Any())
            {
                dbList.ForEach(x => blList.Add(x));
            }
            return blList;
        }
    }
}
