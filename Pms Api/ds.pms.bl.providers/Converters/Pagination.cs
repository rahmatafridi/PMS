using ds.pms.apicommon.Models;
using ds.pms.bl.providers.Models;
using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System.Collections.Generic;
using System.Linq;

namespace ds.pms.bl.providers.Converters
{
    public static class Pagination
    {
        public static PaginatedList<Provider> ConvertDalToBl(PaginatedList<TblProvider> paginatedDbList)
        {
            PaginatedList<Provider> paginatedList = new PaginatedList<Provider>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlUserList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<Provider> ConvertDalToBlUserList(List<TblProvider> dbList)
        {
            List<Provider> blList = new List<Provider>();
            if (dbList != null && dbList.Any())
            {
                dbList.ForEach(x => blList.Add(x));
            }
            return blList;
        }

        public static PaginatedList<Provider> ConvertDalToBl(PaginatedList<ProviderList> paginatedDbList)
        {
            PaginatedList<Provider> paginatedList = new PaginatedList<Provider>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlUserList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<Provider> ConvertDalToBlUserList(List<ProviderList> dbList)
        {
            List<Provider> blList = new List<Provider>();
            if (dbList != null && dbList.Any())
            {
                dbList.ForEach(x => blList.Add(x));
            }
            return blList;
        }
    }
}
