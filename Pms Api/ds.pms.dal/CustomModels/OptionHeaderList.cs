using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.dal.CustomModels
{
  public  class OptionHeaderList
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public string Title { get; set; }
        public string ClientName { get; set; }
    }
}
