using ds.pms.apicommon.Models;
using ds.pms.bl.notes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.notes.IServices
{
    public interface INoteService
    {
        PaginatedList<Note> GetActiveNoteList(int porId,string search, int? limit = 10, int? page = 1, string sort = "");
        Note GetNoteById(int noteId, int typeId);
        Note GetNoteByParent(int tenantId, int typeId);
        NoteCommonResponse AddNote(Note addNote, string userName);
        NoteCommonResponse UpdateNote(Note updateNote, string userName);
        NoteCommonResponse SoftDelete(int noteId, string userName);
        bool HardDelete(int noteId);
    }
}
