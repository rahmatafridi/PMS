using ds.pms.apicommon.Models;
using ds.pms.bl.roles.Models;
using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System.Collections.Generic;
using System.Linq;

namespace ds.pms.bl.roles.Converters
{
    public static class Pagination
    {
        public static PaginatedList<Role> ConvertDalToBl(PaginatedList<TblRole> paginatedDbList)
        {
            PaginatedList<Role> paginatedList = new PaginatedList<Role>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlRoleList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<Role> ConvertDalToBlRoleList(List<TblRole> dbList)
        {
            List<Role> blList = new List<Role>();
            if (dbList != null && dbList.Any())
            {
                dbList.ForEach(x => blList.Add(x));
            }
            return blList;
        }

        public static PaginatedList<Role> ConvertDalToBl(PaginatedList<RoleList> paginatedDbList)
        {
            PaginatedList<Role> paginatedList = new PaginatedList<Role>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlRoleList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<Role> ConvertDalToBlRoleList(List<RoleList> dbList)
        {
            List<Role> blList = new List<Role>();
            if (dbList != null && dbList.Any())
            {
                dbList.ForEach(x => blList.Add(x));
            }
            return blList;
        }
    }
}
