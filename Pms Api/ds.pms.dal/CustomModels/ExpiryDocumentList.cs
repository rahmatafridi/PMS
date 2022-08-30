using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.dal.CustomModels
{
    public class ExpiryDocumentList
    {
        public int Id { get; set; }
        public string Document { get; set; }
        public string Expiry { get; set; }
        public int Days { get; set; }
        public string Address { get; set; }
        public int  PropDocId { get; set; }
    }
}
