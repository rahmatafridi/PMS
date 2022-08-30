using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.tenants.Models
{
    public class Tenant
    {
        public int Id { get; set; } // int
        public int ClientId { get; set; } // int
        public int ProviderId { get; set; } // int
        public string FirstName { get; set; } // varchar(100)
        public string LastName { get; set; } // varchar(100)
        public int? Gender { get; set; } // int
        public int? Ethnicity { get; set; } // int
        public string ClaimNumber { get; set; } // varchar(50)
        public int? ReferralMethod { get; set; } // int
        public int? LocalAuthority { get; set; } // int
        public DateTime? DateSupportPlanCompleted { get; set; } // date
        public DateTime? DateRiskAssessmentCompleted { get; set; } // date
        public DateTime? DatePreAcceptanceInspection { get; set; } // date
        public DateTime? DateRiskAssessmentReview { get; set; } // date
        public DateTime? DateSupportPlanReview { get; set; } // date
        public string NiNumber { get; set; } // varchar(10)
        public DateTime? Dob { get; set; } // date
        public string Email { get; set; } // varchar(100)
        public string Mobile { get; set; } // varchar(15)
        public string Address1 { get; set; } // varchar(200)
        public string Address2 { get; set; } // varchar(200)
        public string Address3 { get; set; } // varchar(200)
        public string City { get; set; } // varchar(100)
        public string PostCode { get; set; } // varchar(50)
        public string County { get; set; } // varchar(100)

        public static implicit operator Tenant(TblTenant dbTenant)
        {
            if (dbTenant != null)
            {
                Tenant dlTenant = new Tenant()
                {
                    Id = dbTenant.Id,
                    ClientId = dbTenant.ClientId,
                    ProviderId = dbTenant.ProviderId,
                    FirstName = dbTenant.FirstName,
                    LastName = dbTenant.LastName,
                    Gender = dbTenant.GenderId,
                    Ethnicity = dbTenant.EthnicityId,
                    ClaimNumber = dbTenant.ClaimNumber,
                    ReferralMethod = dbTenant.ReferralMethod,
                    LocalAuthority = dbTenant.LocalAuthority,
                    DateSupportPlanCompleted = dbTenant.DateSupportPlanCompleted,
                    DateRiskAssessmentCompleted = dbTenant.DateRiskAssessmentCompleted,
                    DatePreAcceptanceInspection = dbTenant.DatePreAcceptanceInspection,
                    DateRiskAssessmentReview = dbTenant.DateRiskAssessmentReview,
                    DateSupportPlanReview = dbTenant.DateSupportPlanReview,
                    NiNumber = dbTenant.NiNumber,
                    Dob = dbTenant.Dob,
                    Email = dbTenant.Email,
                    Mobile = dbTenant.Mobile,
                    Address1 = dbTenant.Address1,
                    Address2 = dbTenant.Address2,
                    Address3 = dbTenant.Address3,
                    City = dbTenant.City,
                    PostCode = dbTenant.PostCode,
                    County = dbTenant.County,
                };
                return dlTenant;
            }
            return null;
        }

        public static implicit operator TblTenant(Tenant dlTenant)
        {
            if (dlTenant != null)
            {
                TblTenant dbTenant = new TblTenant()
                {
                    Id = dlTenant.Id,
                    ClientId = dlTenant.ClientId,
                    ProviderId = dlTenant.ProviderId,
                    FirstName = dlTenant.FirstName,
                    LastName = dlTenant.LastName,
                    GenderId = dlTenant.Gender,
                    EthnicityId = dlTenant.Ethnicity,
                    ClaimNumber = dlTenant.ClaimNumber,
                    ReferralMethod = dlTenant.ReferralMethod,
                    LocalAuthority = dlTenant.LocalAuthority,
                    DateSupportPlanCompleted = dlTenant.DateSupportPlanCompleted,
                    DateRiskAssessmentCompleted = dlTenant.DateRiskAssessmentCompleted,
                    DatePreAcceptanceInspection = dlTenant.DatePreAcceptanceInspection,
                    DateRiskAssessmentReview = dlTenant.DateRiskAssessmentReview,
                    DateSupportPlanReview = dlTenant.DateSupportPlanReview,
                    NiNumber = dlTenant.NiNumber,
                    Dob = dlTenant.Dob,
                    Email = dlTenant.Email,
                    Mobile = dlTenant.Mobile,
                    Address1 = dlTenant.Address1,
                    Address2 = dlTenant.Address2,
                    Address3 = dlTenant.Address3,
                    City = dlTenant.City,
                    PostCode = dlTenant.PostCode,
                    County = dlTenant.County,
                };
                return dbTenant;
            }
            return null;
        }
    }
}
