using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.dal.CustomModels
{
  public  class RoomHistoryList
    {
        public int Id { get; set; } // int
        public int? PropertyId { get; set; } // int
        public string Property { get; set; }
        public int? TenantId { get; set; } // int
        public string Tenant { get; set; }
        public int? RoomNo { get; set; } // int
        public string RoomName { get; set; } // varchar(50)
        public DateTime? TenancyStartDate { get; set; } // datetime
        public DateTime? TenancyEndDate { get; set; } // datetime
        public bool? IsTenantIsMoving { get; set; } // bit
        public bool? IsTenantLeaving { get; set; } // bit
    }
}
