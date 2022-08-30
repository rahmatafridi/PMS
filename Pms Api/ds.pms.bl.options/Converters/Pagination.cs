using ds.pms.apicommon.Models;
using ds.pms.bl.options.Models;
using ds.pms.dal.Models;
using System.Collections.Generic;
using System.Linq;

namespace ds.pms.bl.options.Converters
{
    public static class Pagination
    {
        public static PaginatedList<Option> ConvertDalToBl(PaginatedList<TblOption> paginatedDbList)
        {
            PaginatedList<Option> paginatedList = new PaginatedList<Option>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlUserList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<Option> ConvertDalToBlUserList(List<TblOption> dbList)
        {
            List<Option> blList = new List<Option>();
            if (dbList != null && dbList.Any())
            {
                dbList.ForEach(x => blList.Add(x));
            }
            return blList;
        }
    }
}
