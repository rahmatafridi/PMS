using ds.pms.apicommon.Models;
using ds.pms.bl.optionheaders.Models;
using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System.Collections.Generic;
using System.Linq;

namespace ds.pms.bl.optionheaders.Converters
{
    public static class Pagination
    {
        public static PaginatedList<OptionHeaderList> ConvertDalToBl(PaginatedList<OptionHeaderList> paginatedDbList)
        {
            PaginatedList<OptionHeaderList> paginatedList = new PaginatedList<OptionHeaderList>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlUserList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<OptionHeaderList> ConvertDalToBlUserList(List<OptionHeaderList> dbList)
        {
            List<OptionHeaderList> blList = new List<OptionHeaderList>();
            if (dbList != null && dbList.Any())
            {
                dbList.ForEach(x => blList.Add(x));
            }
            return blList;
        }
    }
}
