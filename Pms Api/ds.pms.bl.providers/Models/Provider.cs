using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.providers.Models
{
    public class Provider
    {
        public int Id { get; set; } // int
        public int ClientId { get; set; } // int
        public string Name { get; set; } // varchar(200)
        public string Email { get; set; } // varchar(100)
        public string Mobile { get; set; } // varchar(20)
        public string Address1 { get; set; } // varchar(2000)
        public string Address2 { get; set; } // varchar(2000)
        public string Address3 { get; set; } // varchar(2000)
        public string PostCode { get; set; } // varchar(20)
        public string City { get; set; } // varchar(200)
        public string County { get; set; } // varchar(200)
        public string ClientName { get; set; } // nvarchar(256)
        public bool IsActive { get; set; }

        public static implicit operator Provider(TblProvider dbProvider)
        {
            if (dbProvider != null)
            {
                Provider dlProvider = new Provider()
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
                    IsActive= dbProvider.IsActive
                };
                return dlProvider;
            }
            return null;
        }

        public static implicit operator TblProvider(Provider dlProvider)
        {
            if (dlProvider != null)
            {
                TblProvider dbProvider = new TblProvider()
                {
                    Id = dlProvider.Id,
                    ClientId = dlProvider.ClientId,
                    Name = dlProvider.Name,
                    Email = dlProvider.Email,
                    Mobile = dlProvider.Mobile,
                    Address1 = dlProvider.Address1,
                    Address2 = dlProvider.Address2,
                    Address3 = dlProvider.Address3,
                    PostCode = dlProvider.PostCode,
                    City = dlProvider.City,
                    County = dlProvider.County,
                    IsActive = dlProvider.IsActive

                };
                return dbProvider;
            }
            return null;
        }

        public static implicit operator Provider(ProviderList dbProvider)
        {
            if (dbProvider != null)
            {
                Provider dlProvider = new Provider()
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
                    ClientName = dbProvider.ClientName,
                    IsActive = dbProvider.IsActive

                };
                return dlProvider;
            }
            return null;
        }

        public static implicit operator ProviderList(Provider dlProvider)
        {
            if (dlProvider != null)
            {
                ProviderList dbProvider = new ProviderList()
                {
                    Id = dlProvider.Id,
                    ClientId = dlProvider.ClientId,
                    Name = dlProvider.Name,
                    Email = dlProvider.Email,
                    Mobile = dlProvider.Mobile,
                    Address1 = dlProvider.Address1,
                    Address2 = dlProvider.Address2,
                    Address3 = dlProvider.Address3,
                    PostCode = dlProvider.PostCode,
                    City = dlProvider.City,
                    County = dlProvider.County,
                    ClientName = dlProvider.ClientName,
                    IsActive=dlProvider.IsActive
                };
                return dbProvider;
            }
            return null;
        }
    }
}
