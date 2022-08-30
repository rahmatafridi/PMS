using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.compliances.Models
{
    public class UpdateCompliance : Compliance
    {
        public bool IsActive { get; set; } // bit

        public static implicit operator UpdateCompliance(TblCompliance dbCompliance)
        {
            if (dbCompliance != null)
            {
                UpdateCompliance dlUpdateCompliance = new UpdateCompliance()
                {
                    Id = dbCompliance.Id,
                    Title = dbCompliance.Title,
                    IsDefault = dbCompliance.IsDefault,
                    SortOrder = dbCompliance.SortOrder,
                    IsRequired = dbCompliance.IsRequired,
                    DefaultRenewValue = dbCompliance.DefaultRenewValue,
                    DefaulRenewType = dbCompliance.DefaulRenewType,
                    IsVisibleToProvider = dbCompliance.IsVisibleToProvider,
                    IsActive = dbCompliance.IsActive,
                };
                return dlUpdateCompliance;
            }
            return null;
        }

        public static implicit operator TblCompliance(UpdateCompliance dlUpdateCompliance)
        {
            if (dlUpdateCompliance != null)
            {
                TblCompliance dbCompliance = new TblCompliance()
                {
                    Id = dlUpdateCompliance.Id,
                    Title = dlUpdateCompliance.Title,
                    IsDefault = dlUpdateCompliance.IsDefault,
                    SortOrder = dlUpdateCompliance.SortOrder,
                    IsRequired = dlUpdateCompliance.IsRequired,
                    DefaultRenewValue = dlUpdateCompliance.DefaultRenewValue,
                    DefaulRenewType = dlUpdateCompliance.DefaulRenewType,
                    IsVisibleToProvider = dlUpdateCompliance.IsVisibleToProvider,
                    IsActive = dlUpdateCompliance.IsActive,
                };
                return dbCompliance;
            }
            return null;
        }
    }
}
