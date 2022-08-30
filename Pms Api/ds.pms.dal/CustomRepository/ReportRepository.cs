using ds.pms.dal.CustomModels;
using ds.pms.dal.GenericRepository;
using ds.pms.dal.Models;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ds.pms.dal.CustomRepository
{
  public  class ReportRepository : BaseCustomRepository
    {
        private GenericRepository<TblTenant> genericRepository;

        public ReportRepository(string databaseProvider, string connectionString) : base(databaseProvider, connectionString)
        {
            genericRepository = new GenericRepository<TblTenant>(databaseProvider, connectionString);
           
        }

        public List<TenantRoportList> TenantReport(int type)
        {
            List<TenantRoportList> list = new List<TenantRoportList>();
            using(dataContext = new PmsDB(providerName, connectionString))
            {
                //var query = (from tenant in dataContext.TblTenants
                //             where role.IsActive
                //             select new TenantRoportList
                //             {
                //                 Id = role.Id,
                //                 Name = role.Name,
                //                 Description = role.Description,
                //                 ClientId = role.ClientId,
                //                 ClientName = client.Name ?? string.Empty,
                //             });
                if (type == 1) {
                    var data = dataContext.TblTenants.Where(x => x.IsDeleted == false).ToList();
                    foreach (var item in data)
                    {
                        var gender = dataContext.TblOptions.FirstOrDefault(x => x.Id == item.GenderId);
                        var localAuthoruty = dataContext.TblOptions.FirstOrDefault(x => x.Id == item.LocalAuthority);
                        var referral = dataContext.TblOptions.FirstOrDefault(x => x.Id == item.ReferralMethod);
                        var ethnicty = dataContext.TblOptions.FirstOrDefault(x => x.Id == item.EthnicityId);

                        list.Add(new TenantRoportList()
                        {
                            Address = item.Address1,
                            CRN = item.ClaimNumber,
                            EndDate = DateTime.Now.ToString("dd/MM/yyyy"),
                            Ethnicty = ethnicty.Title,
                            Gender=gender.Title,
                            Id= item.Id,
                            LocalAuthority= localAuthoruty.Title,
                            Name= item.FirstName +" "+ item.LastName,
                            Referral= referral.Title,
                            StartDate = DateTime.Now.ToString("dd/MM/yyyy"),
                            SupportPlan= item.DateSupportPlanReview?.ToString("dd/MM/yyyy")
                        }) ;
                    }
                  }
                if (type == 2)
                {

                    var data = (from tenant in dataContext.TblTenants
                                 from room in dataContext.TblRooms.LeftJoin(c => c.TenantId == tenant.Id )
                                 from property in dataContext.TblProperties.LeftJoin(c=>c.Id== room.PropertyId)
                                 from gender in dataContext.TblOptions.LeftJoin(c=>c.Id== tenant.GenderId)
                                 from localAuthoruty in dataContext.TblOptions.LeftJoin(c => c.Id == tenant.LocalAuthority)
                                 from referral in dataContext.TblOptions.LeftJoin(c => c.Id == tenant.ReferralMethod)
                                 from ethnicty in dataContext.TblOptions.LeftJoin(c => c.Id == tenant.EthnicityId)
                                where room.TenantId == tenant.Id
                                select new TenantRoportList
                                {
                                    Address = tenant.Address1,
                                    CRN = tenant.ClaimNumber,
                                    EndDate = DateTime.Now.ToString("dd/MM/yyyy"),
                                    Ethnicty = ethnicty.Title,
                                    Gender = gender.Title,
                                    Id = tenant.Id,
                                    LocalAuthority = localAuthoruty.Title,
                                    Name = tenant.FirstName + " " + tenant.LastName,
                                    Referral = referral.Title,
                                    StartDate = DateTime.Now.ToString("dd/MM/yyyy"),
                                    SupportPlan = tenant.DateSupportPlanReview.ToString()
                                });
                    list = data.ToList();
                    //var data = (from tenant dataContext.TblTenants
                    //            from room dataContext.TblRooms


                    //            )
                    //            .Where(x => x.IsDeleted == true).ToList();
                }
                if (type == 3)
                {


                    var data = dataContext.TblTenants.Where(x => x.IsDeleted == true).ToList();
                    foreach (var item in data)
                    {
                        var gender = dataContext.TblOptions.FirstOrDefault(x => x.Id == item.GenderId);
                        var localAuthoruty = dataContext.TblOptions.FirstOrDefault(x => x.Id == item.LocalAuthority);
                        var referral = dataContext.TblOptions.FirstOrDefault(x => x.Id == item.ReferralMethod);
                        var ethnicty = dataContext.TblOptions.FirstOrDefault(x => x.Id == item.EthnicityId);

                        list.Add(new TenantRoportList()
                        {
                            Address = item.Address1,
                            CRN = item.ClaimNumber,
                            EndDate = DateTime.Now.ToString("dd/MM/yyyy"),
                            Ethnicty = ethnicty.Title,
                            Gender = gender.Title,
                            Id = item.Id,
                            LocalAuthority = localAuthoruty.Title,
                            Name = item.FirstName + " " + item.LastName,
                            Referral = referral.Title,
                            StartDate = DateTime.Now.ToString("dd/MM/yyyy"),
                            SupportPlan = item.DateSupportPlanReview?.ToString("dd/MM/yyyy")
                        });
                    }
                }

            }
            return list;
        }
        public List<EmptyRoomList> EmptyRooms()
        {
            List<EmptyRoomList> list = new List<EmptyRoomList>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {

                var data = (from room in dataContext.TblRooms
                            from prop in dataContext.TblProperties.LeftJoin(c => c.Id == room.PropertyId)
                            from provider in dataContext.TblProviders.LeftJoin(c => c.Id == prop.ProviderId)
                           
                            where room.IsVacant == false 
                            select new EmptyRoomList
                            {
                               Id=prop.Id,
                               Property= prop.Address1,
                               Provider= provider.Name,
                               Room= (int)room.RoomNo
                            }) ;
                list = data.ToList();
            }
            return list;
        }

        public List<MissingDocumentList> MissingDocument(int clinetId,int id)
        {
            List<MissingDocumentList> list = new List<MissingDocumentList>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                var reslult = dataContext.SpReportDocumentMissing(clinetId,id).ToList();
                foreach (var item in reslult)
                {
                    list.Add(new MissingDocumentList()
                    {
                        Address= item.prop_address,
                        Document= item.title,
                        Id= item.property_id
                    });
                }
                //if (id == 0)
                //{
                //    var data = (from doc in dataContext.TblCompliancePropertyDocs
                //                from comp in dataContext.TblCompliances.LeftJoin(c => c.Id == doc.ComplianceId)

                //                from property in dataContext.TblProperties.LeftJoin(c => c.Id == doc.PropertyId)

                //                where doc.DocObject == null || doc.DocObject == ""
                //                select new MissingDocumentList
                //                {
                //                    Address = property.Address1,
                //                    Document = comp.Title,
                //                    Id = property.Id
                //                });
                //    list = data.ToList();
                //}
                //else
                //{
                //    var data = (from doc in dataContext.TblCompliancePropertyDocs
                //                from comp in dataContext.TblCompliances.LeftJoin(c => c.Id == doc.ComplianceId)

                //                from property in dataContext.TblProperties.LeftJoin(c => c.Id == doc.PropertyId)

                //                where (doc.DocObject == null || doc.DocObject == "") && doc.ComplianceId == id
                //                select new MissingDocumentList
                //                {
                //                    Address = property.Address1,
                //                    Document = comp.Title,
                //                    Id = property.Id
                //                });
                //    list = data.ToList();
                //}
            }
            return list;
        }
        int DaysBetween(DateTime d1, DateTime d2)
        {
            TimeSpan span = d1.Subtract(d2);
            return (int)span.TotalDays;
        }
        public List<ExpiryDocumentList> ExpiryDocument(int day, int typeId)
        {
            List<ExpiryDocumentList> list = new List<ExpiryDocumentList>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                DateTime today = DateTime.Now;
                if (day == 0 && typeId==0)
                {
                    var data = (from doc in dataContext.TblCompliancePropertyDocs
                                from comp in dataContext.TblCompliances.LeftJoin(c => c.Id == doc.ComplianceId)

                                from property in dataContext.TblProperties.LeftJoin(c => c.Id == doc.PropertyId)

                                where doc.DocObject != null || doc.DocObject != ""
                                select new ExpiryDocumentList
                                {
                                    Address = property.Address1,
                                    Document = comp.Title,
                                    Id = property.Id,
                                    Days = DaysBetween(doc.ExpiryDateTo, today),
                                    Expiry = doc.ExpiryDateTo.ToString("dd/MM/yyyy"),
                                    PropDocId = doc.Id

                                });
                    list = data.ToList();
                }

                else
                {
                    if (day == -1)
                    {
                        var zeor = 0;
                        var data = (from doc in dataContext.TblCompliancePropertyDocs
                                    from comp in dataContext.TblCompliances.LeftJoin(c => c.Id == doc.ComplianceId)

                                    from property in dataContext.TblProperties.LeftJoin(c => c.Id == doc.PropertyId)

                                    where (doc.DocObject != null || doc.DocObject != "") && doc.ComplianceId == typeId
                                    select new ExpiryDocumentList
                                    {
                                        Address = property.Address1,
                                        Document = comp.Title,
                                        Id = property.Id,
                                        Days = DaysBetween(doc.ExpiryDateTo, today),
                                        Expiry = doc.ExpiryDateTo.ToString("dd/MM/yyyy"),
                                        PropDocId = doc.Id
                                    });
                        list = data.Where(x => x.Days < 0).ToList();
                    }

                    if (day == 0) {

                        var data = (from doc in dataContext.TblCompliancePropertyDocs
                                    from comp in dataContext.TblCompliances.LeftJoin(c => c.Id == doc.ComplianceId)

                                    from property in dataContext.TblProperties.LeftJoin(c => c.Id == doc.PropertyId)

                                    where (doc.DocObject != null || doc.DocObject != "") && doc.ComplianceId == typeId
                                    select new ExpiryDocumentList
                                    {
                                        Address = property.Address1,
                                        Document = comp.Title,
                                        Id = property.Id,
                                        Days = DaysBetween(doc.ExpiryDateTo, today),
                                        Expiry = doc.ExpiryDateTo.ToString("dd/MM/yyyy"),
                                        PropDocId = doc.Id
                                    });
                        list = data.ToList();
                    }
                    if (typeId == 0)
                    {
                        var data = (from doc in dataContext.TblCompliancePropertyDocs
                                    from comp in dataContext.TblCompliances.LeftJoin(c => c.Id == doc.ComplianceId)

                                    from property in dataContext.TblProperties.LeftJoin(c => c.Id == doc.PropertyId)

                                    where doc.DocObject != null || doc.DocObject != ""
                                    select new ExpiryDocumentList
                                    {
                                        Address = property.Address1,
                                        Document = comp.Title,
                                        Id = property.Id,
                                        Days = DaysBetween(doc.ExpiryDateTo, today),
                                        Expiry = doc.ExpiryDateTo.ToString("dd/MM/yyyy"),
                                        PropDocId = doc.Id
                                    });
                        list = data.Where(x => x.Days == day).ToList();
                    }
                }
            }
            return list;
        }
    }
}
