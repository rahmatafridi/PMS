using ds.pms.apicommon.Models;
using ds.pms.dal.CustomModels;
using ds.pms.dal.GenericRepository;
using ds.pms.dal.Models;
using ds.pms.dal.SortFields;
using ds.pms.dal.SortFields.ds.pms.dal.SortFields;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ds.pms.dal.CustomRepository
{
     public class OptionHeaderRepository : BaseCustomRepository
    {
        private GenericRepository<TblOptionHeader> optionHeaderGenericRepository;
        public OptionHeaderRepository(string databaseProperty, string connectionString) : base(databaseProperty, connectionString)
        {
            optionHeaderGenericRepository = new GenericRepository<TblOptionHeader>(databaseProperty, connectionString);
        }
        public PaginatedList<OptionHeaderList> GetActiveOptionHeaderList(int clientId,string search, int limit = 10, int page = 1, string sortBy = "", string sortDir = "")
        {
            PaginatedList<OptionHeaderList> paginatedNotesCategorys = new PaginatedList<OptionHeaderList>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                OptionHeaderSortFields sortField = sortBy.OptionHeaderSortField();
                SortDirection sortDirection = sortDir.GetSortDirectionByName();

                //Select Query
                var listQuery = new List<OptionHeaderList>();
                var query = (from header in dataContext.TblOptionHeaders
                             from client in dataContext.TblClients.LeftJoin(c => c.Id == header.ClientId && c.IsActive && !c.IsDeleted)
                             where header.IsDeleted == false
                             select new OptionHeaderList
                             {
                                 ClientId = header.ClientId,
                                 ClientName = client.Name,
                                 Id = header.Id,
                                 Title = header.Title
                             });
                //var query = dataContext.TblOptionHeaders.Where(p => (bool)!p.IsDeleted);
                //foreach (var item in query)
                //{
                //    var client = dataContext.TblClients.Where(x => x.Id == item.ClientId).FirstOrDefault();
                //    listQuery.Add(new OptionHeaderList()
                //    {
                //        ClientId=item.ClientId,
                //        ClientName= client.Name,
                //        Id= item.Id,
                //        Title= item.Title
                //    });
                //}
                if (clientId !=0)
                {
                    query = query.Where(x => x.ClientId == clientId);
                }
              
                // Search - go deeper
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(p => (p.Title != null && p.Title.ToLower().Contains(search))
                                        );
                }

                // Sorting
                //if (sortField != OptionHeaderSortFields.None && sortDirection != SortDirection.None)
                //{
                //    if (sortDirection == SortDirection.Asc)
                //        query = query.OrderBy(sortField.GetColumn());
                //    else if (sortDirection == SortDirection.Desc)
                //        query = query.OrderByDescending(sortField.GetColumn());
                //}

                // Pagination
                paginatedNotesCategorys.TotalCount = query.LongCount();
                paginatedNotesCategorys.PageSize = limit;
                paginatedNotesCategorys.CurrentPage = page;
                query = query.Skip((page - 1) * limit).Take(limit);

                paginatedNotesCategorys.Items = query.ToList();

                return paginatedNotesCategorys;
            }
        }

        public TblOptionHeader GetOptionHeaderById(int notesCategoryId)
        {

            return optionHeaderGenericRepository.GetById(notesCategoryId);
        }

        public List<TblOptionHeader> GetActiveOptionHeaderListByClient(int clientId)
        {
            List<TblOptionHeader> list = new List<TblOptionHeader>();
            list = dataContext.TblOptionHeaders.Where(p => (bool)!p.IsDeleted && p.ClientId==clientId).ToList();
            return list;

        }
        public TblOptionHeader Add(TblOptionHeader addOptionHeader)
        {
            return optionHeaderGenericRepository.Insert(addOptionHeader);
        }

        public TblOptionHeader Update(TblOptionHeader update)
        {
            return optionHeaderGenericRepository.Update(update);
        }

        public bool Delete(int id)
        {
            return optionHeaderGenericRepository.DeleteById(id);
        }
    }
}
