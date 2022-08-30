using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.properties.Models
{
    public class Property
    {
        public int Id { get; set; } // int
        public int ClientId { get; set; } // int
        public int ProviderId { get; set; } // int
        public int AreaId { get; set; } // int
        public int AgentId { get; set; } // int
        public string Address1 { get; set; } // varchar(100)
        public string Address2 { get; set; } // varchar(100)
        public string Address3 { get; set; } // varchar(100)
        public string City { get; set; } // varchar(100)
        public string PostCode { get; set; } // varchar(50)
        public string County { get; set; } // varchar(100)
        public int? Country { get; set; } // int
        public int? FireExitBlanket { get; set; } // int
        public int? NumberOfRooms { get; set; } // int
        public string TitleNo { get; set; } // nvarchar(50)
        public int? LastRegNumber { get; set; } // int
        public int? LocalAuth { get; set; } // int
        public DateTime? DateSlaStart { get; set; } // datetime
        public DateTime? DateSlaEnd { get; set; } // datetime
        public DateTime? DateLeaseStart { get; set; } // datetime
        public DateTime? DateLeaseEnd { get; set; } // datetime
        public DateTime? DatePreAcceptInsp { get; set; } // datetime
        public DateTime? DateInspection { get; set; } // datetime
        public DateTime? DateExempt { get; set; } // datetime
        public DateTime? DateQuarterlyAudit { get; set; } // datetime
        public DateTime? DateQuarterlyInsp { get; set; } // datetime
        public bool IsPublished { get; set; } // bit
        public string ClientName { get; set; } // nvarchar(50)
        public string ProviderName { get; set; } // nvarchar(50)

        public static implicit operator Property(TblProperty dbProperty)
        {
            if (dbProperty != null)
            {
                Property dlProperty = new Property()
                {
                    Id = dbProperty.Id,
                    ClientId = dbProperty.ClientId,
                    ProviderId = dbProperty.ProviderId,
                    AreaId = dbProperty.AreaId,
                    AgentId = dbProperty.AgentId,
                    Address1 = dbProperty.Address1,
                    Address2 = dbProperty.Address2,
                    Address3 = dbProperty.Address3,
                    PostCode = dbProperty.PostCode,
                    City = dbProperty.City,
                    County = dbProperty.County,
                    Country = dbProperty.Country,
                    FireExitBlanket = dbProperty.FireExitBlanket,
                    NumberOfRooms = dbProperty.NumberOfRooms,
                    TitleNo = dbProperty.TitleNo,
                    LastRegNumber = dbProperty.LastRegNumber,
                    LocalAuth = dbProperty.LocalAuth,
                    DateSlaStart = dbProperty.DateSlaStart,
                    DateSlaEnd = dbProperty.DateSlaEnd,
                    DateLeaseStart = dbProperty.DateLeaseStart,
                    DateLeaseEnd = dbProperty.DateLeaseEnd,
                    DatePreAcceptInsp = dbProperty.DatePreAcceptInsp,
                    DateInspection = dbProperty.DateInspection,
                    DateExempt = dbProperty.DateExempt,
                    DateQuarterlyAudit = dbProperty.DateQuarterlyAudit,
                    DateQuarterlyInsp = dbProperty.DateQuarterlyInsp,
                    IsPublished = dbProperty.IsPublished,
                };
                return dlProperty;
            }
            return null;
        }

        public static implicit operator TblProperty(Property dlProperty)
        {
            if (dlProperty != null)
            {
                TblProperty dbProperty = new TblProperty()
                {
                    Id = dlProperty.Id,
                    ClientId = dlProperty.ClientId,
                    ProviderId = dlProperty.ProviderId,
                    AreaId = dlProperty.AreaId,
                    AgentId = dlProperty.AgentId,
                    Address1 = dlProperty.Address1,
                    Address2 = dlProperty.Address2,
                    Address3 = dlProperty.Address3,
                    PostCode = dlProperty.PostCode,
                    City = dlProperty.City,
                    County = dlProperty.County,
                    Country = dlProperty.Country,
                    FireExitBlanket = dlProperty.FireExitBlanket,
                    NumberOfRooms = dlProperty.NumberOfRooms,
                    TitleNo = dlProperty.TitleNo,
                    LastRegNumber = dlProperty.LastRegNumber,
                    LocalAuth = dlProperty.LocalAuth,
                    DateSlaStart = dlProperty.DateSlaStart,
                    DateSlaEnd = dlProperty.DateSlaEnd,
                    DateLeaseStart = dlProperty.DateLeaseStart,
                    DateLeaseEnd = dlProperty.DateLeaseEnd,
                    DatePreAcceptInsp = dlProperty.DatePreAcceptInsp,
                    DateInspection = dlProperty.DateInspection,
                    DateExempt = dlProperty.DateExempt,
                    DateQuarterlyAudit = dlProperty.DateQuarterlyAudit,
                    DateQuarterlyInsp = dlProperty.DateQuarterlyInsp,
                    IsPublished = dlProperty.IsPublished,
                };
                return dbProperty;
            }
            return null;
        }

        public static implicit operator Property(PropertyList dbProperty)
        {
            if (dbProperty != null)
            {
                Property dlProperty = new Property()
                {
                    Id = dbProperty.Id,
                    ClientId = dbProperty.ClientId,
                    ProviderId = dbProperty.ProviderId,
                    AreaId = dbProperty.AreaId,
                    AgentId = dbProperty.AgentId,
                    Address1 = dbProperty.Address1,
                    Address2 = dbProperty.Address2,
                    Address3 = dbProperty.Address3,
                    PostCode = dbProperty.PostCode,
                    City = dbProperty.City,
                    County = dbProperty.County,
                    Country = dbProperty.Country,
                    FireExitBlanket = dbProperty.FireExitBlanket,
                    NumberOfRooms = dbProperty.NumberOfRooms,
                    TitleNo = dbProperty.TitleNo,
                    LastRegNumber = dbProperty.LastRegNumber,
                    LocalAuth = dbProperty.LocalAuth,
                    DateSlaStart = dbProperty.DateSlaStart,
                    DateSlaEnd = dbProperty.DateSlaEnd,
                    DateLeaseStart = dbProperty.DateLeaseStart,
                    DateLeaseEnd = dbProperty.DateLeaseEnd,
                    DatePreAcceptInsp = dbProperty.DatePreAcceptInsp,
                    DateInspection = dbProperty.DateInspection,
                    DateExempt = dbProperty.DateExempt,
                    DateQuarterlyAudit = dbProperty.DateQuarterlyAudit,
                    DateQuarterlyInsp = dbProperty.DateQuarterlyInsp,
                    IsPublished = dbProperty.IsPublished,
                    ClientName = dbProperty.ClientName,
                    ProviderName = dbProperty.ProviderName,
                };
                return dlProperty;
            }
            return null;
        }

        public static implicit operator PropertyList(Property dlProperty)
        {
            if (dlProperty != null)
            {
                PropertyList dbProperty = new PropertyList()
                {
                    Id = dlProperty.Id,
                    ClientId = dlProperty.ClientId,
                    ProviderId = dlProperty.ProviderId,
                    AreaId = dlProperty.AreaId,
                    AgentId = dlProperty.AgentId,
                    Address1 = dlProperty.Address1,
                    Address2 = dlProperty.Address2,
                    Address3 = dlProperty.Address3,
                    PostCode = dlProperty.PostCode,
                    City = dlProperty.City,
                    County = dlProperty.County,
                    Country = dlProperty.Country,
                    FireExitBlanket = dlProperty.FireExitBlanket,
                    NumberOfRooms = dlProperty.NumberOfRooms,
                    TitleNo = dlProperty.TitleNo,
                    LastRegNumber = dlProperty.LastRegNumber,
                    LocalAuth = dlProperty.LocalAuth,
                    DateSlaStart = dlProperty.DateSlaStart,
                    DateSlaEnd = dlProperty.DateSlaEnd,
                    DateLeaseStart = dlProperty.DateLeaseStart,
                    DateLeaseEnd = dlProperty.DateLeaseEnd,
                    DatePreAcceptInsp = dlProperty.DatePreAcceptInsp,
                    DateInspection = dlProperty.DateInspection,
                    DateExempt = dlProperty.DateExempt,
                    DateQuarterlyAudit = dlProperty.DateQuarterlyAudit,
                    DateQuarterlyInsp = dlProperty.DateQuarterlyInsp,
                    IsPublished = dlProperty.IsPublished,
                    ClientName = dlProperty.ClientName,
                    ProviderName = dlProperty.ProviderName
                };
                return dbProperty;
            }
            return null;
        }
    }
}
