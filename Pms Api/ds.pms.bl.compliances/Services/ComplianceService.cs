using ds.pms.apicommon.Models;
using ds.pms.bl.compliances.Converters;
using ds.pms.bl.compliances.IServices;
using ds.pms.bl.compliances.Models;
using ds.pms.bl.logger;
using ds.pms.dal.CustomRepository;
using ds.pms.dal.Models;
using ds.pms.helpers.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ds.pms.bl.compliances.Services
{
    public class ComplianceService : IComplianceService
    {
        private ComplianceRepository complianceRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public ComplianceService(string _databaseProvider, string _connectionString, ILogger<ComplianceService> complianceServiceLogger)
        {
            complianceRepository = new ComplianceRepository(_databaseProvider, _connectionString);
            logging = new Logging(complianceServiceLogger);
            _className = this.GetType().Name;
        }

        public ComplianceService(string _databaseProvider, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds
            , ILogger<ComplianceService> complianceServiceLogger)
        {
            complianceRepository = new ComplianceRepository(_databaseProvider, _connectionString);
            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(complianceServiceLogger);
            _className = this.GetType().Name;
        }

        public PaginatedList<Compliance> GetActiveComplianceList(string search, int? limit = 10, int? page = 1, string sort = "")
        {
            PaginatedList<Compliance> compliancesPaginatedList = new PaginatedList<Compliance>();
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
                compliancesPaginatedList = Pagination.ConvertDalToBl(complianceRepository.GetActiveComplianceList(search, pageSize, pageNumber, sortBy, sortDirection));
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return compliancesPaginatedList;
        }

        public Compliance GetComplianceById(int complianceId)
        {
            try
            {
                if (complianceId > 0)
                {
                    Compliance compliance = complianceRepository.GetComplianceById(complianceId);
                    if (compliance != null)
                        return compliance;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        public ComplianceCommonResponse AddCompliance(Compliance addCompliance, string userName)
        {
            ComplianceCommonResponse complianceCommonResponse = new ComplianceCommonResponse();
            try
            {
                complianceCommonResponse.Success = false;
                if (addCompliance != null)
                {
                    TblCompliance tblCompliance = addCompliance;
                    tblCompliance.IsActive = true;
                    tblCompliance.AddedBy = userName;
                    tblCompliance.AddedDate = DateTime.UtcNow;
                    tblCompliance.IsDeleted = false;
                    complianceCommonResponse.Compliance = complianceRepository.Add(tblCompliance);
                    if (complianceCommonResponse.Compliance != null && complianceCommonResponse.Compliance.Id > 0)
                    {
                        complianceCommonResponse.Success = true;
                        return complianceCommonResponse;
                    }
                    else
                        complianceCommonResponse.Message = "Unable to add compliance.";
                }
                else
                    complianceCommonResponse.Message = "Supplied compliance information is not valid.";
            }
            catch (Exception ex)
            {
                complianceCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return complianceCommonResponse;
        }

        public ComplianceCommonResponse UpdateCompliance(UpdateCompliance updateCompliance, string userName)
        {
            ComplianceCommonResponse complianceCommonResponse = new ComplianceCommonResponse();
            try
            {
                complianceCommonResponse.Success = false;
                if (updateCompliance != null && updateCompliance.Id > 0)
                {
                    TblCompliance tblCompliance;
                    tblCompliance = complianceRepository.GetComplianceById(updateCompliance.Id);
                    if (tblCompliance != null)
                    {
                        tblCompliance.Id = updateCompliance.Id;
                        tblCompliance.Title = updateCompliance.Title;
                        tblCompliance.IsDefault = updateCompliance.IsDefault;
                        tblCompliance.SortOrder = updateCompliance.SortOrder;
                        tblCompliance.IsRequired = updateCompliance.IsRequired;
                        tblCompliance.DefaultRenewValue = updateCompliance.DefaultRenewValue;
                        tblCompliance.DefaulRenewType = updateCompliance.DefaulRenewType;
                        tblCompliance.IsVisibleToProvider = updateCompliance.IsVisibleToProvider;
                        tblCompliance.IsActive = updateCompliance.IsActive;
                        tblCompliance.ModifiedBy = userName;
                        tblCompliance.ModifiedDate = DateTime.UtcNow;
                        complianceCommonResponse.UpdateCompliance = complianceRepository.Update(tblCompliance);
                        if (complianceCommonResponse.UpdateCompliance != null && complianceCommonResponse.UpdateCompliance.Id > 0)
                        {
                            complianceCommonResponse.Success = true;
                            return complianceCommonResponse;
                        }
                        else
                            complianceCommonResponse.Message = "Unable to update compliance.";
                    }
                    else
                        complianceCommonResponse.Message = "Compliance does not exists.";
                }
                else
                    complianceCommonResponse.Message = "Supplied compliance information is not valid.";
            }
            catch (Exception ex)
            {
                complianceCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return complianceCommonResponse;
        }

        public ComplianceCommonResponse SoftDelete(int complianceId, string userName)
        {
            ComplianceCommonResponse complianceCommonResponse = new ComplianceCommonResponse();
            try
            {
                complianceCommonResponse.Success = false;
                if (complianceId > 0)
                {
                    TblCompliance tblCompliance = complianceRepository.GetComplianceById(complianceId);
                    if (tblCompliance != null && tblCompliance.Id > 0)
                    {
                        tblCompliance.ModifiedBy = userName;
                        tblCompliance.ModifiedDate = DateTime.UtcNow;
                        tblCompliance.IsDeleted = true;
                        tblCompliance.DeletedBy = userName;
                        tblCompliance.DeletedDate = DateTime.Now;
                        complianceCommonResponse.UpdateCompliance = complianceRepository.Update(tblCompliance);
                        if (complianceCommonResponse.UpdateCompliance != null && complianceCommonResponse.UpdateCompliance.Id > 0)
                        {
                            complianceCommonResponse.Success = true;
                            return complianceCommonResponse;
                        }
                        else
                            complianceCommonResponse.Message = "Unable to delete compliance.";
                    }
                    else
                        complianceCommonResponse.Message = "Compliance does not exists.";
                }
                else
                    complianceCommonResponse.Message = "Supplied compliance information is not valid.";
            }
            catch (Exception ex)
            {
                complianceCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {

            }
            return complianceCommonResponse;
        }

        public bool HardDelete(int complianceId)
        {
            try
            {
                if (complianceId > 0)
                {
                    return complianceRepository.Delete(complianceId);
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return false;
        }

        public PaginatedList<Compliance> GetComplianceByClient(int clientId)
        {
            PaginatedList<Compliance> compliancesPaginatedList = new PaginatedList<Compliance>();
            try
            {
                
               
                compliancesPaginatedList = Pagination.ConvertDalToBl(complianceRepository.GetActiveComplianceByClient(clientId));
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return compliancesPaginatedList;
        }
    }
}
