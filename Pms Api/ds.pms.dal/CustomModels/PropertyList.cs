using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.dal.CustomModels
{
    public class PropertyList
    {
        public int Id { get; set; } // int
        public int ClientId { get; set; } // int
        public int ProviderId { get; set; } // int
        public int AreaId { get; set; } // int
        public int AgentId { get; set; } // int
        public string Address1 { get; set; } // varchar(100)
        public string Address2 { get; set; } // varchar(100)
        public string Address3 { get; set; } // varchar(100)
        public string City { get; set; } // varchar(100)
        public string PostCode { get; set; } // varchar(50)
        public string County { get; set; } // varchar(100)
        public int? Country { get; set; } // int
        public int? FireExitBlanket { get; set; } // int
        public int? NumberOfRooms { get; set; } // int
        public string TitleNo { get; set; } // nvarchar(50)
        public int? LastRegNumber { get; set; } // int
        public int? LocalAuth { get; set; } // int
        public DateTime? DateSlaStart { get; set; } // datetime
        public DateTime? DateSlaEnd { get; set; } // datetime
        public DateTime? DateLeaseStart { get; set; } // datetime
        public DateTime? DateLeaseEnd { get; set; } // datetime
        public DateTime? DatePreAcceptInsp { get; set; } // datetime
        public DateTime? DateInspection { get; set; } // datetime
        public DateTime? DateExempt { get; set; } // datetime
        public DateTime? DateQuarterlyAudit { get; set; } // datetime
        public DateTime? DateQuarterlyInsp { get; set; } // datetime
        public bool IsPublished { get; set; } // bit
        public string ClientName { get; set; } // nvarchar(50)
        public string ProviderName { get; set; } // nvarchar(50)
    }
}
