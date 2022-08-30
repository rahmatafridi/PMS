using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.reports.Model
{
  public   class TenantReportModel
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CRN { get; set; }
        public string LocalAuthority { get; set; }
        public string Gender { get; set; }
        public string Ethnicty { get; set; }
        public string Referral { get; set; }
        public string SupportPlan { get; set; }
        public int Id { get; set; }
    }
}
