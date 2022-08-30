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
    public class DocumentRepository : BaseCustomRepository
    {
        private GenericRepository<TblDocument> documentGenericRepository;

        public DocumentRepository(string databaseProvider, string connectionString) : base(databaseProvider, connectionString)
        {
            documentGenericRepository = new GenericRepository<TblDocument>(databaseProvider, connectionString);
        }

        public PaginatedList<TblDocument> GetActiveDocumentList(int proId,string search, int limit = 10, int page = 1, string sortBy = "", string sortDir = "")
        {
            PaginatedList<TblDocument> paginatedDocuments = new PaginatedList<TblDocument>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                DocumentSortFields sortField = sortBy.GetDocumentField();
                SortDirection sortDirection = sortDir.GetSortDirectionByName();

                //Select Query
                var query = dataContext.TblDocuments.Where(p => !p.IsDeleted && p.ParentId == proId);

                // Search - go deeper
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(p => p.Name != null && p.Name.ToLower().Contains(search)
                                        );
                }

                // Sorting
                if (sortField != DocumentSortFields.None && sortDirection != SortDirection.None)
                {
                    if (sortDirection == SortDirection.Asc)
                        query = query.OrderBy(sortField.GetColumn());
                    else if (sortDirection == SortDirection.Desc)
                        query = query.OrderByDescending(sortField.GetColumn());
                }

                // Pagination
                paginatedDocuments.TotalCount = query.LongCount();
                paginatedDocuments.PageSize = limit;
                paginatedDocuments.CurrentPage = page;
                query = query.Skip((page - 1) * limit).Take(limit);

                paginatedDocuments.Items = query.ToList();

                return paginatedDocuments;
            }
        }

        public TblDocument GetDocumentById(int DocumentId)
        {
            return documentGenericRepository.GetById(DocumentId);
        }

        public TblDocument Add(TblDocument addDocument)
        {
            return documentGenericRepository.Insert(addDocument);
        }

        public TblDocument Update(TblDocument updateDocument)
        {
            return documentGenericRepository.Update(updateDocument);
        }

        public bool Delete(int DocumentId)
        {
            return documentGenericRepository.DeleteById(DocumentId);
        }
    }
}
