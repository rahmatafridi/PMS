using ds.pms.dal.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.compliancepropertydocs.Models
{
    public class CompliancePropertyDoc
    {
        public int Id { get; set; } // int
        public int ComplianceId { get; set; } // int
        public int CompliancePropertyId { get; set; } // int
        public int PropertyId { get; set; } // int
        public DateTime? ExpiryDateFrom { get; set; } // datetime
        public DateTime ExpiryDateTo { get; set; } // datetime
        public string ComplianceName { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime ValidToDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UploadedDate { get; set; }

        public string Name { get; set; } // nvarchar(500)
        public string Extension { get; set; } // nvarchar(50)
        public string MimeType { get; set; } // nvarchar(250)
        public string DocObject { get; set; } // varbinary(max)
        public long Size { get; set; } // bigint
        //public IFormFile File { get; set; }

        public static implicit operator CompliancePropertyDoc(TblCompliancePropertyDoc dbCompliancePropertyDoc)
        {
            if (dbCompliancePropertyDoc != null)
            {
                CompliancePropertyDoc dlCompliancePropertyDoc = new CompliancePropertyDoc()
                {
                    Id = dbCompliancePropertyDoc.Id,
                    ComplianceId = dbCompliancePropertyDoc.ComplianceId,
                    CompliancePropertyId = dbCompliancePropertyDoc.CompliancePropertyId,
                    PropertyId = dbCompliancePropertyDoc.PropertyId,
                    ExpiryDateFrom = dbCompliancePropertyDoc.ExpiryDateFrom,
                    ExpiryDateTo = dbCompliancePropertyDoc.ExpiryDateTo,

                    Name = dbCompliancePropertyDoc.Name,
                    Extension = dbCompliancePropertyDoc.Extension,
                    MimeType = dbCompliancePropertyDoc.MimeType,
                    DocObject = dbCompliancePropertyDoc.DocObject,
                    Size = dbCompliancePropertyDoc.Size,
                };
                return dlCompliancePropertyDoc;
            }
            return null;
        }

        public static implicit operator TblCompliancePropertyDoc(CompliancePropertyDoc dlCompliancePropertyDoc)
        {
            if (dlCompliancePropertyDoc != null)
            {
                TblCompliancePropertyDoc dbCompliancePropertyDoc = new TblCompliancePropertyDoc()
                {
                    Id = dlCompliancePropertyDoc.Id,
                    ComplianceId = dlCompliancePropertyDoc.ComplianceId,
                    CompliancePropertyId = dlCompliancePropertyDoc.CompliancePropertyId,
                    PropertyId = dlCompliancePropertyDoc.PropertyId,
                    ExpiryDateFrom = dlCompliancePropertyDoc.ExpiryDateFrom,
                    ExpiryDateTo = dlCompliancePropertyDoc.ExpiryDateTo,
                    Name = dlCompliancePropertyDoc.Name,
                    Extension = dlCompliancePropertyDoc.Extension,
                    MimeType = dlCompliancePropertyDoc.MimeType,
                    DocObject = dlCompliancePropertyDoc.DocObject,
                    Size = dlCompliancePropertyDoc.Size,
                };
                return dbCompliancePropertyDoc;
            }
            return null;
        }
    }
}
