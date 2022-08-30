using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.compliancepropertydocs.Models
{
    public class CompliancePropertyDocCommonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public CompliancePropertyDoc CompliancePropertyDoc { get; set; }
        public UpdateCompliancePropertyDoc UpdateCompliancePropertyDoc { get; set; }
    }
}
