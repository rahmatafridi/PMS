using ds.pms.apicommon.Models;
using ds.pms.dal.GenericRepository;
using ds.pms.dal.Models;
using ds.pms.dal.SortFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ds.pms.dal.CustomRepository
{
    public class NoteRepository : BaseCustomRepository
    {
        private GenericRepository<TblNote> noteGenericRepository;

        public NoteRepository(string databaseProperty, string connectionString) : base(databaseProperty, connectionString)
        {
            noteGenericRepository = new GenericRepository<TblNote>(databaseProperty, connectionString);
        }

        public PaginatedList<TblNote> GetActiveNoteList(int proId, string search, int limit = 10, int page = 1, string sortBy = "", string sortDir = "")
        {
            PaginatedList<TblNote> paginatedNotes = new PaginatedList<TblNote>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                NoteSortFields sortField = sortBy.GetNoteField();
                SortDirection sortDirection = sortDir.GetSortDirectionByName();

                //Select Query
                var query = dataContext.TblNotes.Where(p => !p.IsDeleted && p.ParentId== proId && p.TypeId==1);

                // Search - go deeper
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(p => (p.Note != null && p.Note.ToLower().Contains(search))
                                        );
                }

                // Sorting
                if (sortField != NoteSortFields.None && sortDirection != SortDirection.None)
                {
                    if (sortDirection == SortDirection.Asc)
                        query = query.OrderBy(sortField.GetColumn());
                    else if (sortDirection == SortDirection.Desc)
                        query = query.OrderByDescending(sortField.GetColumn());
                }

                // Pagination
                paginatedNotes.TotalCount = query.LongCount();
                paginatedNotes.PageSize = limit;
                paginatedNotes.CurrentPage = page;
                query = query.Skip((page - 1) * limit).Take(limit);

                paginatedNotes.Items = query.ToList();

                return paginatedNotes;
            }
        }

        public TblNote GetNoteById(int noteId,  int typeId)
        {
            var data = dataContext.TblNotes.FirstOrDefault(x => x.Id == noteId && x.TypeId == typeId);
            return data;
        }
        public TblNote GetNoteByParentId(int parentId, int typeId)
        {
            var data = dataContext.TblNotes.FirstOrDefault(x => x.ParentId == parentId && x.TypeId == typeId);
            return data;
        }
        public TblNote Add(TblNote addNote)
        {
            return noteGenericRepository.Insert(addNote);
        }

        public TblNote Update(TblNote updateNote)
        {
            return noteGenericRepository.Update(updateNote);
        }

        public bool Delete(int noteId)
        {
            return noteGenericRepository.DeleteById(noteId);
        }
    }
}
