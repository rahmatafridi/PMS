using ds.pms.apicommon.Models;
using ds.pms.dal.GenericRepository;
using ds.pms.dal.Models;
using ds.pms.dal.SortFields;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ds.pms.dal.CustomRepository
{
   public class OptionRepository : BaseCustomRepository
    {
        private GenericRepository<TblOption> optionGenericRepository;
        public OptionRepository(string databaseProperty, string connectionString) : base(databaseProperty, connectionString)
        {
            optionGenericRepository = new GenericRepository<TblOption>(databaseProperty, connectionString);
        }
        public PaginatedList<TblOption> GetActiveOptionList(string search, int limit = 10, int page = 1, string sortBy = "", string sortDir = "")
        {
            PaginatedList<TblOption> paginatedOption = new PaginatedList<TblOption>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                OptionSortFields sortField = sortBy.OptionSortField();
                SortDirection sortDirection = sortDir.GetSortDirectionByName();

                //Select Query
                var query = dataContext.TblOptions.Where(p => (bool)!p.IsDeleted);

                // Search - go deeper
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(p => (p.Title != null && p.Title.ToLower().Contains(search))
                                        );
                }

                // Sorting
                if (sortField != OptionSortFields.None && sortDirection != SortDirection.None)
                {
                    if (sortDirection == SortDirection.Asc)
                        query = query.OrderBy(sortField.GetColumn());
                    else if (sortDirection == SortDirection.Desc)
                        query = query.OrderByDescending(sortField.GetColumn());
                }

                // Pagination
                paginatedOption.TotalCount = query.LongCount();
                paginatedOption.PageSize = limit;
                paginatedOption.CurrentPage = page;
                query = query.Skip((page - 1) * limit).Take(limit);

                paginatedOption.Items = query.ToList();

                return paginatedOption;
            }
        }
        public bool IsValidOption(int id, int headerId, string value,string title)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                if (id != 0)
                {
                    return dataContext.TblOptions.Any(x => x.HeaderId == headerId && (x.Value == value || x.Title == title) && x.Id != id);

                }
                else
                {
                  return dataContext.TblOptions.Any(x => x.HeaderId == headerId && (x.Value == value || x.Title == title));
                }

                
              
            }
        }
        public TblOption GetOptionById(int id)
        {
            return optionGenericRepository.GetById(id);
        }
        public List<TblOption> LoadOption(string header)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {

                var query = (from option in dataContext.TblOptions
                             from he in dataContext.TblOptionHeaders.InnerJoin(c => c.Id == option.HeaderId)
                             where he.Title==header
                             select new TblOption
                             {
                               
                                 Id = option.Id,
                                 Title = option.Title
                             });
                return query.ToList();
            }
            
        }
        public List<TblOption> GetOptionListById(int id)
        {
            return dataContext.TblOptions.Where(x => x.HeaderId == id && x.IsDeleted==false).ToList();
        }
        public TblOption Add(TblOption addOptionHeader)
        {
            return optionGenericRepository.Insert(addOptionHeader);
        }

        public TblOption Update(TblOption update)
        {
            return optionGenericRepository.Update(update);
        }

        public bool Delete(int id)
        {
            return optionGenericRepository.DeleteById(id);
        }
    }
}
