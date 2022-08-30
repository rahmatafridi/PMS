using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.compliances.Models
{
    public class Compliance
    {
		public int Id { get; set; } // int
		public string Title { get; set; } // varchar(100)
		public bool IsDefault { get; set; } // bit
		public int? SortOrder { get; set; } // int
		public bool IsRequired { get; set; } // bit
		public int? DefaultRenewValue { get; set; } // int
		public int? DefaulRenewType { get; set; } // int
		public bool IsVisibleToProvider { get; set; } // bit

        public static implicit operator Compliance(TblCompliance dbCompliance)
        {
            if (dbCompliance != null)
            {
                Compliance dlCompliance = new Compliance()
                {
                    Id = dbCompliance.Id,
                    Title = dbCompliance.Title,
                    IsDefault = dbCompliance.IsDefault,
                    SortOrder = dbCompliance.SortOrder,
                    IsRequired = dbCompliance.IsRequired,
                    DefaultRenewValue = dbCompliance.DefaultRenewValue,
                    DefaulRenewType = dbCompliance.DefaulRenewType,
                    IsVisibleToProvider = dbCompliance.IsVisibleToProvider,
                };
                return dlCompliance;
            }
            return null;
        }

        public static implicit operator TblCompliance(Compliance dlCompliance)
        {
            if (dlCompliance != null)
            {
                TblCompliance dbCompliance = new TblCompliance()
                {
                    Id = dlCompliance.Id,
                    Title = dlCompliance.Title,
                    IsDefault = dlCompliance.IsDefault,
                    SortOrder = dlCompliance.SortOrder,
                    IsRequired = dlCompliance.IsRequired,
                    DefaultRenewValue = dlCompliance.DefaultRenewValue,
                    DefaulRenewType = dlCompliance.DefaulRenewType,
                    IsVisibleToProvider = dlCompliance.IsVisibleToProvider,
                };
                return dbCompliance;
            }
            return null;
        }
    }
}
