using ds.pms.apicommon.Models;
using ds.pms.bl.compliances.Models;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ds.pms.bl.compliances.Converters
{
    public static class Pagination
    {
        public static PaginatedList<Compliance> ConvertDalToBl(PaginatedList<TblCompliance> paginatedDbList)
        {
            PaginatedList<Compliance> paginatedList = new PaginatedList<Compliance>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlUserList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<Compliance> ConvertDalToBlUserList(List<TblCompliance> dbList)
        {
            if (dbList != null && dbList.Any())
            {
                List<Compliance> blList = new List<Compliance>();
                dbList.ForEach(x => blList.Add(x));
                return blList;
            }
            return null;
        }
    }
}
