using ds.pms.apicommon.Models;
using ds.pms.bl.documents.Models;
using ds.pms.dal.Models;
using System.Collections.Generic;
using System.Linq;

namespace ds.pms.bl.documents.Converters
{
    public static class Pagination
    {
        public static PaginatedList<Document> ConvertDalToBl(PaginatedList<TblDocument> paginatedDbList)
        {
            PaginatedList<Document> paginatedList = new PaginatedList<Document>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlUserList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<Document> ConvertDalToBlUserList(List<TblDocument> dbList)
        {
            if (dbList != null && dbList.Any())
            {
                List<Document> blList = new List<Document>();
                dbList.ForEach(x => blList.Add(x));
                return blList;
            }
            return null;
        }
    }
}
