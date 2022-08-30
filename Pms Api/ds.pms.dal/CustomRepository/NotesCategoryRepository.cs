using ds.pms.apicommon.Models;
using ds.pms.dal.GenericRepository;
using ds.pms.dal.Models;
using ds.pms.dal.SortFields;
using System.Linq;

namespace ds.pms.dal.CustomRepository
{
    public class NotesCategoryRepository : BaseCustomRepository
    {
        private GenericRepository<TblNotesCategory> notesCategoryGenericRepository;

        public NotesCategoryRepository(string databaseProperty, string connectionString) : base(databaseProperty, connectionString)
        {
            notesCategoryGenericRepository = new GenericRepository<TblNotesCategory>(databaseProperty, connectionString);
        }

        public PaginatedList<TblNotesCategory> GetActiveNotesCategoryList(string search, int limit = 10, int page = 1, string sortBy = "", string sortDir = "")
        {
            PaginatedList<TblNotesCategory> paginatedNotesCategorys = new PaginatedList<TblNotesCategory>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                NotesCategorySortFields sortField = sortBy.GetNotesCategoryField();
                SortDirection sortDirection = sortDir.GetSortDirectionByName();

                //Select Query
                var query = dataContext.TblNotesCategories.Where(p => !p.IsDeleted);

                // Search - go deeper
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(p => (p.Name != null && p.Name.ToLower().Contains(search))
                                        );
                }

                // Sorting
                if (sortField != NotesCategorySortFields.None && sortDirection != SortDirection.None)
                {
                    if (sortDirection == SortDirection.Asc)
                        query = query.OrderBy(sortField.GetColumn());
                    else if (sortDirection == SortDirection.Desc)
                        query = query.OrderByDescending(sortField.GetColumn());
                }

                // Pagination
                paginatedNotesCategorys.TotalCount = query.LongCount();
                paginatedNotesCategorys.PageSize = limit;
                paginatedNotesCategorys.CurrentPage = page;
                query = query.Skip((page - 1) * limit).Take(limit);

                paginatedNotesCategorys.Items = query.ToList();

                return paginatedNotesCategorys;
            }
        }

        public TblNotesCategory GetNotesCategoryById(int notesCategoryId)
        {
            return notesCategoryGenericRepository.GetById(notesCategoryId);
        }

        public TblNotesCategory Add(TblNotesCategory addNotesCategory)
        {
            return notesCategoryGenericRepository.Insert(addNotesCategory);
        }

        public TblNotesCategory Update(TblNotesCategory updateNotesCategory)
        {
            return notesCategoryGenericRepository.Update(updateNotesCategory);
        }

        public bool Delete(int notesCategoryId)
        {
            return notesCategoryGenericRepository.DeleteById(notesCategoryId);
        }
    }
}
