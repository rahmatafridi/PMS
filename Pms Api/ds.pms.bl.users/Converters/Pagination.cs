using ds.pms.apicommon.Models;
using ds.pms.bl.users.Models;
using ds.pms.dal.Models;
using System.Collections.Generic;
using System.Linq;

namespace ds.pms.bl.users.Converters
{
    public static class Pagination
    {
        public static PaginatedList<T2> Convert<T1, T2>(PaginatedList<T1> paginatedDbList)
                 where T1 : class, T2
                 where T2 : class
        {
            PaginatedList<T2> paginatedList = new PaginatedList<T2>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            if (paginatedDbList.Items != null && paginatedDbList.Items.Any())
            {
                paginatedList.Items = new List<T2>();
                paginatedDbList.Items.ForEach(x => paginatedList.Items.Add(x));
            }
            return paginatedList;
        }

        public static PaginatedList<User> ConvertDalToBl(PaginatedList<TblUser> paginatedDbList)
        {
            PaginatedList<User> paginatedList = new PaginatedList<User>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlUserList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<User> ConvertDalToBlUserList(List<TblUser> dbList)
        {
            List<User> blList = new List<User>();
            if (dbList != null && dbList.Any())
            {
                dbList.ForEach(x => blList.Add(x));
            }
            return blList;
        }
    }
}
