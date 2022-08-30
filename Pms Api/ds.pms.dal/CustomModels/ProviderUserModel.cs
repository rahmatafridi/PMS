using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.dal.CustomModels
{
   public class ProviderUserModel
    {
        public int Id { get; set; }
        public int ProividerId { get; set; }
        public int UserId { get; set; }
    }
}
