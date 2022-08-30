using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.dal.CustomModels
{
  public  class Dashboard
    {
        public string TotalProperty { get; set; }
        public string AssignedProperty { get; set; }
        public string OccupancyProperty { get; set; }

        public string TotalTenant { get; set; }
        public string AssignedTenant { get; set; }
        public string OccupancyTenant { get; set; }

        public string TotalRoom { get; set; }
        public string AssignedRoom { get; set; }
        public string OccupancyRoom { get; set; }



        public string MissingDoc { get; set; }
        public string ExpiredDoc { get; set; }
        public string ExpiredDoc7 { get; set; }
        public string ExpiredDoc28 { get; set; }

    }
    public class MissingDoc
    {
        public int id { get; set; }
        public int propertyId { get; set; }
        public DateTime expiryDate { get; set; }
    }
}
