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
    public class ComplianceRepository : BaseCustomRepository
    {
        private GenericRepository<TblCompliance> complianceGenericRepository;

        public ComplianceRepository(string databaseProvider, string connectionString) : base(databaseProvider, connectionString)
        {
            complianceGenericRepository = new GenericRepository<TblCompliance>(databaseProvider, connectionString);
        }

        public PaginatedList<TblCompliance> GetActiveComplianceList(string search, int limit = 10, int page = 1, string sortBy = "", string sortDir = "")
        {
            PaginatedList<TblCompliance> paginatedCompliances = new PaginatedList<TblCompliance>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                ComplianceSortFields sortField = sortBy.GetComplianceField();
                SortDirection sortDirection = sortDir.GetSortDirectionByName();

                //Select Query
                var query = dataContext.TblCompliances.Where(p => !p.IsDeleted);

                // Search - go deeper
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(p => p.Title != null && p.Title.ToLower().Contains(search)
                                        );
                }

                // Sorting
                if (sortField != ComplianceSortFields.None && sortDirection != SortDirection.None)
                {
                    if (sortDirection == SortDirection.Asc)
                        query = query.OrderBy(sortField.GetColumn());
                    else if (sortDirection == SortDirection.Desc)
                        query = query.OrderByDescending(sortField.GetColumn());
                }

                // Pagination
                paginatedCompliances.TotalCount = query.LongCount();
                paginatedCompliances.PageSize = limit;
                paginatedCompliances.CurrentPage = page;
                query = query.Skip((page - 1) * limit).Take(limit);

                paginatedCompliances.Items = query.ToList();

                return paginatedCompliances;
            }
        }
        public PaginatedList<TblCompliance> GetActiveComplianceByClient(int client)
        {
            PaginatedList<TblCompliance> paginatedCompliances = new PaginatedList<TblCompliance>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                

                //Select Query
                var query = dataContext.TblCompliances.Where(p => !p.IsDeleted);

              

            
               

                paginatedCompliances.Items = query.ToList();

                return paginatedCompliances;
            }
        }

        public TblCompliance GetComplianceById(int complianceId)
        {
            return complianceGenericRepository.GetById(complianceId);
        }

        public TblCompliance Add(TblCompliance addCompliance)
        {
            return complianceGenericRepository.Insert(addCompliance);
        }

        public TblCompliance Update(TblCompliance updateCompliance)
        {
            return complianceGenericRepository.Update(updateCompliance);
        }

        public bool Delete(int complianceId)
        {
            return complianceGenericRepository.DeleteById(complianceId);
        }
    }
}
