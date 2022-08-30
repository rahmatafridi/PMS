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
    public class DashboardReporsitory : BaseCustomRepository
    {
        private GenericRepository<TblProperty> propertyGenericRepository;
        public DashboardReporsitory(string databaseProvider, string connectionString) : base(databaseProvider, connectionString)
        {
            propertyGenericRepository = new GenericRepository<TblProperty>(databaseProvider, connectionString);


        }

        public Dashboard GetDashboardData(int clientId)
        {
            var data = new Dashboard();
         

            using (dataContext = new PmsDB(providerName, connectionString))
            {
              
                var result = dataContext.SpDashboardOverview().FirstOrDefault();
                data.TotalProperty = result.property_count.ToString();
                data.OccupancyProperty = result.property_occupancy_overall_rate.ToString();
                data.AssignedProperty = result.property_occupancy_overall_count.ToString();

                data.TotalTenant = result.tenant_count.ToString();
                data.AssignedTenant = result.tenant_occupancy_count.ToString();
                data.OccupancyTenant = result.tenant_occupancy_rate.ToString();

                data.TotalRoom = result.rooms_count.ToString();
                data.AssignedRoom = result.room_occupancy_count.ToString();
                data.OccupancyRoom = result.room_occupancy_rate.ToString();

                var expiryDate = (from cdoc in dataContext.TblCompliancePropertyDocs
                                 join c in dataContext.TblCompliances on cdoc.ComplianceId equals c.Id
                                 select  new MissingDoc
                                 {
                                     expiryDate = cdoc.ExpiryDateTo,
                                     id= c.Id,
                                     propertyId= cdoc.PropertyId
                                 }).Select(x=>x.expiryDate).Distinct().Max();
   

                var MissingCount = dataContext.SpGetMissing(clientId).FirstOrDefault();
                var expiryDoc = dataContext.TblCompliancePropertyDocs.Where(x => x.ExpiryDateTo <= DateTime.Now).Count();
                var expirayIn7Day = dataContext.TblCompliancePropertyDocs.Where(x => x.ExpiryDateTo <= DateTime.Now.AddDays(7)).Count();
                var expirayIn28Day = dataContext.TblCompliancePropertyDocs.Where(x => x.ExpiryDateTo <= DateTime.Now.AddDays(28)).Count();

                data.MissingDoc = MissingCount.MissingDocConunt.ToString();
                data.ExpiredDoc = expiryDoc.ToString();
                data.ExpiredDoc28 = expirayIn28Day.ToString();
                data.ExpiredDoc7 = expirayIn7Day.ToString();


                //var result2 = dataContext.

            }

            return data;
        }
    }
}