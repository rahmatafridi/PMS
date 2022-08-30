using ds.pms.apicommon.Models;

using ds.pms.bl.logger;
using ds.pms.bl.optionheaders.Converters;
using ds.pms.bl.optionheaders.IServices;
using ds.pms.bl.optionheaders.Models;
using ds.pms.dal.CustomModels;
using ds.pms.dal.CustomRepository;
using ds.pms.dal.Models;
using ds.pms.helpers.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace pms.bl.clients.Services
{
    public class OptionHeaderService : IOptionHeaderService
    {
        private OptionHeaderRepository optionRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public OptionHeaderService(string _databaseProvider, string _connectionString, ILogger<OptionHeaderService> logger)
        {
            optionRepository = new OptionHeaderRepository(_databaseProvider, _connectionString);
            logging = new Logging(logger);
            _className = this.GetType().Name;
        }

        public OptionHeaderService(string _databaseProvider, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds, ILogger<OptionHeaderService> logger)
        {
            optionRepository = new OptionHeaderRepository(_databaseProvider, _connectionString);
            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(logger);
            _className = this.GetType().Name;
        }

        public PaginatedList<OptionHeaderList> GetActiveOptionHeaderList(int clientId,string search, int? limit = 10, int? page = 1, string sort = "")
        {
            PaginatedList<OptionHeaderList> optionsPaginatedList = new PaginatedList<OptionHeaderList>();
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
                optionsPaginatedList = Pagination.ConvertDalToBl(optionRepository.GetActiveOptionHeaderList(clientId,search, pageSize, pageNumber, sortBy, sortDirection));
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return optionsPaginatedList;
        }

        public TblOptionHeader GetOptionHeaderById(int headerId)
        {
            try
            {
                if (headerId > 0)
                {
                    TblOptionHeader header = optionRepository.GetOptionHeaderById(headerId);
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

        public OptionHeaderCommonResponse AddOptionHeader(OptionHeader addHeader, string userName)
        {
            OptionHeaderCommonResponse headerCommonResponse = new OptionHeaderCommonResponse();
            try
            {
                headerCommonResponse.Success = false;
                if (addHeader != null)
                {

                    TblOptionHeader tblHeader = new TblOptionHeader();
                    tblHeader.Title = addHeader.Title;
                    tblHeader.ClientId = addHeader.ClientId;
                    tblHeader.IsDeleted = false;
                    var data = optionRepository.Add(tblHeader);
                    if (data != null && data.Id > 0)
                    {
                        var optionHeader = new OptionHeader();
                        optionHeader.ClientId = data.ClientId;
                        optionHeader.Id = data.Id;
                        optionHeader.Title = data.Title;

                        headerCommonResponse.Success = true;
                        headerCommonResponse.OptionHeader = optionHeader;
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

        public OptionHeaderCommonResponse UpdateOptionHeader(OptionHeader update, string userName)
        {
            OptionHeaderCommonResponse headerCommonResponse = new OptionHeaderCommonResponse();
            try
            {
                headerCommonResponse.Success = false;
                if (update != null && update.Id > 0)
                {

                    TblOptionHeader tblHeader;
                    tblHeader = optionRepository.GetOptionHeaderById(update.Id);
                    if (tblHeader != null)
                    {
                        tblHeader.Title = update.Title;
                        tblHeader.IsDeleted = false;

                        var data = optionRepository.Update(tblHeader);
                        if (data != null && data.Id > 0)
                        {
                            var optionHeader = new OptionHeader();
                            optionHeader.ClientId = data.ClientId;
                            optionHeader.Id = data.Id;
                            optionHeader.Title = data.Title;

                            headerCommonResponse.Success = true;
                            headerCommonResponse.OptionHeader = optionHeader;
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

        public OptionHeaderCommonResponse SoftDelete(int headerId, string userName)
        {
            OptionHeaderCommonResponse headerCommonResponse = new OptionHeaderCommonResponse();
            try
            {
                headerCommonResponse.Success = false;
                if (headerId > 0)
                {
                    TblOptionHeader tblHeader = optionRepository.GetOptionHeaderById(headerId);
                    if (tblHeader != null && tblHeader.Id > 0)
                    {
                        tblHeader.IsDeleted = true;

                        var data = optionRepository.Update(tblHeader);
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

        public List<TblOptionHeader> GetOptionHeaderListById(int clientId)
        {
            List<TblOptionHeader> list = new List<TblOptionHeader>();
            try
            {

                list = optionRepository.GetActiveOptionHeaderListByClient(clientId) ;
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return list;
        }
    }
}
