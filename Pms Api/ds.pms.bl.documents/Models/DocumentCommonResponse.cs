using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.documents.Models
{
    public class DocumentCommonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Document Document { get; set; }
    }
}
