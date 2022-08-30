using ds.pms.apicommon.Models;
using ds.pms.bl.logger;
using ds.pms.bl.notes.Converters;
using ds.pms.bl.notes.IServices;
using ds.pms.bl.notes.Models;
using ds.pms.dal.CustomRepository;
using ds.pms.dal.Models;
using ds.pms.helpers.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace ds.pms.bl.notes.Services
{
    public class NoteService : INoteService
    {
        private NoteRepository noteRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public NoteService(string _databaseProvider, string _connectionString, ILogger<NoteService> noteServicelogger)
        {
            noteRepository = new NoteRepository(_databaseProvider, _connectionString);
            logging = new Logging(noteServicelogger);
            _className = this.GetType().Name;
        }

        public NoteService(string _databaseProvider, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds, ILogger<NoteService> noteServicelogger)
        {
            noteRepository = new NoteRepository(_databaseProvider, _connectionString);
            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(noteServicelogger);
            _className = this.GetType().Name;
        }

        public PaginatedList<Note> GetActiveNoteList(int proId, string search, int? limit = 10, int? page = 1, string sort = "")
        {
            PaginatedList<Note> notesPaginatedList = new PaginatedList<Note>();
            try
            {
                int pageSize = limit ?? 10;
                int pageNumber = page ?? 1;
                string sortBy = string.Empty, sortDirection = string.Empty;
                if (!string.IsNullOrEmpty(sort))
                {
                    string[] sorting = SortDirection.SortDir(sort);
                    if (sorting.Length > 1)
                    {
                        sortBy = sorting[0];
                        sortDirection = sorting[1];
                    }
                }
                notesPaginatedList = Pagination.ConvertDalToBl(noteRepository.GetActiveNoteList(proId,search, pageSize, pageNumber, sortBy, sortDirection));

            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return notesPaginatedList;
        }

        public Note GetNoteById(int noteId,int typeId)
        {
            try
            {
                if (noteId > 0)
                {
                    Note note = noteRepository.GetNoteById(noteId,typeId);
                    if (note != null)
                        return note;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        public NoteCommonResponse AddNote(Note addNote, string userName)
        {
            NoteCommonResponse noteCommonResponse = new NoteCommonResponse();
            try
            {
                noteCommonResponse.Success = false;
                if (addNote != null)
                {
                    TblNote tblNote = addNote;
                    tblNote.AddedBy = userName;
                    tblNote.AddedDate = DateTime.UtcNow;
                    noteCommonResponse.Note = noteRepository.Add(tblNote);
                    if (noteCommonResponse.Note != null && noteCommonResponse.Note.Id > 0)
                    {
                        noteCommonResponse.Success = true;
                        return noteCommonResponse;
                    }
                    else
                        noteCommonResponse.Message = "Unable to add note.";
                }
                else
                    noteCommonResponse.Message = "Supplied note information is not valid.";
            }
            catch (Exception ex)
            {
                noteCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return noteCommonResponse;
        }

        public NoteCommonResponse UpdateNote(Note updateNote, string userName)
        {
            NoteCommonResponse noteCommonResponse = new NoteCommonResponse();
            try
            {
                noteCommonResponse.Success = false;
                if (updateNote != null && updateNote.Id > 0)
                {
                    TblNote tblNote;
                    tblNote = noteRepository.GetNoteById(updateNote.Id,1);
                    if (tblNote != null)
                    {
                        tblNote.NoteCategoryId = updateNote.NoteCategoryId;
                        tblNote.ParentId = updateNote.ParentId;
                        tblNote.TypeId = updateNote.TypeId;
                        tblNote.Note = updateNote.Notes;
                        tblNote.ModifiedBy = userName;
                        tblNote.ModifiedDate = DateTime.UtcNow;
                        noteCommonResponse.Note = noteRepository.Update(tblNote);
                        if (noteCommonResponse.Note != null && noteCommonResponse.Note.Id > 0)
                        {
                            noteCommonResponse.Success = true;
                            return noteCommonResponse;
                        }
                        else
                            noteCommonResponse.Message = "Unable to update note.";
                    }
                    else
                        noteCommonResponse.Message = "Note does not exists.";
                }
                else
                    noteCommonResponse.Message = "Supplied note information is not valid.";
            }
            catch (Exception ex)
            {
                noteCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return noteCommonResponse;
        }

        public NoteCommonResponse SoftDelete(int noteId, string userName)
        {
            NoteCommonResponse noteCommonResponse = new NoteCommonResponse();
            try
            {
                noteCommonResponse.Success = false;
                if (noteId > 0)
                {
                    TblNote tblNote = noteRepository.GetNoteById(noteId,1);
                    if (tblNote != null && tblNote.Id > 0)
                    {
                        tblNote.ModifiedBy = userName;
                        tblNote.ModifiedDate = DateTime.UtcNow;
                        tblNote.IsDeleted = true;
                        tblNote.DeletedBy = userName;
                        tblNote.DeletedDate = DateTime.Now;
                        noteCommonResponse.Note = noteRepository.Update(tblNote);
                        if (noteCommonResponse.Note != null && noteCommonResponse.Note.Id > 0)
                        {
                            noteCommonResponse.Success = true;
                            return noteCommonResponse;
                        }
                        else
                            noteCommonResponse.Message = "Unable to delete note.";
                    }
                    else
                        noteCommonResponse.Message = "Note does not exists.";
                }
                else
                    noteCommonResponse.Message = "Supplied note information is not valid.";
            }
            catch (Exception ex)
            {
                noteCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {

            }
            return noteCommonResponse;
        }

        public bool HardDelete(int noteId)
        {
            try
            {
                if (noteId > 0)
                {
                    return noteRepository.Delete(noteId);
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return false;
        }

        public Note GetNoteByParent(int tenantId, int typeId)
        {
            try
            {
                if (tenantId > 0)
                {
                    Note note = noteRepository.GetNoteByParentId(tenantId, typeId);
                    if (note != null)
                        return note;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }
    }
}
