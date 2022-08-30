using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.compliances.Models
{
    public class ComplianceCommonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Compliance Compliance { get; set; }
        public UpdateCompliance UpdateCompliance { get; set; }
    }
}
