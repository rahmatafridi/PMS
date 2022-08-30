using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.compliancepropertydocs.Models
{
    public class UpdateCompliancePropertyDoc : CompliancePropertyDoc
    {
        public DateTime? LastModifiedDate { get; set; } // datetime

        public static implicit operator UpdateCompliancePropertyDoc(TblCompliancePropertyDoc dbCompliancePropertyDoc)
        {
            if (dbCompliancePropertyDoc != null)
            {
                UpdateCompliancePropertyDoc dlUpdateCompliancePropertyDoc = new UpdateCompliancePropertyDoc()
                {
                    Id = dbCompliancePropertyDoc.Id,
                    ComplianceId = dbCompliancePropertyDoc.ComplianceId,
                    CompliancePropertyId = dbCompliancePropertyDoc.CompliancePropertyId,
                    PropertyId = dbCompliancePropertyDoc.PropertyId,
                    ExpiryDateFrom = dbCompliancePropertyDoc.ExpiryDateFrom,
                    ExpiryDateTo = dbCompliancePropertyDoc.ExpiryDateTo,
                    //Name = dbCompliancePropertyDoc.Name,
                    //Extension = dbCompliancePropertyDoc.Extension,
                    //LastModifiedDate = dbCompliancePropertyDoc.LastModifiedDate,
                    //MimeType = dbCompliancePropertyDoc.MimeType,
                    //DocObject = dbCompliancePropertyDoc.DocObject,
                    //Size = dbCompliancePropertyDoc.Size,
                };
                return dlUpdateCompliancePropertyDoc;
            }
            return null;
        }

        public static implicit operator TblCompliancePropertyDoc(UpdateCompliancePropertyDoc dlUpdateCompliancePropertyDoc)
        {
            if (dlUpdateCompliancePropertyDoc != null)
            {
                TblCompliancePropertyDoc dbCompliancePropertyDoc = new TblCompliancePropertyDoc()
                {
                    Id = dlUpdateCompliancePropertyDoc.Id,
                    ComplianceId = dlUpdateCompliancePropertyDoc.ComplianceId,
                    CompliancePropertyId = dlUpdateCompliancePropertyDoc.CompliancePropertyId,
                    PropertyId = dlUpdateCompliancePropertyDoc.PropertyId,
                    ExpiryDateFrom = dlUpdateCompliancePropertyDoc.ExpiryDateFrom,
                    ExpiryDateTo = dlUpdateCompliancePropertyDoc.ExpiryDateTo,
                    //Name = dlUpdateCompliancePropertyDoc.Name,
                    //Extension = dlUpdateCompliancePropertyDoc.Extension,
                    //LastModifiedDate = dlUpdateCompliancePropertyDoc.LastModifiedDate,
                    //MimeType = dlUpdateCompliancePropertyDoc.MimeType,
                    //DocObject = dlUpdateCompliancePropertyDoc.DocObject,
                    //Size = dlUpdateCompliancePropertyDoc.Size,
                };
                return dbCompliancePropertyDoc;
            }
            return null;
        }
    }
}
