using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.dal.CustomModels
{
   public class DocumentList
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string MimeType { get; set; }
        public string DocObject { get; set; }
        public byte[] DocObjectArr { get; set; }

        public long Size { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? ParentId { get; set; }
        public int? TypeId { get; set; }
        public int? ClinetId { get; set; }
    }
}
