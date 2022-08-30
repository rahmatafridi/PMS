using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.providers.Models
{
    public class ProviderCommonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Provider Provider { get; set; }
        public ProviderUser ProviderUser { get; set; }
        public UpdateProvider UpdateProvider { get; set; }
    }
}
