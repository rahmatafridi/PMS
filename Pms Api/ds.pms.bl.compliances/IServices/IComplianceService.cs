using ds.pms.apicommon.Models;
using ds.pms.bl.compliances.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.compliances.IServices
{
    public interface IComplianceService
    {
        PaginatedList<Compliance> GetActiveComplianceList(string search, int? limit = 10, int? page = 1, string sort = "");
        Compliance GetComplianceById(int complianceId);
        ComplianceCommonResponse AddCompliance(Compliance addCompliance, string userName);
        ComplianceCommonResponse UpdateCompliance(UpdateCompliance updateCompliance, string userName);
        ComplianceCommonResponse SoftDelete(int complianceId, string userName);
        bool HardDelete(int complianceId);
        PaginatedList<Compliance> GetComplianceByClient(int clientId);

    }
}
