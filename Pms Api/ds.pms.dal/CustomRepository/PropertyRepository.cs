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
    public class PropertyRepository : BaseCustomRepository
    {
        private GenericRepository<TblProperty> propertyGenericRepository;
        private GenericRepository<TblComplianceProperty> compliancePropertyRepository;
        private GenericRepository<TblRoom> roomGenericRepository;
        public PropertyRepository(string databaseProvider, string connectionString) : base(databaseProvider, connectionString)
        {
            propertyGenericRepository = new GenericRepository<TblProperty>(databaseProvider, connectionString);
            compliancePropertyRepository = new GenericRepository<TblComplianceProperty>(databaseProvider, connectionString);
            roomGenericRepository = new GenericRepository<TblRoom>(databaseProvider, connectionString);

        }

        public PaginatedList<PropertyList> GetActivePropertyList(int clientId, int providerId, string search, int limit = 10, int page = 1, string sortBy = "", string sortDir = "")
        {
            PaginatedList<PropertyList> paginatedProperties = new PaginatedList<PropertyList>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                PropertySortFields sortField = sortBy.GetPropertyField();
                SortDirection sortDirection = sortDir.GetSortDirectionByName();

                //Select Query
                //var query = dataContext.TblProperties.Where(p => !p.IsDeleted);

                var query = (from property in dataContext.TblProperties
                             from client in dataContext.TblClients.LeftJoin(c => c.Id == property.ClientId && c.IsActive && !c.IsDeleted)
                             from provier in dataContext.TblProviders.LeftJoin(c => c.Id == property.ProviderId && c.IsActive && !c.IsDeleted)
                             where !property.IsDeleted
                             select new PropertyList
                             {
                                 Id = property.Id,
                                 ClientId = property.ClientId,
                                 ProviderId = property.ProviderId,
                                 AreaId = property.AreaId,
                                 AgentId = property.AgentId,
                                 Address1 = property.Address1,
                                 Address2 = property.Address2,
                                 Address3 = property.Address3,
                                 PostCode = property.PostCode,
                                 City = property.City,
                                 County = property.County,
                                 Country = property.Country,
                                 FireExitBlanket = property.FireExitBlanket,
                                 NumberOfRooms = property.NumberOfRooms,
                                 TitleNo = property.TitleNo,
                                 LastRegNumber = property.LastRegNumber,
                                 LocalAuth = property.LocalAuth,
                                 DateSlaStart = property.DateSlaStart,
                                 DateSlaEnd = property.DateSlaEnd,
                                 DateLeaseStart = property.DateLeaseStart,
                                 DateLeaseEnd = property.DateLeaseEnd,
                                 DatePreAcceptInsp = property.DatePreAcceptInsp,
                                 DateInspection = property.DateInspection,
                                 DateExempt = property.DateExempt,
                                 DateQuarterlyAudit = property.DateQuarterlyAudit,
                                 DateQuarterlyInsp = property.DateQuarterlyInsp,
                                 IsPublished = property.IsPublished,
                                 ClientName = client.Name ?? string.Empty,
                                 ProviderName = provier.Name ?? string.Empty,
                             });

                if (clientId > 0)
                {
                    query = query.Where(p => p.ClientId == clientId);
                }

                if (providerId > 0)
                {
                    query = query.Where(p => p.ProviderId == providerId);
                }

                // Search - go deeper
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(p => (p.Address1 != null && p.Address1.ToLower().Contains(search))
                                        || (p.Address2 != null && p.Address2.ToLower().Contains(search))
                                        || (p.Address3 != null && p.Address3.ToLower().Contains(search))
                                        || (p.PostCode != null && p.PostCode.ToLower().Contains(search))
                                        || (p.City != null && p.City.ToLower().Contains(search))
                                        || (p.County != null && p.County.ToLower().Contains(search))
                                        || (p.TitleNo != null && p.TitleNo.ToLower().Contains(search))
                                        || (p.NumberOfRooms != null && Convert.ToString(p.NumberOfRooms).ToLower().Contains(search))
                                        || (p.ClientName != null && p.ClientName.ToLower().Contains(search))
                                        || (p.ProviderName != null && p.ProviderName.ToLower().Contains(search))
                                        );
                }

                // Sorting
                if (sortField != PropertySortFields.None && sortDirection != SortDirection.None)
                {
                    if (sortDirection == SortDirection.Asc)
                        query = query.OrderBy(sortField.GetColumn());
                    else if (sortDirection == SortDirection.Desc)
                        query = query.OrderByDescending(sortField.GetColumn());
                }

                // Pagination
                paginatedProperties.TotalCount = query.LongCount();
                paginatedProperties.PageSize = limit;
                paginatedProperties.CurrentPage = page;
                query = query.Skip((page - 1) * limit).Take(limit);

                paginatedProperties.Items = query.ToList();

                return paginatedProperties;
            }
        }

        public TblProperty GetPropertyById(int propertyId)
        {
            return propertyGenericRepository.GetById(propertyId);
        }

        public TblProperty Add(TblProperty addProperty)
        {
            return propertyGenericRepository.Insert(addProperty);
        }
        public TblRoom AddRoom(TblRoom addRoom)
        {
            return roomGenericRepository.Insert(addRoom);
        }
        public TblProperty Update(TblProperty updateProperty)
        {
            return propertyGenericRepository.Update(updateProperty);
        }

        public bool Delete(int propertyId)
        {
            return propertyGenericRepository.DeleteById(propertyId);
        }

        public TblComplianceProperty AddComplianceProperty(TblComplianceProperty tblComplianceProperty)
        {
            return compliancePropertyRepository.Insert(tblComplianceProperty);
        }
    }
}
