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
    public class CompliancePropertyDocRepository : BaseCustomRepository
    {
        private GenericRepository<TblCompliancePropertyDoc> compliancePropertyDocGenericRepository;
        private GenericRepository<TblComplianceProperty> compliancePropertyGenericRepository;

        public CompliancePropertyDocRepository(string databaseProvider, string connectionString) : base(databaseProvider, connectionString)
        {
            compliancePropertyDocGenericRepository = new GenericRepository<TblCompliancePropertyDoc>(databaseProvider, connectionString);
            compliancePropertyGenericRepository = new GenericRepository<TblComplianceProperty>(databaseProvider, connectionString);
        }

        public PaginatedList<PropertyCompianceDocsList> GetActiveCompliancePropertyDocList(int proId,string search, int limit = 10, int page = 1, string sortBy = "", string sortDir = "")
        {
            PaginatedList<PropertyCompianceDocsList> paginatedCompliancePropertyDocs = new PaginatedList<PropertyCompianceDocsList>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                CompliancePropertyDocSortFields sortField = sortBy.GetCompliancePropertyDocField();
                SortDirection sortDirection = sortDir.GetSortDirectionByName();

                //Select Query
                //var query = dataContext.TblCompliancePropertyDocs.Where(p => !p.IsDeleted);

                var query = (from pd in dataContext.TblCompliancePropertyDocs join 
                             
                              cd in dataContext.TblCompliances  on pd.ComplianceId equals cd.Id
                             where pd.PropertyId== proId
                                 //where provider.IsActive && !provider.IsDeleted
                             select new PropertyCompianceDocsList
                             {
                                 Id = pd.Id,
                                 Name = pd.Name,
                                 Title = cd.Title,
                                 ValidFromDate = pd.ExpiryDateFrom,
                                 ValidToDate = pd.ExpiryDateTo,
                                 UpdatedBy = pd.AddedBy,
                                 UploadedDate = pd.AddedDate,
                             });

                // Search - go deeper
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(p => p.Name != null && p.Name.ToLower().Contains(search)
                                        );
                }

                // Sorting
                //if (sortField != CompliancePropertyDocSortFields.None && sortDirection != SortDirection.None)
                //{
                //    if (sortDirection == SortDirection.Asc)
                //        query = query.OrderBy(sortField.GetColumn());
                //    else if (sortDirection == SortDirection.Desc)
                //        query = query.OrderByDescending(sortField.GetColumn());
                //}

                // Pagination
                paginatedCompliancePropertyDocs.TotalCount = query.LongCount();
                paginatedCompliancePropertyDocs.PageSize = limit;
                paginatedCompliancePropertyDocs.CurrentPage = page;
                query = query.Skip((page - 1) * limit).Take(limit);

                paginatedCompliancePropertyDocs.Items = query.ToList();

                return paginatedCompliancePropertyDocs;
            }
        }

        public TblCompliancePropertyDoc GetCompliancePropertyDocById(int compliancePropertyDocId)
        {
            return compliancePropertyDocGenericRepository.GetById(compliancePropertyDocId);
        }

        public TblCompliancePropertyDoc Add(TblCompliancePropertyDoc addCompliancePropertyDoc)
        {
            return compliancePropertyDocGenericRepository.Insert(addCompliancePropertyDoc);
        }
        public TblComplianceProperty AddPro(TblComplianceProperty addComplianceProperty)
        {
            return compliancePropertyGenericRepository.Insert(addComplianceProperty);
        }
        public TblCompliancePropertyDoc Update(TblCompliancePropertyDoc updateCompliancePropertyDoc)
        {
            return compliancePropertyDocGenericRepository.Update(updateCompliancePropertyDoc);
        }

        public bool Delete(int compliancePropertyDocId)
        {
            return compliancePropertyDocGenericRepository.DeleteById(compliancePropertyDocId);
        }
    }
}
