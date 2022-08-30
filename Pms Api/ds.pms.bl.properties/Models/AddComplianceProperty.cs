using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.properties.Models
{
    public class AddComplianceProperty
    {
        public int PropertyId { get; set; }
        public List<int> ComplianceIds { get; set; }
    }
}
