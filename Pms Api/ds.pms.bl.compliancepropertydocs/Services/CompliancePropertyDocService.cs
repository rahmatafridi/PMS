using ds.pms.apicommon.Models;
using ds.pms.bl.compliancepropertydocs.Converters;
using ds.pms.bl.compliancepropertydocs.IServices;
using ds.pms.bl.compliancepropertydocs.Models;
using ds.pms.bl.logger;
using ds.pms.dal.CustomModels;
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

namespace ds.pms.bl.compliancepropertydocs.Services
{
    public class CompliancePropertyDocService : ICompliancePropertyDocService
    {
        private CompliancePropertyDocRepository compliancePropertyDocRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public CompliancePropertyDocService(string _databaseProvider, string _connectionString, ILogger<CompliancePropertyDocService> compliancePropertyDocServiceLogger)
        {
            compliancePropertyDocRepository = new CompliancePropertyDocRepository(_databaseProvider, _connectionString);
            logging = new Logging(compliancePropertyDocServiceLogger);
            _className = this.GetType().Name;
        }

        public CompliancePropertyDocService(string _databaseProvider, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds
            , ILogger<CompliancePropertyDocService> compliancePropertyDocServiceLogger)
        {
            compliancePropertyDocRepository = new CompliancePropertyDocRepository(_databaseProvider, _connectionString);
            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(compliancePropertyDocServiceLogger);
            _className = this.GetType().Name;
        }

        public PaginatedList<PropertyCompianceDocsList> GetActiveCompliancePropertyDocList(int proId,string search, int? limit = 10, int? page = 1, string sort = "")
        {
            PaginatedList<PropertyCompianceDocsList> compliancePropertyDocsPaginatedList = new PaginatedList<PropertyCompianceDocsList>();
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
                compliancePropertyDocsPaginatedList = Pagination.ConvertDalToBl(compliancePropertyDocRepository.GetActiveCompliancePropertyDocList(proId,search, pageSize, pageNumber, sortBy, sortDirection));
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return compliancePropertyDocsPaginatedList;
        }

