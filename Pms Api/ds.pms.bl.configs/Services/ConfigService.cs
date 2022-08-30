using ds.pms.apicommon.Models;
using ds.pms.bl.configs.Converters;
using ds.pms.bl.configs.IServices;
using ds.pms.bl.configs.Models;
using ds.pms.bl.logger;
using ds.pms.dal.CustomModels;
using ds.pms.dal.CustomRepository;
using ds.pms.dal.Models;
using ds.pms.helpers.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ds.pms.bl.configs.Services
{
    public class ConfigService : IConfigService
    {
        private ConfigRepository configRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public ConfigService(string _databaseProvider, string _connectionString, ILogger<ConfigService> configServiceLogger)
        {
            configRepository = new ConfigRepository(_databaseProvider, _connectionString);
            logging = new Logging(configServiceLogger);
            _className = this.GetType().Name;
        }

        public ConfigService(string _databaseProvider, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds
            , ILogger<ConfigService> configServiceLogger)
        {
            configRepository = new ConfigRepository(_databaseProvider, _connectionString);
            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(configServiceLogger);
            _className = this.GetType().Name;
        }

        public PaginatedList<Config> GetActiveConfigList(int clientId, string search, int? limit = 10, int? page = 1, string sort = "")
        {
            PaginatedList<Config> ConfigsPaginatedList = new PaginatedList<Config>();
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
                ConfigsPaginatedList = Pagination.ConvertDalToBlUserList(configRepository.GetActiveConfigList(clientId,search, pageSize, pageNumber, sortBy, sortDirection));
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return ConfigsPaginatedList;
        }

        public Config GetConfigById(int configId)
        {
            try
            {
                if (configId > 0)
                {
                    Config config = configRepository.GetConfigById(configId);
                    if (config != null)
                        return config;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        public ConfigCommonResponse AddConfig(Config addConfig, string userName)
        {
            ConfigCommonResponse configCommonResponse = new ConfigCommonResponse();
            try
            {
                configCommonResponse.Success = false;
                if (addConfig != null)
                {
                    TblConfig tblConfig = addConfig;
                    tblConfig.AddedBy = userName;
                    tblConfig.AddedDate = DateTime.UtcNow;
                    configCommonResponse.Config = configRepository.Add(tblConfig);
                    if (configCommonResponse.Config != null && configCommonResponse.Config.Id > 0)
                    {
                        configCommonResponse.Success = true;
                        return configCommonResponse;
                    }
                    else
                        configCommonResponse.Message = "Unable to add config.";
                }
                else
                    configCommonResponse.Message = "Supplied config information is not valid.";
            }
            catch (Exception ex)
            {
                configCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return configCommonResponse;
        }

        public ConfigCommonResponse UpdateConfig(Config updateConfig, string userName)
        {
            ConfigCommonResponse configCommonResponse = new ConfigCommonResponse();
            try
            {
                configCommonResponse.Success = false;
                if (updateConfig != null && updateConfig.Id > 0)
                {
                    TblConfig tblConfig;
                    tblConfig = configRepository.GetConfigById(updateConfig.Id);
                    if (tblConfig != null)
                    {
                        tblConfig.ClientId = updateConfig.ClientId;
                        tblConfig.Key = updateConfig.Key;
                        tblConfig.Value = updateConfig.Value;
                        tblConfig.Description = updateConfig.Description;
                        tblConfig.ModifiedBy = userName;
                        tblConfig.ModifiedDate = DateTime.UtcNow;
                        configCommonResponse.Config = configRepository.Update(tblConfig);
                        if (configCommonResponse.Config != null && configCommonResponse.Config.Id > 0)
                        {
                            configCommonResponse.Success = true;
                            return configCommonResponse;
                        }
                        else
                            configCommonResponse.Message = "Unable to update config.";
                    }
                    else
                        configCommonResponse.Message = "Config does not exists.";
                }
                else
                    configCommonResponse.Message = "Supplied config information is not valid.";
            }
            catch (Exception ex)
            {
                configCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return configCommonResponse;
        }

        public ConfigCommonResponse SoftDelete(int configId, string userName)
        {
            ConfigCommonResponse configCommonResponse = new ConfigCommonResponse();
            try
            {
                configCommonResponse.Success = false;
                if (configId > 0)
                {
                    TblConfig tblConfig = configRepository.GetConfigById(configId);
                    if (tblConfig != null && tblConfig.Id > 0)
                    {
                        tblConfig.ModifiedBy = userName;
                        tblConfig.ModifiedDate = DateTime.UtcNow;
                        tblConfig.IsDeleted = true;
                        tblConfig.DeletedBy = userName;
                        tblConfig.DeletedDate = DateTime.Now;
                        configCommonResponse.Config = configRepository.Update(tblConfig);
                        if (configCommonResponse.Config != null && configCommonResponse.Config.Id > 0)
                        {
                            configCommonResponse.Success = true;
                            return configCommonResponse;
                        }
                        else
                            configCommonResponse.Message = "Unable to delete config.";
                    }
                    else
                        configCommonResponse.Message = "Config does not exists.";
                }
                else
                    configCommonResponse.Message = "Supplied config information is not valid.";
            }
            catch (Exception ex)
            {
                configCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {

            }
            return configCommonResponse;
        }

        public bool HardDelete(int configId)
        {
            try
            {
                if (configId > 0)
                {
                    return configRepository.Delete(configId);
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
