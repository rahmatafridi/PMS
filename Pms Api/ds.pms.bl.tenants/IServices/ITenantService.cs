using ds.pms.apicommon.Models;
using ds.pms.bl.tenants.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.tenants.IServices
{
    public interface ITenantService
    {
        PaginatedList<Tenant> GetActiveTenantList(int clientId, string search, int? limit = 10, int? page = 1, string sort = "");
        Tenant GetTenantById(int TenantId);
        TenantCommonResponse AddTenant(Tenant addTenant, string userName);
        TenantCommonResponse UpdateTenant(Tenant updateTenant, string userName);
        //bool IsValidEmail(string email);
        //bool IsValidEmail(long? TenantId, string email);
        TenantCommonResponse SoftDelete(int TenantId, string userName);
        bool HardDelete(int TenantId);
    }
}
