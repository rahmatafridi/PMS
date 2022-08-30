using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.dal.CustomModels
{
    public  class RoomList
    {
        public int Id { get; set; } // int
        public int PropertyId { get; set; } // int
        public int TenantId { get; set; } // int
        public string Tenant { get; set; }
        public int? RoomNo { get; set; } // int
        public string RoomName { get; set; } // varchar(50)
        public DateTime? TenancyStartDate { get; set; } // datetime
        public DateTime? TenancyEndDate { get; set; } // datetime
        public decimal? CoreRent { get; set; } // decimal(18, 2)
        public decimal? ServiceCharge { get; set; } // decimal(18, 2)
        public decimal? PersonalCharge { get; set; } // decimal(18, 2)
    }
}
