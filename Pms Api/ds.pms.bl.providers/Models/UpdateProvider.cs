using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.providers.Models
{
    public class UpdateProvider : Provider
    {
        public bool IsActive { get; set; } // bit

        public static implicit operator UpdateProvider(TblProvider dbProvider)
        {
            if (dbProvider != null)
            {
                UpdateProvider dlUpdateProvider = new UpdateProvider()
                {
                    Id = dbProvider.Id,
                    ClientId = dbProvider.ClientId,
                    Name = dbProvider.Name,
                    Email = dbProvider.Email,
                    Mobile = dbProvider.Mobile,
                    Address1 = dbProvider.Address1,
                    Address2 = dbProvider.Address2,
                    Address3 = dbProvider.Address3,
                    PostCode = dbProvider.PostCode,
                    City = dbProvider.City,
                    County = dbProvider.County,
                    IsActive = dbProvider.IsActive,
                };
                return dlUpdateProvider;
            }
            return null;
        }

        public static implicit operator TblProvider(UpdateProvider dlUpdateProvider)
        {
            if (dlUpdateProvider != null)
            {
                TblProvider dbProvider = new TblProvider()
                {
                    Id = dlUpdateProvider.Id,
                    ClientId = dlUpdateProvider.ClientId,
                    Name = dlUpdateProvider.Name,
                    Email = dlUpdateProvider.Email,
                    Mobile = dlUpdateProvider.Mobile,
                    Address1 = dlUpdateProvider.Address1,
                    Address2 = dlUpdateProvider.Address2,
                    Address3 = dlUpdateProvider.Address3,
                    PostCode = dlUpdateProvider.PostCode,
                    City = dlUpdateProvider.City,
                    County = dlUpdateProvider.County,
                    IsActive = dlUpdateProvider.IsActive,
                };
                return dbProvider;
            }
            return null;
        }
    }
}
