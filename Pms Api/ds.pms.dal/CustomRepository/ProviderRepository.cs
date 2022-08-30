using ds.pms.apicommon.Models;
using ds.pms.dal.CustomModels;
using ds.pms.dal.GenericRepository;
using ds.pms.dal.Models;
using ds.pms.dal.SortFields;
using LinqToDB;
using System.Linq;

namespace ds.pms.dal.CustomRepository
{
    public class ProviderRepository : BaseCustomRepository
    {
        private GenericRepository<TblProvider> providerGenericRepository;
        private GenericRepository<TblProviderUser> userProviderGenericRepository;

        public ProviderRepository(string databaseProvider, string connectionString) : base(databaseProvider, connectionString)
        {
            providerGenericRepository = new GenericRepository<TblProvider>(databaseProvider, connectionString);
            userProviderGenericRepository = new GenericRepository<TblProviderUser>(databaseProvider, connectionString);

        }

        public PaginatedList<ProviderList> GetActiveProviderList(int clientId, string search, int limit = 10, int page = 1, string sortBy = "", string sortDir = "")
        {
            PaginatedList<ProviderList> paginatedProviders = new PaginatedList<ProviderList>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                ProviderSortFields sortField = sortBy.GetProviderField();
                SortDirection sortDirection = sortDir.GetSortDirectionByName();

                //Select Query
                //var query = dataContext.TblProviders.Where(p => p.IsActive && !p.IsDeleted);

                var query = (from provider in dataContext.TblProviders
                             from client in dataContext.TblClients.RightJoin(c => c.Id == provider.ClientId && c.IsActive && !c.IsDeleted)
                             where  !provider.IsDeleted
                             select new ProviderList
                             {
                                 Id = provider.Id,
                                 ClientId = provider.ClientId,
                                 Name = provider.Name,
                                 Email = provider.Email,
                                 Mobile = provider.Mobile,
                                 Address1 = provider.Address1,
                                 Address2 = provider.Address2,
                                 Address3 = provider.Address3,
                                 PostCode = provider.PostCode,
                                 City = provider.City,
                                 County = provider.County,
                                 ClientName = client.Name ?? string.Empty,
                             }); ;

                if (clientId > 0)
                {
                    query = query.Where(p => p.ClientId == clientId);
                }

                // Search - go deeper
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(p => (p.Name != null && p.Name.ToLower().Contains(search))
                                        || (p.Email != null && p.Email.ToLower().Contains(search))
                                        || (p.Mobile != null && p.Mobile.ToLower().Contains(search))
                                        || (p.Address1 != null && p.Address1.ToLower().Contains(search))
                                        || (p.Address2 != null && p.Address2.ToLower().Contains(search))
                                        || (p.Address3 != null && p.Address3.ToLower().Contains(search))
                                        || (p.PostCode != null && p.PostCode.ToLower().Contains(search))
                                        || (p.City != null && p.City.ToLower().Contains(search))
                                        || (p.County != null && p.County.ToLower().Contains(search))
                                        || (p.ClientName != null && p.ClientName.ToLower().Contains(search))
                                        );
                }

                // Sorting
                if (sortField != ProviderSortFields.None && sortDirection != SortDirection.None)
                {
                    if (sortDirection == SortDirection.Asc)
                        query = query.OrderBy(sortField.GetColumn());
                    else if (sortDirection == SortDirection.Desc)
                        query = query.OrderByDescending(sortField.GetColumn());
                }

                // Pagination
                paginatedProviders.TotalCount = query.LongCount();
                paginatedProviders.PageSize = limit;
                paginatedProviders.CurrentPage = page;
                if (limit != -1)
                    query = query.Skip((page - 1) * limit).Take(limit);

                paginatedProviders.Items = query.ToList();

                return paginatedProviders;
            }
        }

        public TblProvider GetProviderById(int providerId)
        {
            return providerGenericRepository.GetById(providerId);
        }
        public TblProviderUser GetProiderUserById(int providerId)
        {
            var tblProviderUser = new TblProviderUser();
            using (dataContext = new PmsDB(providerName, connectionString))
            {

                //Select Query
                //var query = dataContext.TblProviders.Where(p => p.IsActive && !p.IsDeleted);
                var data = dataContext.TblProviderUsers.FirstOrDefault(x => x.Id == providerId);
                tblProviderUser.Id = data.Id;
                tblProviderUser.UserId = data.UserId;
                tblProviderUser.ProviderId = data.ProviderId;



                return tblProviderUser;
            }

        }

        public TblProviderUser GetProiderUserDetailById(int providerId)
        {
            var tblProviderUser = new TblProviderUser();
            using (dataContext = new PmsDB(providerName, connectionString))
            {

                //Select Query
                //var query = dataContext.TblProviders.Where(p => p.IsActive && !p.IsDeleted);
                var data = dataContext.TblProviderUsers.FirstOrDefault(x => x.ProviderId == providerId);
                tblProviderUser.Id = data.Id;
                tblProviderUser.UserId = data.UserId;
                tblProviderUser.ProviderId = data.ProviderId;
                
              

                return tblProviderUser;
            }

        }
        public TblProvider Add(TblProvider addProvider)
        {
            return providerGenericRepository.Insert(addProvider);
        }
        public TblProviderUser AddUser(TblProviderUser addProvider)
        {
            return userProviderGenericRepository.Insert(addProvider);
        }
        public TblProvider Update(TblProvider updateProvider)
        {
            return providerGenericRepository.Update(updateProvider);
        }
        public TblProviderUser UpdateUser(TblProviderUser updateProvider)
        {
            return userProviderGenericRepository.Update(updateProvider);
        }
        public bool DoesEmailExists(string email)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                return dataContext.TblProviders.Any(c => c.Email.ToLower() == email.ToLower() && !c.IsDeleted);
            }
        }

        public bool DoesAnyOtherUserUseThisEmail(long? providerId, string email)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                if (providerId != null && providerId.HasValue && providerId.Value > 0)
                    return dataContext.TblProviders.Any(c => c.Id != providerId.Value && c.Email.ToLower() == email.ToLower() && !c.IsDeleted);
                else
                    return dataContext.TblProviders.Any(c => c.Email.ToLower() == email.ToLower() && !c.IsDeleted);
            }
        }

        public bool Delete(int ProviderId)
        {
            return providerGenericRepository.DeleteById(ProviderId);
        }
    }
}
