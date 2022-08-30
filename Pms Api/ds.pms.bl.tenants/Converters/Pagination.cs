using ds.pms.apicommon.Models;
using ds.pms.bl.tenants.Models;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ds.pms.bl.tenants.Converters
{
    public static class Pagination
    {
        public static PaginatedList<Tenant> ConvertDalToBl(PaginatedList<TblTenant> paginatedDbList)
        {
            PaginatedList<Tenant> paginatedList = new PaginatedList<Tenant>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlTenantList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<Tenant> ConvertDalToBlTenantList(List<TblTenant> dbList)
        {
            if (dbList != null && dbList.Any())
            {
                List<Tenant> blList = new List<Tenant>();
                dbList.ForEach(x => blList.Add(x));
                return blList;
            }
            return null;
        }
    }
}
