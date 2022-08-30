using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.providers.Models
{
   public class ProviderUser
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public int UserId { get; set; }
    }
}
