using ds.pms.apicommon.Models;
using ds.pms.bl.configs.Models;
using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ds.pms.bl.configs.Converters
{
    public static class Pagination
    {
        public static PaginatedList<Config> ConvertDalToBl(PaginatedList<TblConfig> paginatedDbList)
        {
            PaginatedList<Config> paginatedList = new PaginatedList<Config>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlUserList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<Config> ConvertDalToBlUserList(List<TblConfig> dbList)
        {
            if (dbList != null && dbList.Any())
            {
                List<Config> blList = new List<Config>();
                dbList.ForEach(x => blList.Add(x));
                return blList;
            }
            return null;
        }

        public static PaginatedList<Config> ConvertDalToBlUserList(PaginatedList<ConfigList> paginatedDbList)
        {
            PaginatedList<Config> paginatedList = new PaginatedList<Config>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlRoleList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<Config> ConvertDalToBlRoleList(List<ConfigList> dbList)
        {
            List<Config> blList = new List<Config>();
            if (dbList != null && dbList.Any())
            {
                dbList.ForEach(x => blList.Add(x));
            }
            return blList;
        }
    }
}
