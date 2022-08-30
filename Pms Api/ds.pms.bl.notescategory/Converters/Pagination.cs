using ds.pms.apicommon.Models;
using ds.pms.bl.notescategory.Models;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ds.pms.bl.NotesCategoryscategory.Converters
{
    public static class Pagination
    {
        public static PaginatedList<NotesCategory> ConvertDalToBl(PaginatedList<TblNotesCategory> paginatedDbList)
        {
            PaginatedList<NotesCategory> paginatedList = new PaginatedList<NotesCategory>();
            paginatedList.TotalCount = paginatedDbList.TotalCount;
            paginatedList.CurrentPage = paginatedDbList.CurrentPage;
            paginatedList.PageSize = paginatedDbList.PageSize;
            paginatedList.Items = ConvertDalToBlUserList(paginatedDbList.Items);
            return paginatedList;
        }

        public static List<NotesCategory> ConvertDalToBlUserList(List<TblNotesCategory> dbList)
        {
            if (dbList != null && dbList.Any())
            {
                List<NotesCategory> blList = new List<NotesCategory>();
                dbList.ForEach(x => blList.Add(x));
                return blList;
            }
            return null;
        }
    }
}
