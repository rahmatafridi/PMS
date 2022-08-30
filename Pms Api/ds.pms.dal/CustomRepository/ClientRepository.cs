using ds.pms.apicommon.Models;
using ds.pms.dal.GenericRepository;
using ds.pms.dal.Models;
using ds.pms.dal.SortFields;
using System.Collections.Generic;
using System.Linq;

namespace ds.pms.dal.CustomRepository
{
    public class ClientRepository : BaseCustomRepository
    {
        private GenericRepository<TblClient> clientGenericRepository;

        public ClientRepository(string databaseProvider, string connectionString) : base(databaseProvider, connectionString)
        {
            clientGenericRepository = new GenericRepository<TblClient>(databaseProvider, connectionString);
        }

        public PaginatedList<TblClient> GetActiveClientList(string search, int limit = 10, int page = 1, string sortBy = "", string sortDir = "")
        {
            PaginatedList<TblClient> paginatedClients = new PaginatedList<TblClient>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                ClientSortFields sortField = sortBy.GetClientField();
                SortDirection sortDirection = sortDir.GetSortDirectionByName();

                //Select Query
                var query = dataContext.TblClients.Where(c => c.IsActive && !c.IsDeleted);

                // Search - go deeper
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(c => (c.Name != null && c.Name.ToLower().Contains(search))
                                        || (c.Email != null && c.Email.ToLower().Contains(search))
                                        || (c.Mobile != null && c.Mobile.ToLower().Contains(search))
                                        || (c.Address1 != null && c.Address1.ToLower().Contains(search))
                                        || (c.Address2 != null && c.Address2.ToLower().Contains(search))
                                        || (c.Address3 != null && c.Address3.ToLower().Contains(search))
                                        || (c.PostCode != null && c.PostCode.ToLower().Contains(search))
                                        || (c.City != null && c.City.ToLower().Contains(search))
                                        || (c.County != null && c.County.ToLower().Contains(search))
                                        );
                }

                // Sorting
                if (sortField != ClientSortFields.None && sortDirection != SortDirection.None)
                {
                    if (sortDirection == SortDirection.Asc)
                        query = query.OrderBy(sortField.GetColumn());
                    else if (sortDirection == SortDirection.Desc)
                        query = query.OrderByDescending(sortField.GetColumn());
                }

                // Pagination
                paginatedClients.TotalCount = query.LongCount();
                paginatedClients.PageSize = limit;
                paginatedClients.CurrentPage = page;
                if (limit != -1)
                    query = query.Skip((page - 1) * limit).Take(limit);

                paginatedClients.Items = query.ToList();

                return paginatedClients;
            }
        }

        public TblClient GetClientById(int clientId)
        {
            return clientGenericRepository.GetById(clientId);
        }

        public TblClient Add(TblClient addClient)
        {
            return clientGenericRepository.Insert(addClient);
        }

        public TblClient Update(TblClient updateClient)
        {
            return clientGenericRepository.Update(updateClient);
        }

        public bool DoesEmailExists(string email)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                return dataContext.TblClients.Any(c => c.Email.ToLower() == email.ToLower() && !c.IsDeleted);
            }
        }

        public bool DoesAnyOtherUserUseThisEmail(long? clientId, string email)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                if (clientId != null && clientId.HasValue && clientId.Value > 0)
                    return dataContext.TblClients.Any(c => c.Id != clientId.Value && c.Email.ToLower() == email.ToLower() && !c.IsDeleted);
                else
                    return dataContext.TblClients.Any(c => c.Email.ToLower() == email.ToLower() && !c.IsDeleted);
            }
        }

        public bool Delete(int clientId)
        {
            return clientGenericRepository.DeleteById(clientId);
        }

        public List<TblRole> CopyRoleToNewlyAddedClient()
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
               return dataContext.TblRoles.Where(r => r.ClientId == 0 && r.IsTemplate && r.IsActive).ToList();
            }
        }
    }
}
