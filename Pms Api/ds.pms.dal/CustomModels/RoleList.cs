using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.dal.CustomModels
{
    public class RoleList
    {
        public int Id { get; set; } // int
        public int ClientId { get; set; } // int
        public string Name { get; set; } // nvarchar(100)
        public string Description { get; set; } // nvarchar(256)
        public string ClientName { get; set; } // nvarchar(256)
    }
}
