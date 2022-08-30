using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.options.Models
{
   public class OptionCommonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Option Option { get; set; }


    }
}
