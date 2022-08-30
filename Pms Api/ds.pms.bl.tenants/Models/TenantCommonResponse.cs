using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.tenants.Models
{
    public class TenantCommonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Tenant Tenant { get; set; }
    }
}
