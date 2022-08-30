using ds.pms.apicommon.Models;
using ds.pms.bl.compliancepropertydocs.Models;
using ds.pms.dal.CustomModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.compliancepropertydocs.IServices
{
    public interface ICompliancePropertyDocService
    {
        PaginatedList<PropertyCompianceDocsList> GetActiveCompliancePropertyDocList(int proId,string search, int? limit = 10, int? page = 1, string sort = "");
        CompliancePropertyDoc GetCompliancePropertyDocById(int compliancePropertyDocId);
        CompliancePropertyDocCommonResponse AddCompliancePropertyDoc(CompliancePropertyDoc addCompliancePropertyDoc, string userName);
        CompliancePropertyDocCommonResponse UpdateCompliancePropertyDoc(UpdateCompliancePropertyDoc updateCompliancePropertyDoc, string userName);
        CompliancePropertyDocCommonResponse SoftDelete(int compliancePropertyDocId, string userName);
        bool HardDelete(int compliancePropertyDocId);
    }
}