        public CompliancePropertyDoc GetCompliancePropertyDocById(int compliancePropertyDocId)
        {
            try
            {
                if (compliancePropertyDocId > 0)
                {
                    CompliancePropertyDoc compliancePropertyDoc = compliancePropertyDocRepository.GetCompliancePropertyDocById(compliancePropertyDocId);
                    if (compliancePropertyDoc != null)
                        return compliancePropertyDoc;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        public CompliancePropertyDocCommonResponse AddCompliancePropertyDoc(CompliancePropertyDoc addCompliancePropertyDoc, string userName)
        {
            CompliancePropertyDocCommonResponse compliancePropertyDocCommonResponse = new CompliancePropertyDocCommonResponse();
            try
            {
                compliancePropertyDocCommonResponse.Success = false;
                if (addCompliancePropertyDoc != null)
                {
                    TblCompliancePropertyDoc tblCompliancePropertyDoc = addCompliancePropertyDoc;
                    tblCompliancePropertyDoc.Name = addCompliancePropertyDoc.Name;
                    tblCompliancePropertyDoc.Extension = addCompliancePropertyDoc.Extension;
                    tblCompliancePropertyDoc.MimeType = addCompliancePropertyDoc.MimeType;
                    tblCompliancePropertyDoc.Size = addCompliancePropertyDoc.Size;
                    tblCompliancePropertyDoc.DocObject = addCompliancePropertyDoc.DocObject;
                    tblCompliancePropertyDoc.PropertyId = addCompliancePropertyDoc.PropertyId;
                    tblCompliancePropertyDoc.AddedBy = userName;
                    tblCompliancePropertyDoc.AddedDate = DateTime.UtcNow;
                    compliancePropertyDocCommonResponse.CompliancePropertyDoc = compliancePropertyDocRepository.Add(tblCompliancePropertyDoc);
                    if (compliancePropertyDocCommonResponse.CompliancePropertyDoc != null && compliancePropertyDocCommonResponse.CompliancePropertyDoc.Id > 0)
                    {
                        TblComplianceProperty pro = new TblComplianceProperty();
                        pro.AddedDate = DateTime.Now;
                        pro.ComplianceId = compliancePropertyDocCommonResponse.CompliancePropertyDoc.ComplianceId;
                        pro.PropertyId = compliancePropertyDocCommonResponse.CompliancePropertyDoc.PropertyId;
                        var data = compliancePropertyDocRepository.AddPro(pro);
                        if (data != null)
                        {
                            compliancePropertyDocCommonResponse.Success = true;
                            return compliancePropertyDocCommonResponse;
                        }
                    }
                    else
                        compliancePropertyDocCommonResponse.Message = "Unable to add compliancePropertyDoc.";
                }
                else
                    compliancePropertyDocCommonResponse.Message = "Supplied compliancePropertyDoc information is not valid.";
            }
            catch (Exception ex)
            {
                compliancePropertyDocCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return compliancePropertyDocCommonResponse;
        }

        public CompliancePropertyDocCommonResponse UpdateCompliancePropertyDoc(UpdateCompliancePropertyDoc updateCompliancePropertyDoc, string userName)
        {
            CompliancePropertyDocCommonResponse compliancePropertyDocCommonResponse = new CompliancePropertyDocCommonResponse();
            try
            {
                compliancePropertyDocCommonResponse.Success = false;
                if (updateCompliancePropertyDoc != null && updateCompliancePropertyDoc.Id > 0)
                {
                    TblCompliancePropertyDoc tblCompliancePropertyDoc;
                    tblCompliancePropertyDoc = compliancePropertyDocRepository.GetCompliancePropertyDocById(updateCompliancePropertyDoc.Id);
                    if (tblCompliancePropertyDoc != null)
                    {
                        tblCompliancePropertyDoc.ComplianceId = updateCompliancePropertyDoc.ComplianceId;
                        tblCompliancePropertyDoc.CompliancePropertyId = updateCompliancePropertyDoc.CompliancePropertyId;
                        tblCompliancePropertyDoc.PropertyId = updateCompliancePropertyDoc.PropertyId;
                        tblCompliancePropertyDoc.ExpiryDateFrom = updateCompliancePropertyDoc.ExpiryDateFrom;
                        tblCompliancePropertyDoc.ExpiryDateTo = updateCompliancePropertyDoc.ExpiryDateTo;
                        //tblCompliancePropertyDoc.Name = updateCompliancePropertyDoc.Name;
                        //tblCompliancePropertyDoc.Extension = updateCompliancePropertyDoc.Extension;
                        //tblCompliancePropertyDoc.MimeType = updateCompliancePropertyDoc.MimeType;
                        //tblCompliancePropertyDoc.DocObject = updateCompliancePropertyDoc.DocObject;
                        //tblCompliancePropertyDoc.Size = updateCompliancePropertyDoc.Size;
                        tblCompliancePropertyDoc.LastModifiedDate = DateTime.UtcNow;
                        tblCompliancePropertyDoc.ModifiedBy = userName;
                        tblCompliancePropertyDoc.ModifiedDate = DateTime.UtcNow;
                        compliancePropertyDocCommonResponse.UpdateCompliancePropertyDoc = compliancePropertyDocRepository.Update(tblCompliancePropertyDoc);
                        if (compliancePropertyDocCommonResponse.UpdateCompliancePropertyDoc != null && compliancePropertyDocCommonResponse.UpdateCompliancePropertyDoc.Id > 0)
                        {
                            compliancePropertyDocCommonResponse.Success = true;
                            return compliancePropertyDocCommonResponse;
                        }
                        else
                            compliancePropertyDocCommonResponse.Message = "Unable to update compliancePropertyDoc.";
                    }
                    else
                        compliancePropertyDocCommonResponse.Message = "CompliancePropertyDoc does not exists.";
                }
                else
                    compliancePropertyDocCommonResponse.Message = "Supplied compliancePropertyDoc information is not valid.";
            }
            catch (Exception ex)
            {
                compliancePropertyDocCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return compliancePropertyDocCommonResponse;
        }

        public CompliancePropertyDocCommonResponse SoftDelete(int compliancePropertyDocId, string userName)
        {
            CompliancePropertyDocCommonResponse compliancePropertyDocCommonResponse = new CompliancePropertyDocCommonResponse();
            try
            {
                compliancePropertyDocCommonResponse.Success = false;
                if (compliancePropertyDocId > 0)
                {
                    TblCompliancePropertyDoc tblCompliancePropertyDoc = compliancePropertyDocRepository.GetCompliancePropertyDocById(compliancePropertyDocId);
                    if (tblCompliancePropertyDoc != null && tblCompliancePropertyDoc.Id > 0)
                    {
                        tblCompliancePropertyDoc.ModifiedBy = userName;
                        tblCompliancePropertyDoc.ModifiedDate = DateTime.UtcNow;
                        tblCompliancePropertyDoc.IsDeleted = true;
                        tblCompliancePropertyDoc.DeletedBy = userName;
                        tblCompliancePropertyDoc.DeletedDate = DateTime.Now;
                        compliancePropertyDocCommonResponse.UpdateCompliancePropertyDoc = compliancePropertyDocRepository.Update(tblCompliancePropertyDoc);
                        if (compliancePropertyDocCommonResponse.UpdateCompliancePropertyDoc != null && compliancePropertyDocCommonResponse.UpdateCompliancePropertyDoc.Id > 0)
                        {
                            compliancePropertyDocCommonResponse.Success = true;
                            return compliancePropertyDocCommonResponse;
                        }
                        else
                            compliancePropertyDocCommonResponse.Message = "Unable to delete compliancePropertyDoc.";
                    }
                    else
                        compliancePropertyDocCommonResponse.Message = "CompliancePropertyDoc does not exists.";
                }
                else
                    compliancePropertyDocCommonResponse.Message = "Supplied compliancePropertyDoc information is not valid.";
            }
            catch (Exception ex)
            {
                compliancePropertyDocCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {

            }
            return compliancePropertyDocCommonResponse;
        }

        public bool HardDelete(int compliancePropertyDocId)
        {
            try
            {
                if (compliancePropertyDocId > 0)
                {
                    return compliancePropertyDocRepository.Delete(compliancePropertyDocId);
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
