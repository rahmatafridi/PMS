using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.dal.CustomModels
{
    public class ConfigList
    {
        public int Id { get; set; } // int
        public int ClientId { get; set; } // int
        public string Client { get; set; }
        public string Key { get; set; } // varchar(50)
        public string Value { get; set; } // varchar(500)
        public string Description { get; set; } // varchar(500)
    }
}
