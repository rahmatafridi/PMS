using ds.pms.apicommon.Models;
using ds.pms.bl.logger;
using ds.pms.bl.providers.Converters;
using ds.pms.bl.providers.IServices;
using ds.pms.bl.providers.Models;
using ds.pms.dal.CustomRepository;
using ds.pms.dal.Models;
using ds.pms.helpers.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace ds.pms.bl.providers.Services
{
    public class ProviderService : IProviderService
    {
        private ProviderRepository providerRepository;

        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public ProviderService(string _databaseProvider, string _connectionString, ILogger<ProviderService> logger)
        {
            providerRepository = new ProviderRepository(_databaseProvider, _connectionString);

            logging = new Logging(logger);
            _className = this.GetType().Name;
        }

        public ProviderService(string _databaseProvider, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds
            , ILogger<ProviderService> logger)
        {
            providerRepository = new ProviderRepository(_databaseProvider, _connectionString);

            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(logger);
            _className = this.GetType().Name;
        }

        public PaginatedList<Provider> GetActiveProviderList(int clientId, string search, int? limit = 10, int? page = 1, string sort = "")
        {
            PaginatedList<Provider> providersPaginatedList = new PaginatedList<Provider>();
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
                providersPaginatedList = Pagination.ConvertDalToBl(providerRepository.GetActiveProviderList(clientId, search, pageSize, pageNumber, sortBy, sortDirection));
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return providersPaginatedList;
        }

        public Provider GetProviderById(int providerId)
        {
            try
            {
                if (providerId > 0)
                {
                    Provider Provider = providerRepository.GetProviderById(providerId);
                    if (Provider != null)
                        return Provider;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        public ProviderCommonResponse AddProvider(Provider addProvider, string userName)
        {
            ProviderCommonResponse providerCommonResponse = new ProviderCommonResponse();
            try
            {
                providerCommonResponse.Success = false;
                if (addProvider != null)
                {
                    if (IsValidEmail(addProvider.Email))
                    {
                        TblProvider tblProvider = addProvider;
                        tblProvider.IsActive = addProvider.IsActive;
                        tblProvider.AddedBy = userName;
                        tblProvider.AddedDate = DateTime.UtcNow;
                        tblProvider.IsDeleted = false;
                        tblProvider.IsActive = addProvider.IsActive;
                        providerCommonResponse.Provider = providerRepository.Add(tblProvider);
                        if (providerCommonResponse.Provider != null && providerCommonResponse.Provider.Id > 0)
                        {
                            providerCommonResponse.Success = true;
                            return providerCommonResponse;
                        }
                        else
                            providerCommonResponse.Message = "Unable to add Provider.";
                    }
                    else
                        providerCommonResponse.Message = "Email already in use.";
                }
                else
                    providerCommonResponse.Message = "Supplied provider information is not valid.";
            }
            catch (Exception ex)
            {
                providerCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return providerCommonResponse;
        }

        public ProviderCommonResponse UpdateProvider(UpdateProvider updateProvider, string userName)
        {
            ProviderCommonResponse providerCommonResponse = new ProviderCommonResponse();
            try
            {
                providerCommonResponse.Success = false;
                if (updateProvider != null && updateProvider.Id > 0)
                {
                    if (IsValidEmail(updateProvider.Id, updateProvider.Email))
                    {
                        TblProvider tblProvider;
                        tblProvider = providerRepository.GetProviderById(updateProvider.Id);
                        if (tblProvider != null)
                        {
                            tblProvider.ClientId = updateProvider.ClientId;
                            tblProvider.Name = updateProvider.Name;
                            tblProvider.Email = updateProvider.Email;
                            tblProvider.Mobile = updateProvider.Mobile;
                            tblProvider.Address1 = updateProvider.Address1;
                            tblProvider.Address2 = updateProvider.Address2;
                            tblProvider.Address3 = updateProvider.Address3;
                            tblProvider.PostCode = updateProvider.PostCode;
                            tblProvider.City = updateProvider.City;
                            tblProvider.County = updateProvider.County;
                            tblProvider.IsActive = updateProvider.IsActive;
                            tblProvider.ModifiedBy = userName;
                            tblProvider.ModifiedDate = DateTime.UtcNow;
                            providerCommonResponse.UpdateProvider = providerRepository.Update(tblProvider);
                            if (providerCommonResponse.UpdateProvider != null && providerCommonResponse.UpdateProvider.Id > 0)
                            {
                                providerCommonResponse.Success = true;
                                return providerCommonResponse;
                            }
                            else
                                providerCommonResponse.Message = "Unable to update provider.";
                        }
                        else
                            providerCommonResponse.Message = "Provider does not exists.";
                    }
                    else
                        providerCommonResponse.Message = "Email already in use.";
                }
                else
                    providerCommonResponse.Message = "Supplied provider information is not valid.";
            }
            catch (Exception ex)
            {
                providerCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return providerCommonResponse;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                return !providerRepository.DoesEmailExists(email);
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                return false;
            }
        }

        public bool IsValidEmail(long? providerId, string email)
        {
            try
            {
                return !providerRepository.DoesAnyOtherUserUseThisEmail(providerId, email);
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                return false;
            }
        }

        public ProviderCommonResponse SoftDelete(int providerId, string userName)
        {
            ProviderCommonResponse providerCommonResponse = new ProviderCommonResponse();
            try
            {
                providerCommonResponse.Success = false;
                if (providerId > 0)
                {
                    TblProvider tblProvider = providerRepository.GetProviderById(providerId);
                    if (tblProvider != null && tblProvider.Id > 0)
                    {
                        tblProvider.ModifiedBy = userName;
                        tblProvider.ModifiedDate = DateTime.UtcNow;
                        tblProvider.IsDeleted = true;
                        tblProvider.DeletedBy = userName;
                        tblProvider.DeletedDate = DateTime.Now;
                        providerCommonResponse.UpdateProvider = providerRepository.Update(tblProvider);
                        if (providerCommonResponse.UpdateProvider != null && providerCommonResponse.UpdateProvider.Id > 0)
                        {
                            providerCommonResponse.Success = true;
                            return providerCommonResponse;
                        }
                        else
                            providerCommonResponse.Message = "Unable to delete Provider.";
                    }
                    else
                        providerCommonResponse.Message = "Provider does not exists.";
                }
                else
                    providerCommonResponse.Message = "Supplied provider information is not valid.";
            }
            catch (Exception ex)
            {
                providerCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {

            }
            return providerCommonResponse;
        }

        public bool HardDelete(int providerId)
        {
            try
            {
                if (providerId > 0)
                {
                    return providerRepository.Delete(providerId);
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return false;
        }

        public ProviderCommonResponse AddUpdateProviderUser(ProviderUser user)
        {
            ProviderCommonResponse providerCommonResponse = new ProviderCommonResponse();
            try
            {
                providerCommonResponse.Success = false;

                if (user.Id > 0)
                {
                    TblProviderUser tblProviderUser;
                    tblProviderUser = providerRepository.GetProiderUserById(user.Id);
                    if(tblProviderUser != null)
                    {
                        tblProviderUser.UserId = user.UserId;
                        tblProviderUser.ProviderId = user.ProviderId;
                        tblProviderUser.Id = user.Id;
                        var data = providerRepository.UpdateUser(tblProviderUser);
                        if (data != null )
                        {
                            ProviderUser tabledata = new ProviderUser();
                            tabledata.Id = data.Id;
                            tabledata.ProviderId = data.ProviderId;
                            tabledata.UserId = data.UserId;
                            providerCommonResponse.ProviderUser = tabledata;
                            providerCommonResponse.Success = true;

                            return providerCommonResponse;
                        }
                        else
                            providerCommonResponse.Message = "Unable to update provider.";
                    }
                }
                else
                {
                    TblProviderUser tblProvider = new TblProviderUser();
                    tblProvider.ProviderId = user.ProviderId;
                    tblProvider.UserId = user.UserId;

                    var data = providerRepository.AddUser(tblProvider);
                    if (data != null)
                    {
                        ProviderUser tabledata = new ProviderUser();
                        tabledata.Id = data.Id;
                        tabledata.ProviderId = data.ProviderId;
                        tabledata.UserId = data.UserId;
                        providerCommonResponse.ProviderUser = tabledata;
                        providerCommonResponse.Success = true;

                        return providerCommonResponse;
                    }
                    else
                        providerCommonResponse.Message = "Unable to add Provider.";

                }
            }
            catch (Exception ex)
            {
                providerCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return providerCommonResponse;
        }

        public ProviderUser GetProiderUserById(int ProviderId)
        {
            try
            {
                if (ProviderId > 0)
                {
                    var providerModel = new ProviderUser();
                    var Provider = providerRepository.GetProiderUserDetailById(ProviderId);

                    if (Provider != null)
                    {
                        providerModel.Id = Provider.Id;
                        providerModel.ProviderId = Provider.ProviderId;
                        providerModel.UserId = Provider.UserId;
                        return providerModel;
                    }
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }
    }
}
