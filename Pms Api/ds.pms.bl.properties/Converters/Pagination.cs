using ds.pms.apicommon.Models;
using ds.pms.bl.properties.Models;
using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ds.pms.bl.properties.Converters
{
    public static class Pagination
    {
        public static PaginatedList<Property> ConvertDalToBl(PaginatedList<TblProperty> paginatedDbList)
        {
            PaginatedList<Property> paginatedList = new PaginatedList<Property>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlUserList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<Property> ConvertDalToBlUserList(List<TblProperty> dbList)
        {
            List<Property> blList = new List<Property>();
            if (dbList != null && dbList.Any())
            {
                dbList.ForEach(x => blList.Add(x));
            }
            return blList;
        }

        public static PaginatedList<Property> ConvertDalToBl(PaginatedList<PropertyList> paginatedDbList)
        {
            PaginatedList<Property> paginatedList = new PaginatedList<Property>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlUserList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<Property> ConvertDalToBlUserList(List<PropertyList> dbList)
        {
            List<Property> blList = new List<Property>();
            if (dbList != null && dbList.Any())
            {
                dbList.ForEach(x => blList.Add(x));
            }
            return blList;
        }
    }
}
