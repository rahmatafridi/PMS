using ds.pms.apicommon.Models;
using ds.pms.bl.notescategory.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.notescategory.IServices
{
    public interface INotesCategoryService
    {
        PaginatedList<NotesCategory> GetActiveNotesCategoryList(string search, int? limit = 10, int? page = 1, string sort = "");
        NotesCategory GetNotesCategoryById(int notesCategoryId);
        NotesCategoryCommonResponse AddNotesCategory(NotesCategory addNotesCategory, string userName);
        NotesCategoryCommonResponse UpdateNotesCategory(NotesCategory updateNotesCategory, string userName);
        NotesCategoryCommonResponse SoftDelete(int notesCategoryId, string userName);
        bool HardDelete(int notesCategoryId);
    }
}
