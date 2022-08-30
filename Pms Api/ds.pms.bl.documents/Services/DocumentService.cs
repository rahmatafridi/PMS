using ds.pms.apicommon.Models;
using ds.pms.bl.documents.Converters;
using ds.pms.bl.documents.IServices;
using ds.pms.bl.documents.Models;
using ds.pms.bl.logger;
using ds.pms.dal.CustomRepository;
using ds.pms.dal.Models;
using ds.pms.helpers.FileOperations;
using ds.pms.helpers.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ds.pms.bl.documents.Services
{
    public class DocumentService : IDocumentService
    {
        private DocumentRepository documentRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public DocumentService(string _databaseProvider, string _connectionString, ILogger<DocumentService> documentServiceLogger)
        {
            documentRepository = new DocumentRepository(_databaseProvider, _connectionString);
            logging = new Logging(documentServiceLogger);
            _className = this.GetType().Name;
        }

        public DocumentService(string _databaseProvider, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds
            , ILogger<DocumentService> documentServiceLogger)
        {
            documentRepository = new DocumentRepository(_databaseProvider, _connectionString);
            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(documentServiceLogger);
            _className = this.GetType().Name;
        }

        public PaginatedList<Document> GetActiveDocumentList(int proId,string search, int? limit = 10, int? page = 1, string sort = "")
        {
            PaginatedList<Document> documentsPaginatedList = new PaginatedList<Document>();
            try
            {
                int pageSize = limit ?? 10;
                int pageNumber = page ?? 1;
                string sortBy = string.Empty, sortDirection = string.Empty;
                if (!string.IsNullOrEmpty(sort))
                {
                    string[] sorting = SortDirection.SortDir(sort);
                    if (sorting.Length > 1)
                    {
                        sortBy = sorting[0];
                        sortDirection = sorting[1];
                    }
                }
                documentsPaginatedList = Pagination.ConvertDalToBl(documentRepository.GetActiveDocumentList(proId,search, pageSize, pageNumber, sortBy, sortDirection));
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return documentsPaginatedList;
        }

        public Document GetDocumentById(int documentId)
        {
            try
            {
                if (documentId > 0)
                {
                    Document document = documentRepository.GetDocumentById(documentId);
                    if (document != null)
                        return document;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        public DocumentCommonResponse AddDocument(Document addDocument, string userName)
        {
            DocumentCommonResponse documentCommonResponse = new DocumentCommonResponse();
            try
            {
                documentCommonResponse.Success = false;
                if (addDocument != null)
                {
                    //byte[] bytes = Encoding.ASCII.GetBytes(addDocument.DocObject);

                    TblDocument tblDocument = addDocument;
                    tblDocument.Name = addDocument.Name;
                    tblDocument.Extension = addDocument.Extension;
                    tblDocument.MimeType = addDocument.MimeType;
                    tblDocument.Size = addDocument.Size;
                    tblDocument.DocObject = addDocument.DocObject;
                    tblDocument.AddedBy = userName;
                    tblDocument.AddedDate = DateTime.UtcNow;
                    tblDocument.ClinetId = addDocument.ClinetId;
                    tblDocument.ParentId = addDocument.ParentId;
                    tblDocument.TypeId = addDocument.TypeId;
                    documentCommonResponse.Document = documentRepository.Add(tblDocument);
                    if (documentCommonResponse.Document != null && documentCommonResponse.Document.Id > 0)
                    {
                        documentCommonResponse.Success = true;
                        return documentCommonResponse;
                    }
                    else
                        documentCommonResponse.Message = "Unable to add document.";
                }
                else
                    documentCommonResponse.Message = "Supplied document information is not valid.";
            }
            catch (Exception ex)
            {
                documentCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return documentCommonResponse;
        }

        public DocumentCommonResponse UpdateDocument(Document updateDocument, string userName)
        {
            DocumentCommonResponse documentCommonResponse = new DocumentCommonResponse();
            try
            {
                documentCommonResponse.Success = false;
                if (updateDocument != null && updateDocument.Id > 0)
                {
                    TblDocument tblDocument;
                    tblDocument = documentRepository.GetDocumentById(updateDocument.Id);
                    if (tblDocument != null)
                    {
                        tblDocument.CategoryId = updateDocument.CategoryId;
                        //tblDocument.Name = updateDocument.Name;
                        //tblDocument.Extension = updateDocument.Extension;
                        //tblDocument.MimeType = updateDocument.MimeType;
                        //tblDocument.DocObject = updateDocument.DocObject;
                        //tblDocument.Size = updateDocument.Size;
                        tblDocument.LastModifiedDate = DateTime.UtcNow;
                        tblDocument.ModifiedBy = userName;
                        tblDocument.ModifiedDate = DateTime.UtcNow;
                        documentCommonResponse.Document = documentRepository.Update(tblDocument);
                        if (documentCommonResponse.Document != null && documentCommonResponse.Document.Id > 0)
                        {
                            documentCommonResponse.Success = true;
                            return documentCommonResponse;
                        }
                        else
                            documentCommonResponse.Message = "Unable to update document.";
                    }
                    else
                        documentCommonResponse.Message = "Document does not exists.";
                }
                else
                    documentCommonResponse.Message = "Supplied document information is not valid.";
            }
            catch (Exception ex)
            {
                documentCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return documentCommonResponse;
        }

        public DocumentCommonResponse SoftDelete(int documentId, string userName)
        {
            DocumentCommonResponse documentCommonResponse = new DocumentCommonResponse();
            try
            {
                documentCommonResponse.Success = false;
                if (documentId > 0)
                {
                    TblDocument tblDocument = documentRepository.GetDocumentById(documentId);
                    if (tblDocument != null && tblDocument.Id > 0)
                    {
                        tblDocument.ModifiedBy = userName;
                        tblDocument.ModifiedDate = DateTime.UtcNow;
                        tblDocument.IsDeleted = true;
                        tblDocument.DeletedBy = userName;
                        tblDocument.DeletedDate = DateTime.Now;
                        documentCommonResponse.Document = documentRepository.Update(tblDocument);
                        if (documentCommonResponse.Document != null && documentCommonResponse.Document.Id > 0)
                        {
                            documentCommonResponse.Success = true;
                            return documentCommonResponse;
                        }
                        else
                            documentCommonResponse.Message = "Unable to delete document.";
                    }
                    else
                        documentCommonResponse.Message = "Document does not exists.";
                }
                else
                    documentCommonResponse.Message = "Supplied document information is not valid.";
            }
            catch (Exception ex)
            {
                documentCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {

            }
            return documentCommonResponse;
        }

        public bool HardDelete(int documentId)
        {
            try
            {
                if (documentId > 0)
                {
                    return documentRepository.Delete(documentId);
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return false;
        }
    }
}
