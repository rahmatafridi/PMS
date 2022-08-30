using ds.pms.bl.logger;
using ds.pms.bl.reports.IServices;
using ds.pms.bl.reports.Model;
using ds.pms.dal.CustomModels;
using ds.pms.dal.CustomRepository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ds.pms.bl.reports.Services
{
    public class ReportService : IReportService
    {
        private ReportRepository reportRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public ReportService(string _databaseProvider, string _connectionString, string jwtSecret, int jwtExpirationSeconds, ILogger<ReportService> reportServicelogger)
        {
            reportRepository = new ReportRepository(_databaseProvider, _connectionString);
            logging = new Logging(reportServicelogger);
            _className = this.GetType().Name;
        }

        public List<TenantRoportList> TenantReport(int types)
        {
            List<TenantRoportList> list = new List<TenantRoportList>();
            try
            {
                list = reportRepository.TenantReport(types);
            }
            catch (Exception ex)
            {

                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return list;
        }

        public List<EmptyRoomList> EmptyRoomReport()
        {
            List<EmptyRoomList> list = new List<EmptyRoomList>();
            try
            {
                list= reportRepository.EmptyRooms();
            }
            catch (Exception ex)
            {

                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return list;
        }

        public List<MissingDocumentList> MissingDocumentReport(int clientId,int id)
        {
            List<MissingDocumentList> list = new List<MissingDocumentList>();
            try
            {
                list = reportRepository.MissingDocument(clientId, id);
            }
            catch (Exception ex)
            {

                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return list;
        }

        public List<ExpiryDocumentList> ExpiyDocumentReport(int days, int typeId)
        {
            List<ExpiryDocumentList> list = new List<ExpiryDocumentList>();
            try
            {
                list = reportRepository.ExpiryDocument(days,typeId);
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
