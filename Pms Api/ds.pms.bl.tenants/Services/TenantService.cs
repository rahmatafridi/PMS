using ds.pms.apicommon.Models;
using ds.pms.bl.logger;
using ds.pms.bl.tenants.Converters;
using ds.pms.bl.tenants.IServices;
using ds.pms.bl.tenants.Models;
using ds.pms.dal.CustomRepository;
using ds.pms.dal.Models;
using ds.pms.helpers.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ds.pms.bl.tenants.Services
{
    public class TenantService : ITenantService
    {
        private TenantRepository tenantRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public TenantService(string _databaseProvider, string _connectionString, ILogger<TenantService> tenantServiceLogger)
        {
            tenantRepository = new TenantRepository(_databaseProvider, _connectionString);
            logging = new Logging(tenantServiceLogger);
            _className = this.GetType().Name;
        }

        public TenantService(string _databaseProvider, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds
            , ILogger<TenantService> tenantServiceLogger)
        {
            tenantRepository = new TenantRepository(_databaseProvider, _connectionString);
            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(tenantServiceLogger);
            _className = this.GetType().Name;
        }

        public PaginatedList<Tenant> GetActiveTenantList(int clientId, string search, int? limit = 10, int? page = 1, string sort = "")
        {
            PaginatedList<Tenant> tenantsPaginatedList = new PaginatedList<Tenant>();
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
                tenantsPaginatedList = Pagination.ConvertDalToBl(tenantRepository.GetActiveTenantList(clientId, search, pageSize, pageNumber, sortBy, sortDirection));
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return tenantsPaginatedList;
        }

        public Tenant GetTenantById(int tenantId)
        {
            try
            {
                if (tenantId > 0)
                {
                    Tenant tenant = tenantRepository.GetTenantById(tenantId);
                    if (tenant != null)
                        return tenant;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        public TenantCommonResponse AddTenant(Tenant addTenant, string userName)
        {
            TenantCommonResponse tenantCommonResponse = new TenantCommonResponse();
            try
            {
                tenantCommonResponse.Success = false;
                if (addTenant != null)
                {
                    //if (IsValidEmail(addTenant.Email))
                    //{
                    TblTenant tblTenant = addTenant;
                    tblTenant.AddedBy = userName;
                    tblTenant.AddedDate = DateTime.UtcNow;
                    tblTenant.IsDeleted = false;
                    tenantCommonResponse.Tenant = tenantRepository.Add(tblTenant);
                    if (tenantCommonResponse.Tenant != null && tenantCommonResponse.Tenant.Id > 0)
                    {
                        tenantCommonResponse.Success = true;
                        return tenantCommonResponse;
                    }
                    else
                        tenantCommonResponse.Message = "Unable to add tenant.";
                    //}
                    //else
                    //    TenantCommonResponse.Message = "Email already in use.";
                }
                else
                    tenantCommonResponse.Message = "Supplied tenant information is not valid.";
            }
            catch (Exception ex)
            {
                tenantCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return tenantCommonResponse;
        }

        public TenantCommonResponse UpdateTenant(Tenant updateTenant, string userName)
        {
            TenantCommonResponse tenantCommonResponse = new TenantCommonResponse();
            try
            {
                tenantCommonResponse.Success = false;
                if (updateTenant != null && updateTenant.Id > 0)
                {
                    //if (IsValidEmail(updateTenant.Id, updateTenant.Email))
                    //{
                    TblTenant tblTenant;
                    tblTenant = tenantRepository.GetTenantById(updateTenant.Id);
                    if (tblTenant != null)
                    {
                        tblTenant.ProviderId = updateTenant.ProviderId;
                        tblTenant.ClientId = updateTenant.ClientId;
                        tblTenant.FirstName = updateTenant.FirstName;
                        tblTenant.LastName = updateTenant.LastName;
                        tblTenant.GenderId = updateTenant.Gender;
                        tblTenant.EthnicityId = updateTenant.Ethnicity;
                        tblTenant.ClaimNumber = updateTenant.ClaimNumber;
                        tblTenant.ReferralMethod = updateTenant.ReferralMethod;
                        tblTenant.LocalAuthority = updateTenant.LocalAuthority;
                        tblTenant.DateSupportPlanCompleted = updateTenant.DateSupportPlanCompleted;
                        tblTenant.DateRiskAssessmentCompleted = updateTenant.DateRiskAssessmentCompleted;
                        tblTenant.DatePreAcceptanceInspection = updateTenant.DatePreAcceptanceInspection;
                        tblTenant.DateRiskAssessmentReview = updateTenant.DateRiskAssessmentReview;
                        tblTenant.DateSupportPlanReview = updateTenant.DateSupportPlanReview;
                        tblTenant.NiNumber = updateTenant.NiNumber;
                        tblTenant.Dob= updateTenant.Dob;
                        tblTenant.Email = updateTenant.Email;
                        tblTenant.Mobile = updateTenant.Mobile;
                        tblTenant.Address1 = updateTenant.Address1;
                        tblTenant.Address2 = updateTenant.Address2;
                        tblTenant.Address3 = updateTenant.Address3;
                        tblTenant.PostCode = updateTenant.PostCode;
                        tblTenant.City = updateTenant.City;
                        tblTenant.County = updateTenant.County;
                        tblTenant.ModifiedBy = userName;
                        tblTenant.ModifiedDate = DateTime.UtcNow;
                        tenantCommonResponse.Tenant = tenantRepository.Update(tblTenant);
                        if (tenantCommonResponse.Tenant != null && tenantCommonResponse.Tenant.Id > 0)
                        {
                            tenantCommonResponse.Success = true;
                            return tenantCommonResponse;
                        }
                        else
                            tenantCommonResponse.Message = "Unable to update tenant.";
                    }
                    else
                        tenantCommonResponse.Message = "User does not exists.";
                    //}
                    //else
                    //    TenantCommonResponse.Message = "Email already in use.";
                }
                else
                    tenantCommonResponse.Message = "Supplied tenant information is not valid.";
            }
            catch (Exception ex)
            {
                tenantCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return tenantCommonResponse;
        }

        public TenantCommonResponse SoftDelete(int tenantId, string userName)
        {
            TenantCommonResponse tenantCommonResponse = new TenantCommonResponse();
            try
            {
                tenantCommonResponse.Success = false;
                if (tenantId > 0)
                {
                    TblTenant tblTenant = tenantRepository.GetTenantById(tenantId);
                    if (tblTenant != null && tblTenant.Id > 0)
                    {
                        tblTenant.ModifiedBy = userName;
                        tblTenant.ModifiedDate = DateTime.UtcNow;
                        tblTenant.IsDeleted = true;
                        tblTenant.DeletedBy = userName;
                        tblTenant.DeletedDate = DateTime.Now;
                        tenantCommonResponse.Tenant = tenantRepository.Update(tblTenant);
                        if (tenantCommonResponse.Tenant != null && tenantCommonResponse.Tenant.Id > 0)
                        {
                            tenantCommonResponse.Success = true;
                            return tenantCommonResponse;
                        }
                        else
                            tenantCommonResponse.Message = "Unable to delete tenant.";
                    }
                    else
                        tenantCommonResponse.Message = "Tenant does not exists.";
                }
                else
                    tenantCommonResponse.Message = "Supplied tenant information is not valid.";
            }
            catch (Exception ex)
            {
                tenantCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {

            }
            return tenantCommonResponse;
        }

        public bool HardDelete(int tenantId)
        {
            try
            {
                if (tenantId > 0)
                {
                    return tenantRepository.Delete(tenantId);
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
