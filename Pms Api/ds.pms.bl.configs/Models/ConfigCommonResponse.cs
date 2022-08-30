using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.configs.Models
{
    public class ConfigCommonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Config Config { get; set; }
    }
}
