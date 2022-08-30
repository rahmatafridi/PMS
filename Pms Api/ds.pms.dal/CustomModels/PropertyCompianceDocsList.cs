using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.dal.CustomModels
{
   public class PropertyCompianceDocsList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
       public string ComplianceName { get; set; }
        public DateTime? ValidFromDate { get; set; }
        public DateTime? ValidToDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UploadedDate { get; set; }
    }
}
