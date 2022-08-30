using ds.pms.apicommon.Models;
using ds.pms.bl.logger;
using ds.pms.bl.notescategory.IServices;
using ds.pms.bl.notescategory.Models;
using ds.pms.bl.NotesCategoryscategory.Converters;
using ds.pms.dal.CustomRepository;
using ds.pms.dal.Models;
using ds.pms.helpers.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace ds.pms.bl.notescategory.Services
{
    public class NotesCategoryService : INotesCategoryService
    {
        private NotesCategoryRepository notesCategoryRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public NotesCategoryService(string _databaseProvider, string _connectionString, ILogger<NotesCategoryService> notesCategoryServicelogger)
        {
            notesCategoryRepository = new NotesCategoryRepository(_databaseProvider, _connectionString);
            logging = new Logging(notesCategoryServicelogger);
            _className = this.GetType().Name;
        }

        public NotesCategoryService(string _databaseProvider, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds, ILogger<NotesCategoryService> notesCategoryServicelogger)
        {
            notesCategoryRepository = new NotesCategoryRepository(_databaseProvider, _connectionString);
            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(notesCategoryServicelogger);
            _className = this.GetType().Name;
        }

        public PaginatedList<NotesCategory> GetActiveNotesCategoryList(string search, int? limit = 10, int? page = 1, string sort = "")
        {
            PaginatedList<NotesCategory> notesCategoriesPaginatedList = new PaginatedList<NotesCategory>();
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
                notesCategoriesPaginatedList = Pagination.ConvertDalToBl(notesCategoryRepository.GetActiveNotesCategoryList(search, pageSize, pageNumber, sortBy, sortDirection));

            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return notesCategoriesPaginatedList;
        }

        public NotesCategory GetNotesCategoryById(int notesCategoryId)
        {
            try
            {
                if (notesCategoryId > 0)
                {
                    NotesCategory notesCategory = notesCategoryRepository.GetNotesCategoryById(notesCategoryId);
                    if (notesCategory != null)
                        return notesCategory;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        public NotesCategoryCommonResponse AddNotesCategory(NotesCategory addNotesCategory, string userName)
        {
            NotesCategoryCommonResponse notesCategoryCommonResponse = new NotesCategoryCommonResponse();
            try
            {
                notesCategoryCommonResponse.Success = false;
                if (addNotesCategory != null)
                {
                    TblNotesCategory tblNotesCategory = addNotesCategory;
                    tblNotesCategory.AddedBy = userName;
                    tblNotesCategory.AddedDate = DateTime.UtcNow;
                    notesCategoryCommonResponse.NotesCategory = notesCategoryRepository.Add(tblNotesCategory);
                    if (notesCategoryCommonResponse.NotesCategory != null && notesCategoryCommonResponse.NotesCategory.Id > 0)
                    {
                        notesCategoryCommonResponse.Success = true;
                        return notesCategoryCommonResponse;
                    }
                    else
                        notesCategoryCommonResponse.Message = "Unable to add notesCategory.";
                }
                else
                    notesCategoryCommonResponse.Message = "Supplied notesCategory information is not valid.";
            }
            catch (Exception ex)
            {
                notesCategoryCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return notesCategoryCommonResponse;
        }

        public NotesCategoryCommonResponse UpdateNotesCategory(NotesCategory updateNotesCategory, string userName)
        {
            NotesCategoryCommonResponse notesCategoryCommonResponse = new NotesCategoryCommonResponse();
            try
            {
                notesCategoryCommonResponse.Success = false;
                if (updateNotesCategory != null && updateNotesCategory.Id > 0)
                {
                    TblNotesCategory tblNotesCategory;
                    tblNotesCategory = notesCategoryRepository.GetNotesCategoryById(updateNotesCategory.Id);
                    if (tblNotesCategory != null)
                    {
                        tblNotesCategory.ClientId = updateNotesCategory.ClientId;
                        tblNotesCategory.ParentId = updateNotesCategory.ParentId;
                        tblNotesCategory.TypeId = updateNotesCategory.TypeId;
                        tblNotesCategory.Name = updateNotesCategory.Name;
                        tblNotesCategory.ModifiedBy = userName;
                        tblNotesCategory.ModifiedDate = DateTime.UtcNow;
                        notesCategoryCommonResponse.NotesCategory = notesCategoryRepository.Update(tblNotesCategory);
                        if (notesCategoryCommonResponse.NotesCategory != null && notesCategoryCommonResponse.NotesCategory.Id > 0)
                        {
                            notesCategoryCommonResponse.Success = true;
                            return notesCategoryCommonResponse;
                        }
                        else
                            notesCategoryCommonResponse.Message = "Unable to update notesCategory.";
                    }
                    else
                        notesCategoryCommonResponse.Message = "NotesCategory does not exists.";
                }
                else
                    notesCategoryCommonResponse.Message = "Supplied notesCategory information is not valid.";
            }
            catch (Exception ex)
            {
                notesCategoryCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return notesCategoryCommonResponse;
        }

        public NotesCategoryCommonResponse SoftDelete(int NotesCategoryId, string userName)
        {
            NotesCategoryCommonResponse notesCategoryCommonResponse = new NotesCategoryCommonResponse();
            try
            {
                notesCategoryCommonResponse.Success = false;
                if (NotesCategoryId > 0)
                {
                    TblNotesCategory tblNotesCategory = notesCategoryRepository.GetNotesCategoryById(NotesCategoryId);
                    if (tblNotesCategory != null && tblNotesCategory.Id > 0)
                    {
                        tblNotesCategory.ModifiedBy = userName;
                        tblNotesCategory.ModifiedDate = DateTime.UtcNow;
                        tblNotesCategory.IsDeleted = true;
                        tblNotesCategory.DeletedBy = userName;
                        tblNotesCategory.DeletedDate = DateTime.Now;
                        notesCategoryCommonResponse.NotesCategory = notesCategoryRepository.Update(tblNotesCategory);
                        if (notesCategoryCommonResponse.NotesCategory != null && notesCategoryCommonResponse.NotesCategory.Id > 0)
                        {
                            notesCategoryCommonResponse.Success = true;
                            return notesCategoryCommonResponse;
                        }
                        else
                            notesCategoryCommonResponse.Message = "Unable to delete notesCategory.";
                    }
                    else
                        notesCategoryCommonResponse.Message = "NotesCategory does not exists.";
                }
                else
                    notesCategoryCommonResponse.Message = "Supplied notesCategory information is not valid.";
            }
            catch (Exception ex)
            {
                notesCategoryCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {

            }
            return notesCategoryCommonResponse;
        }

        public bool HardDelete(int notesCategoryId)
        {
            try
            {
                if (notesCategoryId > 0)
                {
                    return notesCategoryRepository.Delete(notesCategoryId);
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return false;
        }
    }
}
