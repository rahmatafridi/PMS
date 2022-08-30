using ds.pms.apicommon.Models;
using ds.pms.bl.compliancepropertydocs.Models;
using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ds.pms.bl.compliancepropertydocs.Converters
{
    public static class Pagination
    {
        public static PaginatedList<PropertyCompianceDocsList> ConvertDalToBl(PaginatedList<PropertyCompianceDocsList> paginatedDbList)
        {
            PaginatedList<PropertyCompianceDocsList> paginatedList = new PaginatedList<PropertyCompianceDocsList>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlUserList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<PropertyCompianceDocsList> ConvertDalToBlUserList(List<PropertyCompianceDocsList> dbList)
        {
            if (dbList != null && dbList.Any())
            {
                List<PropertyCompianceDocsList> blList = new List<PropertyCompianceDocsList>();
                dbList.ForEach(x => blList.Add(x));
                return blList;
            }
            return null;
        }
    }
}
