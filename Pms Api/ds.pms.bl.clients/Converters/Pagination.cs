using ds.pms.apicommon.Models;
using ds.pms.bl.clients.Models;
using ds.pms.dal.Models;
using System.Collections.Generic;
using System.Linq;

namespace ds.pms.bl.clients.Converters
{
    public static class Pagination
    {
        public static PaginatedList<Client> ConvertDalToBl(PaginatedList<TblClient> paginatedDbList)
        {
            PaginatedList<Client> paginatedList = new PaginatedList<Client>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlUserList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<Client> ConvertDalToBlUserList(List<TblClient> dbList)
        {
            List<Client> blList = new List<Client>();
            if (dbList != null && dbList.Any())
            {
                dbList.ForEach(x => blList.Add(x));
            }
            return blList;
        }
    }
}
