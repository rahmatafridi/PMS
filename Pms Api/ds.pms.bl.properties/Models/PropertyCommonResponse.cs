using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.properties.Models
{
    public class PropertyCommonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Property Property { get; set; }
    }
}
