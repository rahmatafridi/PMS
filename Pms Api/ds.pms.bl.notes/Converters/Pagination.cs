using ds.pms.apicommon.Models;
using ds.pms.bl.notes.Models;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ds.pms.bl.notes.Converters
{
    public static class Pagination
    {
        public static PaginatedList<Note> ConvertDalToBl(PaginatedList<TblNote> paginatedDbList)
        {
            PaginatedList<Note> paginatedList = new PaginatedList<Note>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlUserList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<Note> ConvertDalToBlUserList(List<TblNote> dbList)
        {
            if (dbList != null && dbList.Any())
            {
                List<Note> blList = new List<Note>();
                dbList.ForEach(x => blList.Add(x));
                return blList;
            }
            return null;
        }
    }
}
