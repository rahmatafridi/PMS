using ds.pms.apicommon.Models;
using ds.pms.bl.logger;
using ds.pms.bl.options.Converters;
using ds.pms.bl.options.IServices;
using ds.pms.bl.options.Models;
using ds.pms.dal.CustomRepository;
using ds.pms.dal.Models;
using ds.pms.helpers.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ds.pms.bl.options.Services
{
    public class OptionService : IOptionService
    {
        private OptionRepository optionRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public OptionService(string _databaseProvider, string _connectionString, ILogger<OptionService> logger)
        {
            optionRepository = new OptionRepository(_databaseProvider, _connectionString);
            logging = new Logging(logger);
            _className = this.GetType().Name;
        }

        public OptionService(string _databaseProvider, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds, ILogger<OptionService> logger)
        {
            optionRepository = new OptionRepository(_databaseProvider, _connectionString);
            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(logger);
            _className = this.GetType().Name;
        }

        public PaginatedList<Option> GetActiveOptionList(string search, int? limit = 10, int? page = 1, string sort = "")
        {
            PaginatedList<Option> optionsPaginatedList = new PaginatedList<Option>();
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
                optionsPaginatedList = Pagination.ConvertDalToBl(optionRepository.GetActiveOptionList(search, pageSize, pageNumber, sortBy, sortDirection));
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return optionsPaginatedList;
        }

        public TblOption GetOptionById(int headerId)
        {
            try
            {
                if (headerId > 0)
                {
                    TblOption header = optionRepository.GetOptionById(headerId);
                    if (header != null)
                        return header;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }
        public List<TblOption> GetOptionListById(int headerId)
        {
            try
            {
                if (headerId > 0)
                {
                    List<TblOption> header = optionRepository.GetOptionListById(headerId);
                    if (header != null)
                        return header;
                }
                //else
                //{
                //    List<>
                //}
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        public OptionCommonResponse AddOption(Option addHeader, string userName)
        {
            OptionCommonResponse headerCommonResponse = new OptionCommonResponse();
            try
            {
                headerCommonResponse.Success = false;
                if (addHeader != null)
                {

                    TblOption tblOption = new TblOption();
                    tblOption.Title = addHeader.Title;
                    tblOption.HeaderId = addHeader.HeaderId;
                    tblOption.SortOrder = addHeader.SortOrder;
                    tblOption.Value = addHeader.Value;
                    tblOption.AddedDate = DateTime.Now;
                    tblOption.IsDeleted = false;


                    var data = optionRepository.Add(tblOption);
                    if (data != null && data.Id > 0)
                    {
                        var option = new Option();
                        option.HeaderId = data.HeaderId;
                        option.Id = data.Id;
                        option.SortOrder = data.SortOrder;
                        option.Title = data.Title;
                        option.Value = data.Value;
                        headerCommonResponse.Success = true;
                        headerCommonResponse.Option = option;
                        return headerCommonResponse;
                    }
                    else
                        headerCommonResponse.Message = "Unable to add Header.";

                }
                else
                    headerCommonResponse.Message = "Supplied client information is not valid.";
            }
            catch (Exception ex)
            {
                headerCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return headerCommonResponse;
        }

        public OptionCommonResponse UpdateOption(Option update, string userName)
        {
            OptionCommonResponse headerCommonResponse = new OptionCommonResponse();
            try
            {
                headerCommonResponse.Success = false;
                if (update != null && update.Id > 0)
                {

                    TblOption tblOption;
                    tblOption = optionRepository.GetOptionById(update.Id);
                    if (tblOption != null)
                    {
                        tblOption.Title = update.Title;
                        tblOption.HeaderId = update.HeaderId;
                        tblOption.SortOrder = update.SortOrder;
                        tblOption.Value = update.Value;
                        tblOption.IsDeleted = false;
                        tblOption.UpdatedDate = DateTime.Now;
                        var data = optionRepository.Update(tblOption);
                        if (data != null && data.Id > 0)
                        {
                            var option = new Option();
                            option.HeaderId = data.HeaderId;
                            option.Id = data.Id;
                            option.SortOrder = data.SortOrder;
                            option.Title = data.Title;
                            option.Value = data.Value;
                            headerCommonResponse.Success = true;
                            headerCommonResponse.Option = option;
                            return headerCommonResponse;
                        }
                        else
                            headerCommonResponse.Message = "Unable to update client.";
                    }
                    else
                        headerCommonResponse.Message = "Header does not exists.";

                }
                else
                    headerCommonResponse.Message = "Supplied Header information is not valid.";
            }
            catch (Exception ex)
            {
                headerCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return headerCommonResponse;
        }

        public OptionCommonResponse SoftDelete(int headerId, string userName)
        {
            OptionCommonResponse headerCommonResponse = new OptionCommonResponse();
            try
            {
                headerCommonResponse.Success = false;
                if (headerId > 0)
                {
                    TblOption tblOption = optionRepository.GetOptionById(headerId);
                    if (tblOption != null && tblOption.Id > 0)
                    {
                        tblOption.IsDeleted = true;

                        var data = optionRepository.Update(tblOption);
                        if (data != null && data.Id > 0)
                        {
                            headerCommonResponse.Success = true;
                            return headerCommonResponse;
                        }
                        else
                            headerCommonResponse.Message = "Unable to delete client.";
                    }
                    else
                        headerCommonResponse.Message = "Client does not exists.";
                }
                else
                    headerCommonResponse.Message = "Supplied client information is not valid.";
            }
            catch (Exception ex)
            {
                headerCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {

            }
            return headerCommonResponse;
        }

        public bool HardDelete(int clientId)
        {
            try
            {
                if (clientId > 0)
                {
                    return optionRepository.Delete(clientId);
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return false;
        }

        public bool IsValidOption(int id, int optionId, string value, string title)
        {
            try
            {
                return !optionRepository.IsValidOption(id,optionId, value, title);
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                return false;
            }
        }

        public List<Option> LoadOption(string headerName)
        {
            OptionCommonResponse headerCommonResponse = new OptionCommonResponse();
            var option = new List<Option>();
            try
            {
               

                var data = optionRepository.LoadOption(headerName);
                if(data != null)
                {
                    foreach (var item in data)
                    {
                        option.Add(new Option()
                        {
                            Id= item.Id,
                            Title= item.Title
                        });
                    }
                }
               
            }
            catch (Exception ex)
            {

                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return option;
        }
    }
}
