using ds.pms.apicommon.Models;
using ds.pms.dal.CustomModels;
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
    public class ConfigRepository : BaseCustomRepository
    {
        private GenericRepository<TblConfig> configGenericRepository;

        public ConfigRepository(string databaseProvider, string connectionString) : base(databaseProvider, connectionString)
        {
            configGenericRepository = new GenericRepository<TblConfig>(databaseProvider, connectionString);
        }

        public PaginatedList<ConfigList> GetActiveConfigList(int clientId,string search, int limit = 10, int page = 1, string sortBy = "", string sortDir = "")
        {
            PaginatedList<ConfigList> paginatedConfigs = new PaginatedList<ConfigList>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                ConfigSortFields sortField = sortBy.GetConfigField();
                SortDirection sortDirection = sortDir.GetSortDirectionByName();

                //Select Query
              //  var query = dataContext.TblConfigs.Where(p => !p.IsDeleted);
                var query = (from config in dataContext.TblConfigs
                             from client in dataContext.TblClients.LeftJoin(c => c.Id == config.ClientId && c.IsActive && !c.IsDeleted)
                             where !config.IsDeleted
                             select new ConfigList
                             {
                                Client = client.Name,
                                ClientId= config.ClientId,
                                Id= config.Id,
                                Value= config.Value,
                                Key= config.Key,
                                Description= config.Description

                             });

                if (clientId > 0)
                    query = query.Where(u => u.ClientId == clientId);
                // Search - go deeper
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(p => p.Key != null && p.Key.ToLower().Contains(search)
                                        || (p.Value != null && p.Value.ToLower().Contains(search))
                                        || (p.Description != null && p.Description.ToLower().Contains(search))
                                        );
                }

                // Sorting
                //if (sortField != ConfigSortFields.None && sortDirection != SortDirection.None)
                //{
                //    if (sortDirection == SortDirection.Asc)
                //        query = query.OrderBy(sortField.GetColumn());
                //    else if (sortDirection == SortDirection.Desc)
                //        query = query.OrderByDescending(sortField.GetColumn());
                //}

                // Pagination
                paginatedConfigs.TotalCount = query.LongCount();
                paginatedConfigs.PageSize = limit;
                paginatedConfigs.CurrentPage = page;
                query = query.Skip((page - 1) * limit).Take(limit);

                paginatedConfigs.Items = query.ToList();

                return paginatedConfigs;
            }
        }

        public TblConfig GetConfigById(int configId)
        {
            return configGenericRepository.GetById(configId);
        }
        public PaginatedList<ConfigList> GetAdminConfigs()
        {
            PaginatedList<ConfigList> paginatedConfigs = new PaginatedList<ConfigList>();

            using (dataContext = new PmsDB(providerName, connectionString))
            {
                var query = (from config in dataContext.TblConfigs
                             where !config.IsDeleted && config.ClientId==0
                             select new ConfigList
                             {
                                 ClientId = config.ClientId,
                                 Id = config.Id,
                                 Value = config.Value,
                                 Key = config.Key,
                                 Description = config.Description

                             });

                paginatedConfigs.Items = query.ToList();
            }
            return paginatedConfigs;
        }
        public TblConfig Add(TblConfig addConfig)
        {
            return configGenericRepository.Insert(addConfig);
        }

        public TblConfig Update(TblConfig updateConfig)
        {
            return configGenericRepository.Update(updateConfig);
        }

        public bool Delete(int configId)
        {
            return configGenericRepository.DeleteById(configId);
        }
    }
}
