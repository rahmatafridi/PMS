using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.dal.CustomModels
{
    public class ProviderList
    {
        public int Id { get; set; } // int
        public int ClientId { get; set; } // int
        public string Name { get; set; } // varchar(200)
        public string Email { get; set; } // varchar(100)
        public string Mobile { get; set; } // varchar(20)
        public string Address1 { get; set; } // varchar(2000)
        public string Address2 { get; set; } // varchar(2000)
        public string Address3 { get; set; } // varchar(2000)
        public string PostCode { get; set; } // varchar(20)
        public string City { get; set; } // varchar(200)
        public string County { get; set; } // varchar(200)
        public string ClientName { get; set; } // int
        public bool IsActive { get; set; }

    }
}
