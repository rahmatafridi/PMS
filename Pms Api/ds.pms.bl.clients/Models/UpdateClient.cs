using ds.pms.dal.Models;

namespace ds.pms.bl.clients.Models
{
    public class UpdateClient : Client
    {
        public bool IsActive { get; set; } // bit

        public static implicit operator UpdateClient(TblClient dbClient)
        {
            if (dbClient != null)
            {
                UpdateClient dlUpdateClient = new UpdateClient()
                {
                    Id = dbClient.Id,
                    Name = dbClient.Name,
                    Email = dbClient.Email,
                    Mobile = dbClient.Mobile,
                    Website = dbClient.Website,
                    Address1 = dbClient.Address1,
                    Address2 = dbClient.Address2,
                    Address3 = dbClient.Address3,
                    PostCode = dbClient.PostCode,
                    City = dbClient.City,
                    County = dbClient.County,
                    IsActive = dbClient.IsActive,
                };
                return dlUpdateClient;
            }
            return null;
        }

        public static implicit operator TblClient(UpdateClient dlUpdateClient)
        {
            if (dlUpdateClient != null)
            {
                TblClient dbClient = new TblClient()
                {
                    Id = dlUpdateClient.Id,
                    Name = dlUpdateClient.Name,
                    Email = dlUpdateClient.Email,
                    Mobile = dlUpdateClient.Mobile,
                    Website = dlUpdateClient.Website,
                    Address1 = dlUpdateClient.Address1,
                    Address2 = dlUpdateClient.Address2,
                    Address3 = dlUpdateClient.Address3,
                    PostCode = dlUpdateClient.PostCode,
                    City = dlUpdateClient.City,
                    County = dlUpdateClient.County,
                    IsActive = dlUpdateClient.IsActive,
                };
                return dbClient;
            }
            return null;
        }
    }
}
