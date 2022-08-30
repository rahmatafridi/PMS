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
    public class TenantRepository : BaseCustomRepository
    {
        private GenericRepository<TblTenant> tenantGenericRepository;

        public TenantRepository(string databaseProvider, string connectionString) : base(databaseProvider, connectionString)
        {
            tenantGenericRepository = new GenericRepository<TblTenant>(databaseProvider, connectionString);
        }

        public PaginatedList<TblTenant> GetActiveTenantList(int clientId, string search, int limit = 10, int page = 1, string sortBy = "", string sortDir = "")
        {
            PaginatedList<TblTenant> paginatedTenants = new PaginatedList<TblTenant>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                TenantSortFields sortField = sortBy.GetTenantField();
                SortDirection sortDirection = sortDir.GetSortDirectionByName();

                //Select Query
                var query = dataContext.TblTenants.Where(p => !p.IsDeleted);
                if (clientId > 0)
                    query = query.Where(u => u.ClientId == clientId);
                // Search - go deeper
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(p => (p.FirstName != null && p.FirstName.ToLower().Contains(search))
                                        || (p.LastName != null && p.LastName.ToLower().Contains(search))
                                        || (p.ClaimNumber != null && p.ClaimNumber.ToLower().Contains(search))
                                        || (p.NiNumber != null && p.NiNumber.ToLower().Contains(search))
                                        || (p.Email != null && p.Email.ToLower().Contains(search))
                                        || (p.Mobile != null && p.Mobile.ToLower().Contains(search))
                                        || (p.Address1 != null && p.Address1.ToLower().Contains(search))
                                        || (p.Address2 != null && p.Address2.ToLower().Contains(search))
                                        || (p.Address3 != null && p.Address3.ToLower().Contains(search))
                                        || (p.PostCode != null && p.PostCode.ToLower().Contains(search))
                                        || (p.City != null && p.City.ToLower().Contains(search))
                                        || (p.County != null && p.County.ToLower().Contains(search))
                                        );
                }

                // Sorting
                if (sortField != TenantSortFields.None && sortDirection != SortDirection.None)
                {
                    if (sortDirection == SortDirection.Asc)
                        query = query.OrderBy(sortField.GetColumn());
                    else if (sortDirection == SortDirection.Desc)
                        query = query.OrderByDescending(sortField.GetColumn());
                }

                // Pagination
                paginatedTenants.TotalCount = query.LongCount();
                paginatedTenants.PageSize = limit;
                paginatedTenants.CurrentPage = page;
                query = query.Skip((page - 1) * limit).Take(limit);

                paginatedTenants.Items = query.ToList();

                return paginatedTenants;
            }
        }

        public TblTenant GetTenantById(int tenantId)
        {
            return tenantGenericRepository.GetById(tenantId);
        }

        public TblTenant Add(TblTenant addTenant)
        {
            return tenantGenericRepository.Insert(addTenant);
        }

        public TblTenant Update(TblTenant updateTenant)
        {
            return tenantGenericRepository.Update(updateTenant);
        }

        public bool Delete(int tenantId)
        {
            return tenantGenericRepository.DeleteById(tenantId);
        }
    }
}
