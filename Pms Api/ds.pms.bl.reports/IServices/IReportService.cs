using ds.pms.bl.reports.Model;
using ds.pms.dal.CustomModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.reports.IServices
{
    public interface IReportService
    {
        List<TenantRoportList> TenantReport(int types);
        List<EmptyRoomList> EmptyRoomReport();
        List<MissingDocumentList> MissingDocumentReport(int clientId,  int id);
        List<ExpiryDocumentList> ExpiyDocumentReport(int days, int typeId);

    }
}
